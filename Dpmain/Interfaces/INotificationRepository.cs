using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetUnreadAsync();
        Task AddAsync(Notification notification);
        Task MarkAsReadAsync(int id);
    }
}
