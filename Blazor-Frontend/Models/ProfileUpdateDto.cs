namespace MeetingRoomNano.Client.Models
{
    public class ProfileUpdateDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }  // ✅ Add this line
    }
}
