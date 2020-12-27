using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cosmos_Odyssey.Entities
{
    public class Pricelist
    {
        [Key]
        public string Id { get; set; }
        public DateTime ValidUntil { get; set; }
        public bool Ready { get; set; }
        public ICollection<Leg> Legs { get; set; }

        public string Status
        {
            get
            {
                return ValidUntil > DateTime.Now ? "Active" : "Expired";
            }
        }
    }
}
