namespace Application.DTOs
{
    public class RegisterRequest
    {
        public int Serial { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; // hashed or masked
        public string Role { get; set; } = "Employee";
    }
}
