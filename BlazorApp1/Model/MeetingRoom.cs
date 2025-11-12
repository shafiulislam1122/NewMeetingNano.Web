public class MeetingRoom
{
    public int Serial { get; set; }      // ✅ correct DB primary key
    public string Name { get; set; }
    public string Role { get; set; }     // ✅ exists in DB
    public int Capacity { get; set; }
    public string Location { get; set; }
    public DateTime CreatedAt { get; set; }  // ✅ exists in DB
}
