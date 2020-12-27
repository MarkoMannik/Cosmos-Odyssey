using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Cosmos_Odyssey.Entities
{
    public class DemandingCustomer
    {
        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Reservation> Reservations { get; set; }

        public decimal TotalQuotedPrice
        {
            get
            {
                return Reservations.Any() ? Math.Round(Reservations.Sum(x => x.Provider.Price), 2, MidpointRounding.AwayFromZero) : 0;
            }
        }

        public int TotalQuotedTravelTime
        {
            get
            {
                return Reservations.Any() ? Convert.ToInt32(Reservations.Sum(x => (x.Provider.FlightEnd - x.Provider.FlightStart).Hours)) : 0;
            }
        }
    }
}
