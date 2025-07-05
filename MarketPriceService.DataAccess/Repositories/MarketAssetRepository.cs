using MarketPriceService.DataAccess.Abstractions;
using MarketPriceService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketPriceService.DataAccess.Repositories
{
    public class MarketAssetRepository : IMarketAssetRepository
    {
        private readonly MarketPriceDbContext _context;

        public MarketAssetRepository(MarketPriceDbContext context)
        {
            _context = context;
        }

        public async Task<List<InstrumentEntity>> GetAllAssetsAsync()
        {
            return await _context.Instruments
                .AsNoTracking()
                .Include(i => i.Mappings)
                .Include(i => i.Profile)
                    .ThenInclude(p => p.Gics)
                .ToListAsync();
        }

        public async Task<InstrumentEntity?> GetAssetBySymbolAsync(string symbol)
        {
            return await _context.Instruments
                .Include(i => i.Mappings)
                .Include(i => i.Profile)
                    .ThenInclude(p => p.Gics)
                .FirstOrDefaultAsync(a => a.Symbol == symbol);
        }

        public async Task UpdatePriceAsync(string instrumentId, decimal price, DateTime updateTime)
        {
            try
            {
                var allAssets = await _context.Instruments.AsNoTracking().ToListAsync();
                var targetAsset = await _context.Instruments.FirstOrDefaultAsync(a => a.InstrumentId == instrumentId);

                if (targetAsset != null)
                {
                    targetAsset.LastPrice = price;
                    targetAsset.LastUpdatedAt = updateTime;

                    await _context.SaveChangesAsync();
                }
                else
                {
                    Console.WriteLine($"[DB][WARNING] No asset found with InstrumentId = {instrumentId}. Cannot update.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB][ERROR] Failed to update asset with InstrumentId {instrumentId}: {ex.Message}");
                throw;
            }
        }

        public async Task AddAssetAsync(InstrumentEntity asset)
        {
            try
            {
                _context.Instruments.Add(asset);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB][ERROR] Failed to add asset {asset.Symbol}: {ex.Message}");
                throw;
            }
        }
    }
}
