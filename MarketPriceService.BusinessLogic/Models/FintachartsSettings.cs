namespace MarketPriceService.BusinessLogic.Models
{
    public class FintachartsSettings
    {
        public string AuthUrl { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string UriRest { get; set; } = string.Empty;
        public string UriWebSocket { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
