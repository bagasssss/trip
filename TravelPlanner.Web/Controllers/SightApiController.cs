using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelPlanner.BusinessLogic.Interfaces;

namespace TravelPlanner.Web.Controllers
{
    [Authorize]
    public class SightApiController : Controller
    {
        private readonly ISightObjectService _sightObjectService;

        public SightApiController(ISightObjectService sightObjectService)
        {
            _sightObjectService = sightObjectService;
        }

        [Route("api/sight/get")]
        [HttpGet]
        public async Task<IActionResult> GetAllSights()
        { 
            var sights = await _sightObjectService.Get();
            return Ok(sights);
        }
    }
}