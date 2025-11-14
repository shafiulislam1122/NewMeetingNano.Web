using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp1.Client.Models; // Notification model

namespace BlazorApp1.Services

{
    public interface INotificationService
    {
        Task<List<Notification>> GetUnreadNotificationsAsync();
        Task MarkAsReadAsync(int id);
    }
}
