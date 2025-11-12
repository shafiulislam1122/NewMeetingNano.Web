using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IMeetingRoomRepository
    {
        Task<MeetingRoom?> GetByIdAsync(int serial);              // Optional
        Task<MeetingRoom?> GetBySerialAsync(int serial);          // ✅ Must for AdminController
        Task<List<MeetingRoom>> GetAllAsync();
        Task<int> AddAsync(MeetingRoom room);
        Task<int> UpdateAsync(MeetingRoom room);
        Task<int> DeleteAsync(int serial);                        // Optional
        Task<int> DeleteBySerialAsync(int serial);               // ✅ Must for AdminController
    }
}
