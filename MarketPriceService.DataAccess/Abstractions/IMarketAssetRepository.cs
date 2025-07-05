using MarketPriceService.DataAccess.Entities;

namespace MarketPriceService.DataAccess.Abstractions
{
    public interface IMarketAssetRepository
    {
        Task AddAssetAsync(InstrumentEntity asset);
        Task<List<InstrumentEntity>> GetAllAssetsAsync();
        Task<InstrumentEntity?> GetAssetBySymbolAsync(string symbol);
        Task UpdatePriceAsync(string instrumentId, decimal price, DateTime updateTime);
    }
}
