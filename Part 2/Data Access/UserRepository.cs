using Dapper;
using Part_2.Data_Access;
using Part_2.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Part_2.Data
{
    public class UserRepository
    {
        private readonly DatabaseContext _context;

        public UserRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user by ID.", ex);
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Email = @Email", new { Email = email });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user by email.", ex);
            }
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "INSERT INTO Users (Name, Email, Password, Role) VALUES (@Name, @Email, @Password, @Role)";
                    await connection.ExecuteAsync(sql, user);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the user.", ex);
            }
        }

        public async Task<IEnumerable<User>> GetAllFarmersAsync()
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<User>("SELECT * FROM Users WHERE Role = 'Farmer'");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the farmers.", ex);
            }
        }

        public async Task<UserProfile> GetUserProfileByIdAsync(int userId)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
                SELECT u.*, up.Age, up.YearsOfService, up.About
                FROM Users u
                INNER JOIN UserProfiles up ON u.Id = up.UserId
                WHERE u.Id = @UserId";
                return await connection.QueryFirstOrDefaultAsync<UserProfile>(sql, new { UserId = userId });
            }
        }


        public async Task<IEnumerable<User>> GetAllEmployeesAsync()
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<User>("SELECT * FROM Users WHERE Role = 'Employee'");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all employees.", ex);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "UPDATE Users SET Name = @Name, Email = @Email, Password = @Password, Role = @Role WHERE Id = @Id";
                    await connection.ExecuteAsync(sql, user);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the user with Id {user.Id}.", ex);
            }
        }

        public async Task<UserProfile> GetUserProfileByUserIdAsync(int userId)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "SELECT * FROM UserProfiles WHERE UserId = @UserId";
                    return await connection.QueryFirstOrDefaultAsync<UserProfile>(sql, new { UserId = userId });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user profile.", ex);
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "DELETE FROM Users WHERE Id = @Id";
                    await connection.ExecuteAsync(sql, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the user with Id {id}.", ex);
            }
        }
    }
}
