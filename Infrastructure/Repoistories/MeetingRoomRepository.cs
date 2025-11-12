using Domain.Entities;
using Domain.Interfaces;
using Dapper;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MeetingRoomRepository : IMeetingRoomRepository
    {
        private readonly DapperContext _context;

        public MeetingRoomRepository(DapperContext context)
        {
            _context = context;
        }

        // ✅ GET SINGLE ROOM BY SERIAL
        public async Task<MeetingRoom?> GetBySerialAsync(int serial)
        {
            var sql = "SELECT Serial, Name, Role, Capacity, Location, CreatedAt FROM MeetingRooms WHERE Serial = @Serial";
            using var conn = _context.CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<MeetingRoom>(sql, new { Serial = serial });
        }

        // ✅ GET ALL ROOMS
        public async Task<List<MeetingRoom>> GetAllAsync()
        {
            var sql = "SELECT Serial, Name, Role, Capacity, Location, CreatedAt FROM MeetingRooms ORDER BY Serial ASC";
            using var conn = _context.CreateConnection();
            var rooms = await conn.QueryAsync<MeetingRoom>(sql);
            return rooms.AsList();
        }

        // ✅ ADD NEW ROOM
        public async Task<int> AddAsync(MeetingRoom room)
        {
            var sql = @"
                INSERT INTO MeetingRooms (Name, Role, Capacity, Location)
                VALUES (@Name, @Role, @Capacity, @Location)";
            using var conn = _context.CreateConnection();
            return await conn.ExecuteAsync(sql, room);
        }

        // ✅ UPDATE ROOM
        public async Task<int> UpdateAsync(MeetingRoom room)
        {
            var sql = @"
                UPDATE MeetingRooms
                SET Name = @Name,
                    Role = @Role,
                    Capacity = @Capacity,
                    Location = @Location
                WHERE Serial = @Serial";
            using var conn = _context.CreateConnection();
            return await conn.ExecuteAsync(sql, room);
        }

        // ✅ DELETE ROOM BY SERIAL
        public async Task<int> DeleteBySerialAsync(int serial)
        {
            var sql = "DELETE FROM MeetingRooms WHERE Serial = @Serial";
            using var conn = _context.CreateConnection();
            return await conn.ExecuteAsync(sql, new { Serial = serial });
        }

        // Optional: legacy compatibility
        public Task<MeetingRoom?> GetByIdAsync(int serial) => GetBySerialAsync(serial);
        public Task<int> DeleteAsync(int serial) => DeleteBySerialAsync(serial);
    }
}
