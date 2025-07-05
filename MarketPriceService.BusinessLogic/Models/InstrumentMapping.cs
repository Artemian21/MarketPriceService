using System.Text.Json.Serialization;

namespace MarketPriceService.BusinessLogic.Models
{
    public class InstrumentMapping
    {
        [JsonIgnore]
        public string Provider { get; set; }
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        [JsonPropertyName("defaultOrderSize")]
        public int DefaultOrderSize { get; set; }

        [JsonPropertyName("maxOrderSize")]
        public int? MaxOrderSize { get; set; }

        [JsonPropertyName("tradingHours")]
        public TradingHours TradingHours { get; set; }
    }
}
