using Cosmos_Odyssey.Entities;
using Cosmos_Odyssey.Helpers;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace Cosmos_Odyssey.Services
{
    public class ApiService : IApiService
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly HttpClient _httpClient;

        public ApiService(IOptions<AppSettings> appSettings, HttpClient httpClient)
        {
            _appSettings = appSettings;
            _httpClient = httpClient;
        }

        public async Task<Pricelist> GetPriceListAsync()
        {
            var uri = new Uri(_appSettings.Value.ApiUrl);
            using var httpResponse = await _httpClient.GetAsync(uri);
            var response = await httpResponse.Content.ReadAsStringAsync();

            if(response.Equals("The service is unavailable."))
            {
                return null;
            }

            var priceList = JsonConvert.DeserializeObject<Pricelist>(response);

            return priceList;
        }
    }
}
