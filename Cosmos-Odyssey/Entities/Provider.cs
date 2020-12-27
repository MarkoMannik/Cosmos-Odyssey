using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cosmos_Odyssey.Entities
{
    public class Provider
    {
        [Key]
        public string Id { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public DateTime FlightStart { get; set; }
        public DateTime FlightEnd { get; set; }
        public string LegId { get; set; }
        public Leg Leg { get; set; }

        public int QuotedTravelTime
        {
            get
            {
                return Convert.ToInt32((FlightEnd - FlightStart).Hours);
            }
        }
    }
}
