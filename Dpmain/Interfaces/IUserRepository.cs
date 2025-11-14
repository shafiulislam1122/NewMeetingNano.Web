using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<int> AddAsync(User user);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByIdAsync(int id);
        Task<int> UpdateAsync(User user);
        Task<int> DeleteAsync(int id);
        Task<List<User>> GetAllAsync();

        // ✅ Add this line
        Task<User> GetByNamePasswordAsync(string name, string password);
    }
}
