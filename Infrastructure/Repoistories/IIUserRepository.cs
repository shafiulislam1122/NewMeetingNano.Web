using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IIUserRepository
    {
        Task<int> AddAsync(User user);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByIdAsync(int id);
        Task<int> UpdateAsync(User user);
        Task<int> DeleteAsync(int id);
        Task<List<User>> GetAllAsync();

        // ✅ NEW: Delete by Name + Email
        Task<int> DeleteByNameEmailAsync(string name, string email);

        // Optional: Get user by Name + Email
        Task<User> GetByNameEmailAsync(string name, string email);
    }
}
