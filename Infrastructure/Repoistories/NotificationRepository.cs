using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;  // jekhane AppDbContext ache
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Add notification to DB
        public async Task AddAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        // ✅ Get all unread notifications, ordered by CreatedAt desc
        public async Task<List<Notification>> GetUnreadAsync()
        {
            return await _context.Notifications
                                 .Where(n => !n.IsRead)
                                 .OrderByDescending(n => n.CreatedAt)
                                 .ToListAsync();
        }

        // ✅ Mark a notification as read
        public async Task MarkAsReadAsync(int id)
        {
            var noti = await _context.Notifications.FindAsync(id);
            if (noti != null)
            {
                noti.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
