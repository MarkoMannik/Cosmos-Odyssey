using Cosmos_Odyssey.Entities;
using System.Threading.Tasks;

namespace Cosmos_Odyssey.Services
{
    public interface IApiService
    {
        public Task<Pricelist> GetPriceListAsync();
    }
}
