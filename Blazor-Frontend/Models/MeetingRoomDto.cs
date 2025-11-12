using System;

namespace MeetingRoomNano.Client.Models
{
    public class MeetingRoomDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
    }
}
