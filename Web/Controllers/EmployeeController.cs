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
        private readonly IIUserRepository _userRepository;

        public EmployeeController(IMeetingRoomRepository meetingRoomRepository, IIUserRepository userRepository)
        {
            _meetingRoomRepository = meetingRoomRepository;
            _userRepository = userRepository;
        }

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

        [HttpPut("Update-Profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileCommand cmd)
        {
            var user = await _userRepository.GetByIdAsync(cmd.UserId);
            if (user == null) return NotFound();

            user.Username = cmd.Username;
            user.PasswordHash = cmd.Password;
            await _userRepository.UpdateAsync(user);
            return Ok("Profile Updated");
        }

        [HttpDelete("Delete-Profile")]
        public async Task<IActionResult> DeleteProfile(DeleteProfileCommand cmd)
        {
            await _userRepository.DeleteAsync(cmd.UserId);
            return Ok("Profile Deleted");
        }
        [AllowAnonymous]
        [HttpPost("Delete-Profile-ByNameEmail")] // POST for body support
        public async Task<IActionResult> DeleteProfileByNameEmail([FromBody] DeleteEmployeeDto request)
        {
            if (request == null || string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Email))
                return BadRequest("Name and Email are required.");

            var affectedRows = await _userRepository.DeleteByNameEmailAsync(request.Name, request.Email);

            if (affectedRows == 0)
                return NotFound("No user found with the given Name and Email.");

            return Ok("User successfully deleted.");
        }

        [HttpGet("MeetingRoomList")]
        public async Task<IActionResult> GetMeetingRooms()
        {
            var rooms = await _meetingRoomRepository.GetAllAsync();
            return Ok(rooms);
        }
    }
}
