using System;
using System.Net.Http;
using System.Threading.Tasks;
using MJonesMmtTest.Api.Models.Config;
using MJonesMmtTest.Api.Models.Dto;
using Newtonsoft.Json;

namespace MJonesMmtTest.Api.Services
{
    public class CustomerService : ICustomerService
    {
        // Create a single, static HttpClient
        private static readonly HttpClient HttpClient = new() { Timeout = TimeSpan.FromMinutes(5) };
        private readonly AppConfiguration _config;

        public CustomerService(AppConfiguration config)
        {
            _config = config;
        }

        public async Task<ApiCustomer> GetCustomerAsync(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
                return null;

            var resourceLocation = string.Concat(_config.ApiEndpoint, "/GetUserDetails?code=", _config.ApiKey, "&email=",
                emailAddress);

            var customerSearchResult = await MakeGetRequest(resourceLocation);
            return string.IsNullOrWhiteSpace(customerSearchResult) ? null : JsonConvert.DeserializeObject<ApiCustomer>(customerSearchResult);
        }

        private static async Task<string> MakeGetRequest(string resource)
        {
            var response = string.Empty;
            var apiResponse = await HttpClient.GetAsync(resource);
            if (apiResponse.IsSuccessStatusCode)
                response = apiResponse.Content.ReadAsStringAsync().Result;
            
            return response;
        }
    }
}
