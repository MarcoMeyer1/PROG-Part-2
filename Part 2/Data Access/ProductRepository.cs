using Dapper;
using Part_2.Data_Access;
using Part_2.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Part_2.Data
{
    public class ProductRepository
    {
        private readonly DatabaseContext _context;

        public ProductRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<Product>("SELECT * FROM Products");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the products.", ex);
            }
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryFirstOrDefaultAsync<Product>("SELECT * FROM Products WHERE Id = @Id", new { Id = id });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the product with Id {id}.", ex);
            }
        }

        public async Task AddAsync(Product product)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "INSERT INTO Products (Name, Category, ProductionDate, FarmerId, Price) VALUES (@Name, @Category, @ProductionDate, @FarmerId, @Price)";
                    await connection.ExecuteAsync(sql, product);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the product.", ex);
            }
        }

        public async Task UpdateAsync(Product product)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "UPDATE Products SET Name = @Name, Category = @Category, ProductionDate = @ProductionDate, Price = @Price WHERE Id = @Id";
                    await connection.ExecuteAsync(sql, product);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the product with Id {product.Id}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "DELETE FROM Products WHERE Id = @Id";
                    await connection.ExecuteAsync(sql, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the product with Id {id}.", ex);
            }
        }

        public async Task<IEnumerable<Product>> GetByFarmerIdAsync(int farmerId)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<Product>("SELECT * FROM Products WHERE FarmerId = @FarmerId", new { FarmerId = farmerId });
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving products for farmer with Id {farmerId}.", ex);
            }
        }

        public async Task<IEnumerable<string>> GetDistinctCategoriesAsync()
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    return await connection.QueryAsync<string>("SELECT DISTINCT Category FROM Products");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the categories.", ex);
            }
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string query)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "SELECT * FROM Products WHERE Name LIKE @Query OR Category LIKE @Query";
                    return await connection.QueryAsync<Product>(sql, new { Query = "%" + query + "%" });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while searching for products.", ex);
            }
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllWithFarmerAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
                    SELECT p.Id, p.Name, p.Category, p.ProductionDate, p.Price, p.FarmerId, u.Name AS FarmerName
                    FROM Products p
                    INNER JOIN Users u ON p.FarmerId = u.Id
                    WHERE u.Role = 'Farmer'";
                return await connection.QueryAsync<ProductViewModel>(sql);
            }
        }

        public async Task<ProductViewModel> GetByIdWithFarmerAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = @"
                    SELECT p.Id, p.Name, p.Category, p.ProductionDate, p.Price, p.FarmerId, u.Name AS FarmerName
                    FROM Products p
                    INNER JOIN Users u ON p.FarmerId = u.Id
                    WHERE p.Id = @Id AND u.Role = 'Farmer'";
                return await connection.QueryFirstOrDefaultAsync<ProductViewModel>(sql, new { Id = id });
            }
        }

        public async Task<IEnumerable<Product>> GetFilteredProductsAsync(DateTime? startDate, DateTime? endDate, string category)
        {
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var sql = "SELECT * FROM Products WHERE 1=1";

                    if (startDate.HasValue)
                    {
                        sql += " AND ProductionDate >= @StartDate";
                    }

                    if (endDate.HasValue)
                    {
                        sql += " AND ProductionDate <= @EndDate";
                    }

                    if (!string.IsNullOrEmpty(category))
                    {
                        sql += " AND Category = @Category";
                    }

                    return await connection.QueryAsync<Product>(sql, new { StartDate = startDate, EndDate = endDate, Category = category });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the filtered products.", ex);
            }
        }
    }
}
    