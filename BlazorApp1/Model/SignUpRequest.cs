namespace BlazorApp1.Client.Models
{
    public class SignUpRequest
    {
        public string FullName { get; set; } = string.Empty;  // Full-Name
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;  // Username field
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
