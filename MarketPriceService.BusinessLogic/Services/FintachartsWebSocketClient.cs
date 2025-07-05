using MarketPriceService.BusinessLogic.Abstractions;
using MarketPriceService.BusinessLogic.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

public class FintachartsWebSocketClient : IFintachartsWebSocketClient
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<FintachartsWebSocketClient> _logger;
    private ClientWebSocket? _webSocket;
    private CancellationTokenSource? _cts;
    private AuthToken? _cachedToken;
    private DateTime _tokenReceivedAtUtc;

    public FintachartsWebSocketClient(
        IServiceProvider serviceProvider,
        ILogger<FintachartsWebSocketClient> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task ConnectAndListenAsync(List<Instrument> instrumentList, CancellationToken cancellationToken)
    {
        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        while (!_cts.Token.IsCancellationRequested)
        {
            try
            {
                ResetWebSocket();

                var token = await GetValidTokenAsync();
                _webSocket.Options.SetRequestHeader("Authorization", $"Bearer {token.AccessToken}");
                _webSocket.Options.SetRequestHeader("Sec-WebSocket-Protocol", "chat");
                _webSocket.Options.SetRequestHeader("Origin", "https://platform.fintacharts.com");
                _webSocket.Options.KeepAliveInterval = TimeSpan.FromSeconds(30);

                var webSocketUrl = "wss://platform.fintacharts.com/api/streaming/ws/v1/realtime/";
                await _webSocket.ConnectAsync(new Uri(webSocketUrl), _cts.Token);

                if (_webSocket.State != WebSocketState.Open)
                {
                    throw new InvalidOperationException($"WebSocket is not open. State: {_webSocket.State}");
                }

                _ = Task.Run(() => ReceiveMessages(_cts.Token), _cts.Token);

                await SubscribeToInstrumentAsync(instrumentList);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("[WS] Operation was cancelled");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[WS] WebSocket connection error");
            }

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(5), _cts.Token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }
    }

    private void ResetWebSocket()
    {
        try
        {
            _webSocket?.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[WS] Error disposing WebSocket");
        }

        _webSocket = new ClientWebSocket();
    }

    private async Task ReceiveMessages(CancellationToken cancellationToken)
    {
        var buffer = new byte[8192];

        try
        {
            while (_webSocket?.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken).ConfigureAwait(false);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", cancellationToken).ConfigureAwait(false);
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                ProcessMessage(message);
            }
        }
        catch (ObjectDisposedException)
        {
            _logger.LogWarning("[WS] WebSocket has been disposed");
        }
        catch (WebSocketException wsex)
        {
            _logger.LogError(wsex, "[WS] WebSocket error: {0}", wsex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[WS] Error receiving message: {0}", ex.Message);
        }
    }

    private void ProcessMessage(string message)
    {
        try
        {
            using (JsonDocument doc = JsonDocument.Parse(message))
            {
                var root = doc.RootElement;

                if (root.TryGetProperty("type", out var typeElement))
                {
                    var messageType = typeElement.GetString();

                    switch (messageType)
                    {
                        case "l1-update":
                            ProcessPriceUpdate(root);
                            break;
                        case "l1-snapshot":
                            ProcessSnapshot(root);
                            break;
                        case "session":
                            _logger.LogInformation($"[WS] Session established: {root.GetProperty("sessionId").GetString()}");
                            break;
                        default:
                            _logger.LogWarning($"[WS] Unknown message type: {messageType}. Message: {message}");
                            break;
                    }
                }
                else
                {
                    _logger.LogWarning($"[WS] Message without 'type' field: {message}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning($"[WS] Error processing message: {ex.Message} \nRaw message: {message}");
        }
    }

    private async Task ProcessPriceUpdate(JsonElement update)
    {
        try
        {
            var instrumentId = update.GetProperty("instrumentId").GetString();
            var provider = update.GetProperty("provider").GetString();

            if (update.TryGetProperty("last", out var lastElement))
            {
                if (!lastElement.TryGetProperty("price", out var priceProp) || !lastElement.TryGetProperty("timestamp", out var tsProp))
                {
                    _logger.LogWarning($"[WS] Incomplete 'last' data for instrument: {instrumentId}");
                    return;
                }

                var price = priceProp.GetDouble();
                var timestamp = tsProp.GetString();
                var localTime = DateTimeOffset.Parse(timestamp).ToLocalTime();

                _logger.LogInformation($"[WS] Price update: {price:F4} at {localTime:HH:mm:ss.fff} for {instrumentId} from provider {provider}");

                using var scope = _serviceProvider.CreateScope();
                var assetService = scope.ServiceProvider.GetRequiredService<IMarketAssetService>();
                await assetService.UpdatePriceAsync(instrumentId, (decimal)price, localTime.DateTime);
            }
            else
            {
                _logger.LogWarning($"[WS] 'last' property missing in update for {instrumentId}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[WS] Error processing price update");
        }
    }

    private void ProcessSnapshot(JsonElement snapshot)
    {
        try
        {
            if (snapshot.TryGetProperty("quote", out var quoteElement))
            {
                if (quoteElement.TryGetProperty("last", out var lastElement))
                {
                    var price = lastElement.GetProperty("price").GetDouble();
                    var timestamp = lastElement.GetProperty("timestamp").GetString();
                    var localTime = DateTimeOffset.Parse(timestamp).ToLocalTime();
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing snapshot: {ex.Message}");
        }
    }

    private async Task SubscribeToInstrumentAsync(List<Instrument> instrumentList)
    {
        foreach (var instrument in instrumentList)
        {
            try
            {
                if (instrument.Mappings == null || !instrument.Mappings.Any())
                {
                    continue;
                }

                var provider = instrument.Mappings.ContainsKey("simulation")
                    ? "simulation"
                    : instrument.Mappings.Keys.First();

                var subscription = new
                {
                    type = "l1-subscription",
                    instrumentId = instrument.Id,
                    provider = provider,
                    kinds = new[] { "last" },
                    subscribe = true
                };

                var json = JsonSerializer.Serialize(subscription);

                var buffer = Encoding.UTF8.GetBytes(json);
                await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, _cts.Token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[WS] Failed to subscribe to instrument {instrument.Symbol} ({instrument.Id})");
            }
        }
    }

    public async Task DisconnectAsync()
    {
        try
        {
            if (_webSocket?.State == WebSocketState.Open || _webSocket?.State == WebSocketState.CloseReceived)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[WS] Error while disconnecting");
        }
        finally
        {
            _cts?.Cancel();
            await Task.Delay(500);
            _webSocket?.Dispose();
            _webSocket = null;
        }
    }

    private async Task<AuthToken> GetValidTokenAsync()
    {
        if (_cachedToken != null && DateTime.UtcNow.AddSeconds(60) < _tokenReceivedAtUtc.AddSeconds(_cachedToken.ExpiresIn))
            return _cachedToken;

        using var scope = _serviceProvider.CreateScope();
        var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        _cachedToken = await authService.AuthenticateAsync();
        _tokenReceivedAtUtc = DateTime.UtcNow;
        return _cachedToken;
    }
}