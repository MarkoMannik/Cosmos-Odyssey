
using System;
using System.ComponentModel.DataAnnotations;

namespace Cosmos_Odyssey.Entities
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Created When")]
        [DataType(DataType.Date)]
        public DateTime TimeStamp { get; set; }

        public string ProviderId { get; set; }

        public Provider Provider { get; set; }

        public int DemandingCustomerId { get; set; }

        public DemandingCustomer DemandingCustomer { get; set; }
    }
}