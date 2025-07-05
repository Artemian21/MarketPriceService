namespace MarketPriceService.DataAccess.Entities
{
    public class InstrumentEntity
    {
        public Guid Id { get; set; }
        public string InstrumentId { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Kind { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double TickSize { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal LastPrice { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public ICollection<InstrumentMappingEntity> Mappings { get; set; } = new List<InstrumentMappingEntity>();
        public InstrumentProfileEntity Profile { get; set; }
    }
}
