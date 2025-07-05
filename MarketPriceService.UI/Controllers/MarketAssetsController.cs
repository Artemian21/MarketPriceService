using MarketPriceService.BusinessLogic.Abstractions;
using MarketPriceService.BusinessLogic.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketPriceService.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketAssetsController : ControllerBase
    {
        private readonly IMarketAssetService _marketAssetService;

        public MarketAssetsController(IMarketAssetService marketAssetService)
        {
            _marketAssetService = marketAssetService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Instrument>>> GetAllAssets()
        {
            var assets = await _marketAssetService.GetAllAssetsAsync();
            return Ok(assets);
        }

        [HttpGet("{symbol}")]
        public async Task<ActionResult<Instrument>> GetAssetBySymbol(string symbol)
        {
            var asset = await _marketAssetService.GetAssetBySymbolAsync(symbol);
            if (asset == null)
                return NotFound();

            return Ok(asset);
        }
    }
}
