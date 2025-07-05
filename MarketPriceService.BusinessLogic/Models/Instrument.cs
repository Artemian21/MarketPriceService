using System.Text.Json.Serialization;

namespace MarketPriceService.BusinessLogic.Models
{
    public class Instrument
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }


        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("tickSize")]
        public double TickSize { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        public decimal LastPrice { get; set; }
        public DateTime LastUpdatedAt { get; set; }


        [JsonPropertyName("mappings")]
        public Dictionary<string, InstrumentMapping> Mappings { get; set; }

        [JsonPropertyName("profile")]
        public InstrumentProfile Profile { get; set; }
    }
}
