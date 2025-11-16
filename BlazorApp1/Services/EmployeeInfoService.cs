using System; // Exception ব্যবহারের জন্য এটি প্রয়োজন
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorApp1.Client.Models;

namespace BlazorApp1.Client.Services
{
    public class EmployeeInfoService
    {
        private readonly HttpClient _httpClient;

        public EmployeeInfoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Update profile
        // এখন 'cmd' (UpdateProfileCommand) এ শুধু Username এবং Password আছে, UserId নেই।
        public async Task UpdateProfileAsync(UpdateProfileCommand cmd)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/Employee/Update-Profile", cmd);

            if (!response.IsSuccessStatusCode)
            {
                // সার্ভার থেকে এরর মেসেজ পড়ার জন্য:
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Failed to update profile. Status: {response.StatusCode}. Details: {errorContent}");
            }
        }

        // Get employee info (যদি প্রয়োজন হয়)
        public async Task<EmployeeInfo> GetEmployeeInfoAsync(int userId)
        {
            // ধরে নেওয়া হচ্ছে EmployeeInfo ক্লাসটি আপনার Models ফোল্ডারে আছে
            return await _httpClient.GetFromJsonAsync<EmployeeInfo>($"/api/Employee/Get-Info?userId={userId}");
        }
    }
}