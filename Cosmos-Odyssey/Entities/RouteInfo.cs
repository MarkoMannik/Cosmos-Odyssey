
using System.ComponentModel.DataAnnotations;

namespace Cosmos_Odyssey.Entities
{
    public class RouteInfo
    {
        [Key]
        public string Id { get; set; }
        public long Distance { get; set; }
        public string FromId { get; set; }
        public From From { get; set; }
        public string ToId { get; set; }
        public To To { get; set; }
    }
}
