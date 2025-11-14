using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorApp1.Client.Models;


namespace BlazorApp1.Services
{
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;

        public NotificationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Notification>> GetUnreadNotificationsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Notification>>("/api/Notification/Unread");
        }

        public async Task MarkAsReadAsync(int id)
        {
            await _httpClient.PostAsJsonAsync($"/api/Notification/MarkAsRead/{id}", new { Id = id });
        }
    }
}
