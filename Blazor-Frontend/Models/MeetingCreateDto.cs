using System;

namespace MeetingRoomNano.Client.Models
{
    public class MeetingCreateDto
    {
        public string Title { get; set; }      // Meeting title
        public Guid RoomId { get; set; }       // Which room to book
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
    }
}
