using Cosmos_Odyssey.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cosmos_Odyssey.Services
{
    public interface IDataService
    {
        Task<List<string>> GetPlanetsFromAsync();

        Task<List<string>> GetPlanetsToAsync(string fromPlanetName);

        Task<string> GetRouteInfoId(string fromPlanetName, string toPlanetName);

        Task<List<Provider>> GetProvidersAsync(string routeInfoId, string companySearchString);

        Task<Provider> GetProviderAsync(string providerId);

        Task CreateNewReservationAsync(string firstName, string lastName, string providerId);

        Task<List<DemandingCustomer>> GetAllDemandingCustomersAsync();

        Task<List<Pricelist>> GetAllPricelistsAsync();
    }
}
