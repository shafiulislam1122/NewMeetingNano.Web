namespace Application.Commands
{
    public class CreateMeetingRoomCommand
    {
        public string Name { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}
