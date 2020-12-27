using Cosmos_Odyssey.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Cosmos_Odyssey.Models
{
    public class BookingViewModel
    {
        public string FromPlanetName { get; set; }
        public SelectList FromPlanets { get; set; }

        public string ToPlanetName { get; set; }
        public SelectList ToPlanets { get; set; }

        public List<Provider> Providers { get; set; }

        public string CompanySearchString { get; set; }

        public string RouteInfoId { get; set; }
    }
}
