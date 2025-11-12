using Application.DTOs;
using Application.Commands;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IMeetingRoomRepository _meetingRoomRepository;

        public AdminController(IMeetingRoomRepository meetingRoomRepository)
        {
            _meetingRoomRepository = meetingRoomRepository;
        }

        // ✅ Get All Rooms
        [HttpGet("MeetingRoomList")]
        public async Task<IActionResult> GetMeetingRooms()
        {
            var rooms = await _meetingRoomRepository.GetAllAsync();
            return Ok(rooms);
        }

        // ✅ Admin Creates Meeting Room (Role = "Admin")
        [HttpPost("Create-MeetingRoom")]
        public async Task<IActionResult> CreateMeetingRoom(CreateMeetingRoomCommand cmd)
        {
            var room = new MeetingRoom
            {
                Name = cmd.Name,
                Role = "Admin",     // ✅ NEW
                Capacity = cmd.Capacity,
                Location = cmd.Location
            };

            await _meetingRoomRepository.AddAsync(room);
            return Ok("Meeting Room Created by Admin");
        }

        // ✅ Get Single Room by Serial
        [HttpGet("MeetingRoom/{serial}")]
        public async Task<IActionResult> GetMeetingRoomBySerial(int serial)
        {
            var room = await _meetingRoomRepository.GetBySerialAsync(serial);
            if (room == null) return NotFound();
            return Ok(room);
        }

        // ✅ Delete Room by Serial
        [HttpDelete("MeetingRoom/{serial}")]
        public async Task<IActionResult> DeleteMeetingRoom(int serial)
        {
            await _meetingRoomRepository.DeleteBySerialAsync(serial);
            return Ok("Meeting Room Deleted");
        }
    }
}
