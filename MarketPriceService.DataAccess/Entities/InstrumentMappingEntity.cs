namespace MarketPriceService.DataAccess.Entities
{
    public class InstrumentMappingEntity
    {
        public Guid Id { get; set; }
        public string Provider { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;
        public int DefaultOrderSize { get; set; }
        public int? MaxOrderSize { get; set; }

        public TradingHoursEntity TradingHours { get; set; }

        public Guid InstrumentEntityId { get; set; }
        public InstrumentEntity Instrument { get; set; }
    }
}
