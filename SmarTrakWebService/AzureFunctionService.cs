using SmarTrakWebDomain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarTrakWebService
{
    public class AzureFunctionService : IAzureFunctionService
    {
        private readonly HttpClient _httpClient;

        public AzureFunctionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAzureFunctionData()
        {
            var response = await _httpClient.GetStringAsync("http://localhost:7120/api/users");
            return response;
        }
    }
}
