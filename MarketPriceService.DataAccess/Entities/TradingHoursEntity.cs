namespace MarketPriceService.DataAccess.Entities
{
    public class TradingHoursEntity
    {
        public Guid Id { get; set; }
        public string RegularStart { get; set; }
        public string RegularEnd { get; set; }
        public string ElectronicStart { get; set; }
        public string ElectronicEnd { get; set; }
    }
}
