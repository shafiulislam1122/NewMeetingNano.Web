namespace Application.DTOs
{
    public class MeetingRoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Location { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // রেকর্ড ক্রিয়েট হলে স্বয়ংক্রিয় সময়
    }
}
