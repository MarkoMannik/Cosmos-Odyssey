using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cosmos_Odyssey.Data;
using Cosmos_Odyssey.Entities;
using Microsoft.EntityFrameworkCore;


namespace Cosmos_Odyssey.Services
{
    public class DataService : IDataService
    {
        private readonly DatabaseContext _databaseContext;

        public DataService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<List<string>> GetPlanetsFromAsync()
        {
            while (!await _databaseContext.PriceList.AnyAsync(x => x.ValidUntil > DateTime.Now && x.Ready))
            {
                Thread.Sleep(500);
            }

            return await _databaseContext.Provider
                .Where(x => x.Leg.Pricelist.ValidUntil >= DateTime.Now)
                .Select(x => x.Leg.RouteInfo.From.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<string>> GetPlanetsToAsync(string fromPlanetName)
        {
            return await _databaseContext.Provider
                .Where(x => x.Leg.Pricelist.ValidUntil >= DateTime.Now && x.Leg.RouteInfo.From.Name == fromPlanetName)
                .Select(x => x.Leg.RouteInfo.To.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<string> GetRouteInfoId(string fromPlanetName, string toPlanetName)
        {
            return await _databaseContext.Leg
                .Where(x => x.Pricelist.ValidUntil >= DateTime.Now && x.RouteInfo.From.Name == fromPlanetName && x.RouteInfo.To.Name == toPlanetName)
                .Select(x => x.RouteInfo.Id).FirstOrDefaultAsync();
        }

        public async Task<List<Provider>> GetProvidersAsync(string routeInfoId, string companySearchString)
        {
            var providers = from p in _databaseContext.Provider
                            select p;

            providers = providers.Where(x => x.Leg.Pricelist.ValidUntil >= DateTime.Now && x.Leg.RouteInfo.Id == routeInfoId);

            if (!string.IsNullOrEmpty(companySearchString))
            {
                providers = providers.Where(x => x.Company.Name.Contains(companySearchString));
            }

            return await providers.OrderByDescending(x => x.Leg.Pricelist.ValidUntil)
                .Include(x => x.Company)
                .Include(x => x.Leg)
                .ThenInclude(x => x.RouteInfo)
                .ThenInclude(x => x.From)
                .Include(x => x.Leg)
                .ThenInclude(x => x.RouteInfo)
                .ThenInclude(x => x.To)
                .ToListAsync();
        }

        public async Task<Provider> GetProviderAsync(string providerId)
        {
            var provider = await _databaseContext.Provider
                .Where(x => x.Leg.Pricelist.ValidUntil >= DateTime.Now && x.Id == providerId)
                .OrderByDescending(x => x.Leg.Pricelist.ValidUntil)
                .Include(x => x.Company)
                .Include(x => x.Leg)
                .ThenInclude(x => x.RouteInfo)
                .ThenInclude(x => x.From)
                .Include(x => x.Leg)
                .ThenInclude(x => x.RouteInfo)
                .ThenInclude(x => x.To)
                .FirstOrDefaultAsync();

            return provider;
        }

        public async Task CreateNewReservationAsync(string firstName, string lastName, string providerId)
        {
            if (await _databaseContext.Provider.AnyAsync(x => x.Leg.Pricelist.ValidUntil >= DateTime.Now && x.Id == providerId) == false)
                return;

            var demandingCustomer = await _databaseContext.DemandingCustomer
                .FirstOrDefaultAsync(x => (x.FirstName ?? "").ToUpper() == firstName && (x.LastName ?? "").ToUpper() == lastName);

            if (demandingCustomer == null)
            {
                demandingCustomer = new DemandingCustomer { FirstName = firstName, LastName = lastName };
                await _databaseContext.DemandingCustomer.AddAsync(demandingCustomer);
                await _databaseContext.SaveChangesAsync();
            }

            await _databaseContext.Reservation.AddAsync(new Reservation
            {
                ProviderId = providerId,
                TimeStamp = DateTime.Now,
                DemandingCustomerId = demandingCustomer.Id
            });

            await _databaseContext.SaveChangesAsync();
        }

        public async Task<List<DemandingCustomer>> GetAllDemandingCustomersAsync()
        {
            return await _databaseContext.DemandingCustomer
                .Include(x => x.Reservations.Where(r => r.Provider.Leg.Pricelist.ValidUntil >= DateTime.Now))
                .ThenInclude(x => x.Provider)
                .ThenInclude(x => x.Company)
                .Include(x => x.Reservations.Where(r => r.Provider.Leg.Pricelist.ValidUntil >= DateTime.Now))
                .ThenInclude(x => x.Provider.Leg.RouteInfo.From)
                .Include(x => x.Reservations.Where(r => r.Provider.Leg.Pricelist.ValidUntil >= DateTime.Now))
                .ThenInclude(x => x.Provider.Leg.RouteInfo.To)
                .ToListAsync();
        }

        public async Task<List<Pricelist>> GetAllPricelistsAsync()
        {
            return await _databaseContext.PriceList.OrderByDescending(x => x.ValidUntil).ToListAsync();
        }
    }
}
