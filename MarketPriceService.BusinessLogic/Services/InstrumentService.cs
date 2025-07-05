using MarketPriceService.BusinessLogic.Abstractions;
using MarketPriceService.BusinessLogic.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace MarketPriceService.BusinessLogic.Services
{
    public class InstrumentService : IInstrumentService
    {
        private readonly HttpClient _httpClient;
        private readonly FintachartsSettings _settings;
        private readonly IAuthService _authService;
        private readonly IMarketAssetService _marketAssetService;

        public InstrumentService(
            IHttpClientFactory httpClientFactory,
            IOptions<FintachartsSettings> options,
            IAuthService authService,
            IMarketAssetService marketAssetService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _settings = options.Value;
            _authService = authService;
            _marketAssetService = marketAssetService;
        }

        public async Task<List<Instrument>> GetInstrumentsAsync(int count = 5)
        {
            try
            {
                var token = await _authService.AuthenticateAsync();

                var url = $"{_settings.UriRest}/api/instruments/v1/instruments?provider=simulation&size={count}";
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.AccessToken);

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var instrumentResponse = JsonSerializer.Deserialize<InstrumentResponse>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                var instrumentsFromApi = instrumentResponse?.Data ?? new List<Instrument>();

                var result = new List<Instrument>();

                foreach (var instrument in instrumentsFromApi)
                {
                    instrument.Exchange ??= "";
                    instrument.Profile.Location ??= "";

                    var existing = await _marketAssetService.GetAssetBySymbolAsync(instrument.Symbol);
                    if (existing == null)
                    {
                        await _marketAssetService.AddAssetAsync(instrument);
                        result.Add(instrument);
                    }
                    else
                    {
                        result.Add(existing);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[API] Failed to get instruments: {ex.Message}");
                return new List<Instrument>();
            }
        }
    }
}
