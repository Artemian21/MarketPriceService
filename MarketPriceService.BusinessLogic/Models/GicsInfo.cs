using System.Text.Json.Serialization;

namespace MarketPriceService.BusinessLogic.Models
{
    public class GicsInfo
    {
        [JsonPropertyName("sectorId")]
        public int SectorId { get; set; }

        [JsonPropertyName("industryGroupId")]
        public int IndustryGroupId { get; set; }

        [JsonPropertyName("industryId")]
        public int IndustryId { get; set; }

        [JsonPropertyName("subIndustryId")]
        public int SubIndustryId { get; set; }
    }
}
