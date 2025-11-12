using Application.DTOs;
using Application.Commands;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IMeetingRoomRepository _meetingRoomRepository;
        private readonly IUserRepository _userRepository;

        public EmployeeController(IMeetingRoomRepository meetingRoomRepository, IUserRepository userRepository)
        {
            _meetingRoomRepository = meetingRoomRepository;
            _userRepository = userRepository;
        }

        // ✅ Employee creates meeting room (Role = "Employee")
        [HttpPost("Create-Meeting")]
        public async Task<IActionResult> CreateMeeting(MeetingRoomDto dto)
        {
            var room = new MeetingRoom
            {
                Name = dto.Name,
                Role = "Employee",   // ✅ NEW
                Capacity = dto.Capacity,
                Location = dto.Location
            };

            await _meetingRoomRepository.AddAsync(room);
            return Ok("Meeting Room Created by Employee");
        }

        // ✅ Update Profile
        [HttpPut("Update-Profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileCommand cmd)
        {
            var user = await _userRepository.GetByIdAsync(cmd.UserId);
            if (user == null) return NotFound();

            user.Username = cmd.Username;
            user.PasswordHash = cmd.Password; // Hashing should be handled by service

            await _userRepository.UpdateAsync(user);
            return Ok("Profile Updated");
        }

        // ✅ Delete Profile
        [HttpDelete("Delete-Profile")]
        public async Task<IActionResult> DeleteProfile(DeleteProfileCommand cmd)
        {
            await _userRepository.DeleteAsync(cmd.UserId);
            return Ok("Profile Deleted");
        }

        // ✅ Get All Meeting Rooms
        [HttpGet("MeetingRoomList")]
        public async Task<IActionResult> GetMeetingRooms()
        {
            var rooms = await _meetingRoomRepository.GetAllAsync();
            return Ok(rooms);
        }
    }
}
