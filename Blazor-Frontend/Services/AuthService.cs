using System.Net.Http.Json;
using MeetingRoomNano.Client.Models;

namespace MeetingRoomNano.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly JwtAuthStateProvider _authStateProvider;
        private readonly LocalStorageService _localStorage;

        public AuthService(HttpClient http, JwtAuthStateProvider authStateProvider, LocalStorageService localStorage)
        {
            _http = http;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/Account/Login", request);
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/Account/Register", request);
            return await response.Content.ReadFromJsonAsync<AuthResponse>();
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            _authStateProvider.NotifyUserLogout();
        }
    }
}
