using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
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
            var query = "SELECT * FROM Users WHERE Id = @Id";
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
                WHERE Id = @Id";

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
            var query = "DELETE FROM Users WHERE Id = @Id";
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

        // Get user by name and password (for login)
        public async Task<User> GetByNamePasswordAsync(string name, string password)
        {
            var query = "SELECT * FROM Users WHERE FullName = @Name AND PasswordHash = @Password";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>(query, new { Name = name, Password = password });
        }
    }
}
