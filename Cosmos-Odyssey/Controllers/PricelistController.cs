using Microsoft.AspNetCore.Mvc;
using Cosmos_Odyssey.Services;
using System.Threading.Tasks;

namespace Cosmos_Odyssey.Controllers
{
    public class PricelistController : Controller
    {
        private readonly IDataService _dataService;

        public PricelistController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _dataService.GetAllPricelistsAsync());
        }
    }
}
