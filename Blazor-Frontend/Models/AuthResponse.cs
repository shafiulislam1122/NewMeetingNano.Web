namespace MeetingRoomNano.Client.Models
{
    public class AuthResponse
    {
        public bool IsSuccess { get; set; }       // login success/fail
        public string Message { get; set; }       // error/success message
        public string Token { get; set; }         // JWT token
        public string Role { get; set; }          // Admin or Employee
    }
}
