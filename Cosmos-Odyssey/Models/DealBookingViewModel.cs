using Cosmos_Odyssey.Entities;
using System.ComponentModel.DataAnnotations;

namespace Cosmos_Odyssey.Models
{
    public class DealBookingViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        public Provider Provider { get; set; }
    }
}
