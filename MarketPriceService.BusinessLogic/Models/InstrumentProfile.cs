using System.Text.Json.Serialization;

namespace MarketPriceService.BusinessLogic.Models
{
    public class InstrumentProfile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("gics")]
        public GicsInfo Gics { get; set; }
    }
}
