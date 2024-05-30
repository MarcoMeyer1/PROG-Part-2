using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Part_2.Models;
using System;
using Part_2.Data_Access;

namespace Part_2.Data
{
    public class FarmerRepository
    {
        private readonly DatabaseContext _context;

        public FarmerRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Farmer>> GetAllAsync()
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<Farmer>("SELECT * FROM Farmers");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the farmers.", ex);
            }
        }

        public async Task<Farmer> GetByIdAsync(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<Farmer>("SELECT * FROM Farmers WHERE Id = @Id", new { Id = id });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the farmer with Id {id}.", ex);
            }
        }

        public async Task AddAsync(Farmer farmer)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "INSERT INTO Farmers (Name, Email, Password) VALUES (@Name, @Email, @Password)";
                    await connection.ExecuteAsync(sql, farmer);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the farmer.", ex);
            }
        }

        public async Task UpdateAsync(Farmer farmer)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "UPDATE Farmers SET Name = @Name, Email = @Email, Password = @Password WHERE Id = @Id";
                    await connection.ExecuteAsync(sql, farmer);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the farmer with Id {farmer.Id}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "DELETE FROM Farmers WHERE Id = @Id";
                    await connection.ExecuteAsync(sql, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the farmer with Id {id}.", ex);
            }
        }
    }
}
