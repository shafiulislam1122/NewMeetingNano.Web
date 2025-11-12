using BlazorApp1.Client.Models;
using System.Net.Http.Json;

namespace BlazorApp1.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly TokenProvider _tokenProvider;

        public AuthService(HttpClient http, TokenProvider tokenProvider)
        {
            _http = http;
            _tokenProvider = tokenProvider;
        }

        // Login method
        public async Task<AuthResponse> LoginAsync(LoginRequest loginModel)
        {
            var response = await _http.PostAsJsonAsync("api/Account/Login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    await _tokenProvider.SetTokenAsync(result.Token);
                    result.IsAuthenticated = true;
                    return result;
                }
            }

            return new AuthResponse { IsAuthenticated = false };
        }

        // SignUp method – backend RegisterAsync returns string now
        public async Task<string> SignUpAsync(SignUpRequest signUpModel)
        {
            var response = await _http.PostAsJsonAsync("api/Account/Register", signUpModel);

            if (response.IsSuccessStatusCode)
            {
                var message = await response.Content.ReadAsStringAsync();
                return message;
            }

            return "Sign-Up Failed!";
        }
    }
}
