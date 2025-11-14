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
                Role = "Employee",
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

        // ✅ Delete Profile by UserId (existing)
        [HttpDelete("Delete-Profile")]
        public async Task<IActionResult> DeleteProfile(DeleteProfileCommand cmd)
        {
            await _userRepository.DeleteAsync(cmd.UserId);
            return Ok("Profile Deleted");
        }

        // ✅ New: Delete Profile by Name + Password
        [HttpDelete("Delete-Profile-ByNamePassword")]
        public async Task<IActionResult> DeleteProfileByNamePassword([FromBody] DeleteByNamePasswordRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Name and Password are required.");

            // Find user by Name + Password (replace with hashed verification in production)
            var user = await _userRepository.GetByNamePasswordAsync(request.Name, request.Password);
            if (user == null)
                return NotFound("User not found or password incorrect.");

            await _userRepository.DeleteAsync(user.Id);
            return Ok("User successfully deleted.");
        }

        // ✅ Get All Meeting Rooms
        [HttpGet("MeetingRoomList")]
        public async Task<IActionResult> GetMeetingRooms()
        {
            var rooms = await _meetingRoomRepository.GetAllAsync();
            return Ok(rooms);
        }
    }

    // ✅ New DTO for Name + Password delete
    public class DeleteByNamePasswordRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
