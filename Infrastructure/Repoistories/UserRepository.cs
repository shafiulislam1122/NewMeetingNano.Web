// UserRepository.cs (Infrastructure/Repositories)

using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    // ✅ FIX: Inherits from IIUserRepository
    public class UserRepository : IIUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        // Add new user, return affected rows
        public async Task<int> AddAsync(User user)
        {
            var query = @"
                INSERT INTO Users (FullName, Email, Username, PasswordHash, Role)
                VALUES (@FullName, @Email, @Username, @PasswordHash, @Role)";

            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(query, new
            {
                user.FullName,
                user.Email,
                user.Username,
                user.PasswordHash,
                user.Role
            });
        }

        // Get user by username
        public async Task<User> GetByUsernameAsync(string username)
        {
            var query = "SELECT * FROM Users WHERE Username = @Username";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>(query, new { Username = username });
        }

        // Get user by Id
        public async Task<User> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Users WHERE [Id] = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>(query, new { Id = id });
        }

        // Update user, return affected rows
        public async Task<int> UpdateAsync(User user)
        {
            var query = @"
                UPDATE Users
                SET FullName = @FullName,
                    Email = @Email,
                    Username = @Username,
                    PasswordHash = @PasswordHash,
                    Role = @Role
                WHERE [Id] = @Id";

            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(query, new
            {
                user.FullName,
                user.Email,
                user.Username,
                user.PasswordHash,
                user.Role,
                user.Id
            });
        }

        // Delete user by Id, return affected rows
        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM Users WHERE [Id] = @Id";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(query, new { Id = id });
        }

        // Get all users
        public async Task<List<User>> GetAllAsync()
        {
            var query = "SELECT * FROM Users";
            using var connection = _context.CreateConnection();
            var users = await connection.QueryAsync<User>(query);
            return users.AsList();
        }

        // ✅ NEW: Get user by name and email
        public async Task<User> GetByNameEmailAsync(string name, string email)
        {
            var query = "SELECT * FROM Users WHERE Username = @Name AND Email = @Email";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>(query, new { Name = name, Email = email });
        }

        // ✅ NEW: Delete user using Name + Email (NO Id needed)
        public async Task<int> DeleteByNameEmailAsync(string name, string email)
        {
            var query = @"
                DELETE FROM Users
                WHERE Username = @Name AND Email = @Email";

            using var connection = _context.CreateConnection();
            return await connection.ExecuteAsync(query, new { Name = name, Email = email });
        }
    }
}
