using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _repo;

        public NotificationController(INotificationRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("Unread")]
        public async Task<ActionResult<List<Notification>>> GetUnread()
        {
            var notifications = await _repo.GetUnreadAsync();
            return Ok(notifications);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> AddNotification([FromBody] Notification notification)
        {
            await _repo.AddAsync(notification);
            return Ok("Notification Added");
        }

        [HttpPut("MarkAsRead/{id}")]
        public async Task<ActionResult> MarkAsRead(int id)
        {
            await _repo.MarkAsReadAsync(id);
            return Ok("Notification marked as read");
        }
    }
}
