using Application.DTOs;
using Application.Commands;
using Domain.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims; // ClaimTypes ব্যবহারের জন্য এটি প্রয়োজন

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

        // ✨ UPDATED: শুধু Username এবং Password আপডেট করা হবে, ID টোকেন থেকে নেওয়া হবে।
        [HttpPut("Update-Profile")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateProfile(UpdateProfileCommand cmd)
        {
            // ১. JWT টোকেন থেকে ইউজারের ID নেওয়া
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                // টোকেনে ID না পেলে
                return Unauthorized("User ID not found in authentication token.");
            }

            // ২. UserRepository তে নতুন মেথডটি ব্যবহার করে শুধু Username ও Password আপডেট করা
            // ধরে নেওয়া হয়েছে পাসওয়ার্ডটি ক্লায়েন্ট সাইড থেকে আসার সময় হ্যাস (Hash) করা হয়েছে অথবা
            // সার্ভার সাইডে পাসওয়ার্ড হ্যাস করার লজিক এখানে যুক্ত করা হবে।
            var affectedRows = await _userRepository.UpdateUsernameAndPasswordAsync(
                userId,
                cmd.Username,
                cmd.Password // ⚠️ এখানে পাসওয়ার্ড হ্যাস করার লজিক দরকার হতে পারে
            );

            if (affectedRows == 0)
            {
                return NotFound("Profile update failed or user not found.");
            }

            return Ok("Profile Updated (Username and Password only)");
        }

        [HttpDelete("Delete-Profile")]
        public async Task<IActionResult> DeleteProfile(DeleteProfileCommand cmd)
        {
            // ⚠️ নোট: এখানেও ID ক্লায়েন্ট থেকে আসছে। যদি টোকেন থেকে নিতে চান, তাহলে পরিবর্তন প্রয়োজন।
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