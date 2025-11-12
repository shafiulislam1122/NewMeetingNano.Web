
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorApp1.Client.Models; // MeetingRoom মডেল
using BlazorApp1.Client.DTOs;   // CreateMeetingRoomCommand DTO
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp1.Services
{
    public class MeetingRoomService
    {
        private readonly HttpClient _httpClient;
        private readonly TokenProvider _tokenProvider;

        public MeetingRoomService(HttpClient httpClient, TokenProvider tokenProvider)
        {
            _httpClient = httpClient;
            _tokenProvider = tokenProvider;
        }

        // ============================
        // JWT attach helper
        // ============================
        private async Task AddJwtHeaderAsync()
        {
            var token = await _tokenProvider.GetTokenAsync();
            if (!string.IsNullOrWhiteSpace(token))
            {
                if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                    _httpClient.DefaultRequestHeaders.Remove("Authorization");

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        // ============================
        // Admin Create Room
        // POST /api/Admin/Create-MeetingRoom
        // ============================
        public async Task CreateRoomAsync(MeetingRoom newRoom)
        {
            await AddJwtHeaderAsync();

            var cmd = new CreateMeetingRoomCommand
            {
                Name = newRoom.Name,
                Capacity = newRoom.Capacity,
                Location = newRoom.Location
            };

            var response = await _httpClient.PostAsJsonAsync("/api/Admin/Create-MeetingRoom", cmd);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Admin: Failed to create room. Status code: {response.StatusCode}");
        }

        // ============================
        // Employee Create Room
        // POST /api/Employee/Create-Meeting
        // ============================
        public async Task CreateRoomEmployeeAsync(MeetingRoom newRoom)
        {
            await AddJwtHeaderAsync();

            var response = await _httpClient.PostAsJsonAsync("/api/Employee/Create-Meeting", newRoom);

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Employee: Failed to create room. Status code: {response.StatusCode}");
        }

        // ============================
        // Get All Rooms (Admin + Employee)
        // GET /api/MeetingRoom/MeetingRoomList
        // ============================
        public async Task<List<MeetingRoom>> GetAllRoomsAsync()
        {
            await AddJwtHeaderAsync();

            try
            {
                return await _httpClient.GetFromJsonAsync<List<MeetingRoom>>("/api/MeetingRoom/MeetingRoomList");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized || ex.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new UnauthorizedAccessException("Authentication failed or access denied for room list.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to fetch room list. Error: {ex.Message}");
            }
        }

        // ============================
        // Get Room Details
        // GET /api/MeetingRoom/MeetingRoom/{serial}
        // ============================
        public async Task<MeetingRoom?> GetRoomDetailsAsync(int serial)
        {
            await AddJwtHeaderAsync();

            try
            {
                return await _httpClient.GetFromJsonAsync<MeetingRoom>($"/api/MeetingRoom/MeetingRoom/{serial}");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to fetch room details for serial {serial}. Error: {ex.Message}");
            }
        }

        // ============================
        // Delete Room
        // DELETE /api/MeetingRoom/MeetingRoom/{serial}
        // ============================
        public async Task DeleteRoomAsync(int serial)
        {
            await AddJwtHeaderAsync();

            var response = await _httpClient.DeleteAsync($"/api/MeetingRoom/MeetingRoom/{serial}");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Failed to delete room with serial {serial}. Status code: {response.StatusCode}");
        }
    }
}
