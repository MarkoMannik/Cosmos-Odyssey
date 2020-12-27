using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cosmos_Odyssey.Entities
{
    public class Leg
    {
        [Key]
        public string Id { get; set; }
        public string PriceListId { get; set; }
        public Pricelist Pricelist { get; set; }
        public string RouteInfoId { get; set; }
        public RouteInfo RouteInfo { get; set; }
        public ICollection<Provider> Providers { get; set; }
    }
}
