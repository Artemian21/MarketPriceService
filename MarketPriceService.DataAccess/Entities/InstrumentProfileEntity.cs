namespace MarketPriceService.DataAccess.Entities
{
    public class InstrumentProfileEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        public GicsInfoEntity Gics { get; set; }

        public Guid InstrumentEntityId { get; set; }
        public InstrumentEntity Instrument { get; set; }
    }
}
