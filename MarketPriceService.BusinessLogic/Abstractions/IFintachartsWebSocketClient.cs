using MarketPriceService.BusinessLogic.Models;

namespace MarketPriceService.BusinessLogic.Abstractions
{
    public interface IFintachartsWebSocketClient
    {
        Task ConnectAndListenAsync(List<Instrument> instrumentList, CancellationToken cancellationToken);
        Task DisconnectAsync();
    }
}
