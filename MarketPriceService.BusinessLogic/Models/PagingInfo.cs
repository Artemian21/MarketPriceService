using System.Text.Json.Serialization;

namespace MarketPriceService.BusinessLogic.Models
{
    public class PagingInfo
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("pages")]
        public int Pages { get; set; }

        [JsonPropertyName("items")]
        public int Items { get; set; }
    }
}
