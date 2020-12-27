using Cosmos_Odyssey.Entities;
using Cosmos_Odyssey.Helpers;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System;


namespace Cosmos_Odyssey.Services
{
    public class ApiService : IApiService
    {
        private readonly IOptions<AppSettings> _appSettings;

        public ApiService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<Pricelist> GetPriceListAsync()
        {
            var uri = new Uri(_appSettings.Value.ApiUrl);
            using var httpClient = new HttpClient();
            using var httpResponse = await httpClient.GetAsync(uri);
            string response = await httpResponse.Content.ReadAsStringAsync();

            if(response.Equals("The service is unavailable."))
            {
                return null;
            }

            var priceList = JsonConvert.DeserializeObject<Pricelist>(response);

            return priceList;
        }
    }
}
