using MeetingRoomNano.Client.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace MeetingRoomNano.Client.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;

        public ApiService(HttpClient http)
        {
            _http = http;
        }

        // ================== Employee Profile ==================
        public async Task<ProfileUpdateDto> GetProfile()
        {
            return await _http.GetFromJsonAsync<ProfileUpdateDto>("api/employee/profile");
        }

        public async Task<bool> UpdateProfile(ProfileUpdateDto profile)
        {
            var response = await _http.PutAsJsonAsync("api/employee/profile", profile);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProfile()
        {
            var response = await _http.DeleteAsync("api/employee/profile");
            return response.IsSuccessStatusCode;
        }

        // ================== Meeting Rooms ==================
        public async Task<List<MeetingRoomDto>> GetMeetingRooms()
        {
            return await _http.GetFromJsonAsync<List<MeetingRoomDto>>("api/meetingrooms");
        }

        public async Task<MeetingRoomDto> GetMeetingRoomById(Guid id)
        {
            return await _http.GetFromJsonAsync<MeetingRoomDto>($"api/meetingrooms/{id}");
        }

        public async Task<bool> DeleteMeetingRoom(Guid id)
        {
            var response = await _http.DeleteAsync($"api/meetingrooms/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CreateMeetingRoom(MeetingRoomCreateDto room)
        {
            var response = await _http.PostAsJsonAsync("api/meetingrooms", room);
            return response.IsSuccessStatusCode;
        }

        // ================== Meetings ==================
        public async Task<bool> CreateMeeting(MeetingCreateDto meeting)
        {
            var response = await _http.PostAsJsonAsync("api/meetings", meeting);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<MeetingCreateDto>> GetMeetings()
        {
            return await _http.GetFromJsonAsync<List<MeetingCreateDto>>("api/meetings");
        }
    }
}
