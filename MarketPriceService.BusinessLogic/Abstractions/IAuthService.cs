using MarketPriceService.BusinessLogic.Models;

namespace MarketPriceService.BusinessLogic.Abstractions
{
    public interface IAuthService
    {
        Task<AuthToken> AuthenticateAsync();
    }
}
