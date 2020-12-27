using Cosmos_Odyssey.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Cosmos_Odyssey.Models;

namespace Cosmos_Odyssey.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IDataService _dataService;

        public ReservationController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpPost("CreateReservation/{providerId}")]
        public async Task<IActionResult> CreateReservation(string providerId, string firstName, string lastName)
        {
            await _dataService.CreateNewReservationAsync(firstName, lastName, providerId);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new ReservationViewModel()
            {
                DemandingCustomers = await _dataService.GetAllDemandingCustomersAsync()
            };

            return View(model);
        }
    }
}
