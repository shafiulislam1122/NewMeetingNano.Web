namespace BlazorApp1.Client.DTOs
{
    public class CreateMeetingRoomCommand
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
    }
}
