using Cosmos_Odyssey.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cosmos_Odyssey.Models
{
    public class BookingViewModel
    {
        [Required]
        [Display(Name = "From Planet")]
        public string FromPlanetName { get; set; }
        public SelectList FromPlanets { get; set; }

        [Required]
        [Display(Name = "To Planet")]
        public string ToPlanetName { get; set; }
        public SelectList ToPlanets { get; set; }

        public List<Provider> Providers { get; set; }

        public string CompanySearchString { get; set; }

        public string RouteInfoId { get; set; }
    }
}
