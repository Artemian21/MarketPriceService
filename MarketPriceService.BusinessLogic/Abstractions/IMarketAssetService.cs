using MarketPriceService.BusinessLogic.Models;

namespace MarketPriceService.BusinessLogic.Abstractions
{
    public interface IMarketAssetService
    {
        Task<List<Instrument>> GetAllAssetsAsync();
        Task<Instrument?> GetAssetBySymbolAsync(string symbol);
        Task<List<Instrument>> GetAssetsBySymbolsAsync(IEnumerable<string> symbols);
        Task UpdatePriceAsync(string instrumentId, decimal price, DateTime updateTime);
        Task AddAssetAsync(Instrument asset);
    }
}
