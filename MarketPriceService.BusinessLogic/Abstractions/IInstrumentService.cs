using MarketPriceService.BusinessLogic.Models;

namespace MarketPriceService.BusinessLogic.Abstractions
{
    public interface IInstrumentService
    {
        Task<List<Instrument>> GetInstrumentsAsync(int count = 5);
    }
}