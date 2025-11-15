using Domain.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Employee")] // Both roles can access
    public class MeetingRoomController : ControllerBase
    {
        private readonly IMeetingRoomRepository _meetingRoomRepository;
        private readonly INotificationRepository _notificationRepository;

        public MeetingRoomController(
            IMeetingRoomRepository meetingRoomRepository,
            INotificationRepository notificationRepository)
        {
            _meetingRoomRepository = meetingRoomRepository;
            _notificationRepository = notificationRepository;
        }

        [HttpGet("MeetingRoomList")]
        public async Task<IActionResult> GetMeetingRooms()
        {
            var rooms = await _meetingRoomRepository.GetAllAsync();
            return Ok(rooms);
        }

        // ✅ New: Create Meeting + Notification
        [HttpPost("CreateMeeting")]
        public async Task<IActionResult> CreateMeeting([FromBody] MeetingRoom room)
        {
            // 1️⃣ Add meeting to DB
            var meetingId = await _meetingRoomRepository.AddAsync(room);

            if (meetingId > 0)
            {
                // 2️⃣ Create notification
                var notification = new Notification
                {
                    Message = $"New meeting created by {User.Identity.Name}",
                    CreatedAt = DateTime.Now,
                    IsRead = false
                };
                await _notificationRepository.AddAsync(notification);

                return Ok(new { Success = true, Message = "Meeting created & notification sent" });
            }

            return BadRequest(new { Success = false, Message = "Failed to create meeting" });
        }
    }
}
