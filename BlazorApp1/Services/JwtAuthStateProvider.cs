using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorApp1.Services
{
    public class JwtAuthStateProvider : AuthenticationStateProvider
    {
        private readonly TokenProvider _tokenProvider;

        public JwtAuthStateProvider(TokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _tokenProvider.GetTokenAsync();
            ClaimsIdentity identity;

            if (!string.IsNullOrEmpty(token))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, "User"),
                    new Claim(ClaimTypes.Role, "Employee")
                };
                identity = new ClaimsIdentity(claims, "jwt");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        // ✅ Proper GetUserAsync for frontend
        public async Task<UserDto> GetUserAsync()
        {
            var token = await _tokenProvider.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return new UserDto { Username = "", Role = "" };

            // JWT parse kore real username/role korte paro
            return new UserDto { Username = "User", Role = "Employee" };
        }

        public async Task Logout()
        {
            await _tokenProvider.RemoveTokenAsync();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }

    public class UserDto
    {
        public string Username { get; set; } = "";
        public string Role { get; set; } = "";
    }
}
