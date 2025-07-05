using System.Text.Json.Serialization;

namespace MarketPriceService.BusinessLogic.Models
{
    public class InstrumentResponse
    {
        [JsonPropertyName("paging")]
        public PagingInfo Paging { get; set; }

        [JsonPropertyName("data")]
        public List<Instrument> Data { get; set; }
    }
}
