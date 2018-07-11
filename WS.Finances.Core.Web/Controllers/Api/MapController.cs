using Microsoft.AspNetCore.Mvc;
using WS.Finances.Core.Lib.Services;

namespace WS.Finances.Core.Web.Controllers.Api
{
    [Route("api/[controller]")]
    public class MapController : Controller
    {
        private readonly MapService _mapService;

        public MapController(MapService mapService)
        {
            _mapService = mapService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_mapService.Get());
        }
    }
}
