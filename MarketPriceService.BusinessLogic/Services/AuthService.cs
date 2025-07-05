using MarketPriceService.BusinessLogic.Abstractions;
using MarketPriceService.BusinessLogic.Models;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace MarketPriceService.BusinessLogic.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly FintachartsSettings _settings;
        private string _accessToken;
        private DateTime _tokenExpiry;

        public AuthService(HttpClient httpClient, IOptions<FintachartsSettings> options)
        {
            _httpClient = httpClient;
            _settings = options.Value;
        }

        public async Task<AuthToken> AuthenticateAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpiry)
            {
                return new AuthToken
                {
                    AccessToken = _accessToken,
                    ExpiresIn = (int)(_tokenExpiry - DateTime.UtcNow).TotalSeconds
                };
            }

            var requestData = new Dictionary<string, string>
            {
                ["grant_type"] = "password",
                ["client_id"] = _settings.ClientId,
                ["username"] = _settings.Username,
                ["password"] = _settings.Password
            };

            var content = new FormUrlEncodedContent(requestData);
            var response = await _httpClient.PostAsync(_settings.AuthUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Authentication failed: {response.StatusCode}. Response: {errorContent}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<AuthToken>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            _accessToken = token.AccessToken;
            _tokenExpiry = DateTime.UtcNow.AddSeconds(token.ExpiresIn - 60);

            return token;
        }
    }
}
