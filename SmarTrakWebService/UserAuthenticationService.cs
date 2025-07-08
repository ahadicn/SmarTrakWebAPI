using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using SmarTrakWebDomain.Services;

namespace SmarTrakWebService
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public UserAuthenticationService(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var tenantId = _config["GraphSettings:TenantId"];
            var clientId = _config["GraphSettings:ClientId"];
            var clientSecret = _config["GraphSettings:ClientSecret"];

            var body = new Dictionary<string, string>
        {
            { "client_id", clientId },
            { "client_secret", clientSecret },
            { "grant_type", "client_credentials" },
            { "scope", "https://graph.microsoft.com/.default" }
        };

            var res = await _httpClient.PostAsync(
                $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token",
                new FormUrlEncodedContent(body)
            );

            res.EnsureSuccessStatusCode();

            var json = await res.Content.ReadAsStringAsync();
            return JObject.Parse(json)["access_token"]?.ToString();
        }

        public async Task<bool> IsRoleAlreadyAssignedAsync(string userId)
        {
            var token = await GetAccessTokenAsync();
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://graph.microsoft.com/v1.0/users/{userId}/appRoleAssignments"
            );
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var assignments = json["value"];

            var targetRoleId = _config["GraphSettings:AppRoleId"];

            return assignments.Any(a => a["appRoleId"]?.ToString() == targetRoleId);
        }

        public async Task AssignRoleIfNotExistsAsync(string userId)
            {
            if (await IsRoleAlreadyAssignedAsync(userId))
                return; // Role already assigned

            var token = await GetAccessTokenAsync();

            var payload = new
            {
                principalId = userId,
                resourceId = _config["GraphSettings:ResourceId"],
                appRoleId = _config["GraphSettings:AppRoleId"]
            };

            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(payload),
                Encoding.UTF8,
                "application/json"
            );

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://graph.microsoft.com/v1.0/users/{userId}/appRoleAssignments"
            );
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                throw new Exception($"Graph API role assignment failed: {err}");
            }
        }

    }
}
