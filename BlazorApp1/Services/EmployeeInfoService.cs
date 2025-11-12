using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorApp1.Client.Models;
using BlazorApp1.Services;

namespace BlazorApp1.Client.Services
{
    public class EmployeeInfoService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenProvider _tokenProvider;

        public EmployeeInfoService(HttpClient httpClient, TokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
        }

        private async Task AddJwtHeaderAsync()
        {
            var token = await _tokenProvider.GetTokenAsync();
            if (!string.IsNullOrWhiteSpace(token))
            {
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // Delete Employee Profile
        public async Task DeleteProfileAsync(int userId)
        {
            await AddJwtHeaderAsync();
            var response = await _httpClient.DeleteAsync($"/api/Employee/Delete-Profile?userId={userId}");
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to delete profile. Status: {response.StatusCode}");
        }

        // Optional: Get Employee Info
        public async Task<EmployeeInfo> GetEmployeeInfoAsync(int userId)
        {
            await AddJwtHeaderAsync();
            return await _httpClient.GetFromJsonAsync<EmployeeInfo>($"/api/Employee/Get-Info?userId={userId}");
        }
    }
}
