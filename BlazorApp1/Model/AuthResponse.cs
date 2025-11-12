namespace BlazorApp1.Client.Models
{
    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;   // JWT token
        public string Username { get; set; } = string.Empty; // Logged-in user
        public string Role { get; set; } = string.Empty;    // User role
        public bool IsAuthenticated { get; set; }           // Login/SignUp status
    }
}
