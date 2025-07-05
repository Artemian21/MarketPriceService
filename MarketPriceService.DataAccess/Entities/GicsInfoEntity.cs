namespace MarketPriceService.DataAccess.Entities
{
    public class GicsInfoEntity
    {
        public Guid Id { get; set; }
        public int SectorId { get; set; }
        public int IndustryGroupId { get; set; }
        public int IndustryId { get; set; }
        public int SubIndustryId { get; set; }

        public Guid InstrumentProfileEntityId { get; set; }
        public InstrumentProfileEntity Profile { get; set; }
    }
}
