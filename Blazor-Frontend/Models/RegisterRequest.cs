namespace MeetingRoomNano.Client.Models
{
    public class RegisterRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; } // unnecessary, remove
        public string Password { get; set; }
        public string Role { get; set; } // "Admin" or "Employee"
    }
}

