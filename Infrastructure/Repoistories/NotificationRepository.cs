using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data; // যেটা আপনার DapperContext
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DapperContext _context;

        public NotificationRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Notification notification)
        {
            var query = "INSERT INTO Notifications (Message, CreatedAt, IsRead) VALUES (@Message, @CreatedAt, @IsRead)";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(query, notification);
        }

        public async Task<List<Notification>> GetUnreadAsync()
        {
            var query = "SELECT * FROM Notifications WHERE IsRead = 0 ORDER BY CreatedAt DESC";
            using var conn = _context.CreateConnection();
            var result = await conn.QueryAsync<Notification>(query);
            return result.AsList();
        }

        public async Task MarkAsReadAsync(int id)
        {
            var query = "UPDATE Notifications SET IsRead = 1 WHERE Id = @Id";
            using var conn = _context.CreateConnection();
            await conn.ExecuteAsync(query, new { Id = id });
        }
    }
}
