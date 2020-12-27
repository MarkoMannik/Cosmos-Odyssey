using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Cosmos_Odyssey.Services;
using Cosmos_Odyssey.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Cosmos_Odyssey.Controllers
{
    [Route("[controller]")]
    public class BookingController : Controller
    {
        private readonly IDataService _dataService;
        public BookingController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<IActionResult> FromPlanet()
        {
            var model = new BookingViewModel
            {
                FromPlanets = new SelectList(await _dataService.GetPlanetsFromAsync())
            };

            return View(model);
        }

        [HttpGet("ToPlanet")]
        public async Task<IActionResult> ToPlanet(string fromPlanetName)
        {
            var model = new BookingViewModel
            {
                FromPlanetName = fromPlanetName,
                ToPlanets = new SelectList(await _dataService.GetPlanetsToAsync(fromPlanetName))
            };
            return View(model);
        }

        [HttpGet("Route")]
        [Route("Route/{fromPlanetName}")]
        public async Task<IActionResult> Route(string fromPlanetName, string toPlanetName)
        {
            var routeInfoId = await _dataService.GetRouteInfoId(fromPlanetName, toPlanetName);
            return RedirectToAction("AvailableDeals", new {routeInfoId });
        }

        [HttpGet("AvailableDeals")]
        [Route("AvailableDeals/{routeinfoId}")]
        public async Task<IActionResult> AvailableDeals(string routeInfoId, string companySearchString)
        {
            var providers = await _dataService.GetProvidersAsync(routeInfoId, companySearchString);
            var model = new BookingViewModel
            {
                FromPlanetName = providers.Select(x=>x.Leg.RouteInfo.From.Name).FirstOrDefault(),
                ToPlanetName = providers.Select(x => x.Leg.RouteInfo.To.Name).FirstOrDefault(),
                RouteInfoId = routeInfoId,
                Providers = providers
            };

            return View(model);
        }

        [HttpGet]
        [Route("DealBooking/{providerId}")]
        public async Task<IActionResult> DealBooking(string providerId)
        {
            var model = new DealBookingViewModel
            {
                Provider = await _dataService.GetProviderAsync(providerId)
            };

            return View(model);
        }
    }
}
