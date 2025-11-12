namespace BlazorApp1.Client.Models
{
    public class Meeting
    {
        public int Serial { get; set; }      // ✅ DB primary key
        public string Name { get; set; }
        public string Role { get; set; }     // ✅ Employee/Admin
        public int Capacity { get; set; }
        public string Location { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;  // ✅ default current time
    }
}
