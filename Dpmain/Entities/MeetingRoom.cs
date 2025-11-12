namespace Domain.Entities
{
    public class MeetingRoom
    {
        public int Serial { get; set; }       // Identity (Primary Key)
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string? Location { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
