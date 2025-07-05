using System.Text.Json.Serialization;

namespace MarketPriceService.BusinessLogic.Models
{
    public class TradingHours
    {
        [JsonPropertyName("regularStart")]
        public string RegularStart { get; set; }

        [JsonPropertyName("regularEnd")]
        public string RegularEnd { get; set; }

        [JsonPropertyName("electronicStart")]
        public string ElectronicStart { get; set; }

        [JsonPropertyName("electronicEnd")]
        public string ElectronicEnd { get; set; }
    }
}
