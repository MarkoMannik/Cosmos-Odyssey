using Microsoft.EntityFrameworkCore;
using Cosmos_Odyssey.Entities;

namespace Cosmos_Odyssey.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }

        public DbSet<From> From { get; set; }

        public DbSet<To> To { get; set; }

        public DbSet<RouteInfo> RouteInfo { get; set; }

        public DbSet<Company> Company { get; set; }

        public DbSet<Pricelist> PriceList { get; set; }

        public DbSet<Provider> Provider { get; set; }

        public DbSet<Reservation> Reservation { get; set; }

        public DbSet<Leg> Leg { get; set; }

        public DbSet<DemandingCustomer> DemandingCustomer { get; set; }
    }
}
