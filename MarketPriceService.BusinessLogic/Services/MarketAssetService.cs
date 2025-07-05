using AutoMapper;
using MarketPriceService.BusinessLogic.Abstractions;
using MarketPriceService.BusinessLogic.Models;
using MarketPriceService.DataAccess.Abstractions;
using MarketPriceService.DataAccess.Entities;

namespace MarketPriceService.BusinessLogic.Services
{
    public class MarketAssetService : IMarketAssetService
    {
        private readonly IMarketAssetRepository _repository;
        private readonly IMapper _mapper;

        public MarketAssetService(
            IMarketAssetRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<Instrument>> GetAllAssetsAsync()
        {
            var entities = await _repository.GetAllAssetsAsync();
            return _mapper.Map<List<Instrument>>(entities);
        }

        public async Task<Instrument?> GetAssetBySymbolAsync(string symbol)
        {
            var entity = await _repository.GetAssetBySymbolAsync(symbol);
            return entity is null ? null : _mapper.Map<Instrument>(entity);
        }

        public async Task<List<Instrument>> GetAssetsBySymbolsAsync(IEnumerable<string> symbols)
        {
            var all = await _repository.GetAllAssetsAsync();
            var filtered = all.Where(a => symbols.Contains(a.Symbol)).ToList();
            return _mapper.Map<List<Instrument>>(filtered);
        }

        public async Task UpdatePriceAsync(string instrumentId, decimal price, DateTime updateTime)
        {
            await _repository.UpdatePriceAsync(instrumentId, price, updateTime);
        }

        public async Task AddAssetAsync(Instrument asset)
        {
            var entity = _mapper.Map<InstrumentEntity>(asset);
            await _repository.AddAssetAsync(entity);
        }
    }
}