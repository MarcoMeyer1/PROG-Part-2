using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Part_2.Models;
using System;
using Part_2.Data_Access;

namespace Part_2.Data
{
    public class EmployeeRepository
    {
        private readonly DatabaseContext _context;

        public EmployeeRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<Employee>("SELECT * FROM Employees");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the employees.", ex);
            }
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<Employee>("SELECT * FROM Employees WHERE Id = @Id", new { Id = id });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the employee with Id {id}.", ex);
            }
        }

        public async Task AddAsync(Employee employee)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "INSERT INTO Employees (Name, Email, Password) VALUES (@Name, @Email, @Password)";
                    await connection.ExecuteAsync(sql, employee);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the employee.", ex);
            }
        }

        public async Task UpdateAsync(Employee employee)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "UPDATE Employees SET Name = @Name, Email = @Email, Password = @Password WHERE Id = @Id";
                    await connection.ExecuteAsync(sql, employee);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the employee with Id {employee.Id}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "DELETE FROM Employees WHERE Id = @Id";
                    await connection.ExecuteAsync(sql, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the employee with Id {id}.", ex);
            }
        }
    }
}
