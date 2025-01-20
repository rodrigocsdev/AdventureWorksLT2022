using AdventureWorksLT2022.Models;
using AdventureWorksLT2022.Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AdventureWorksLT2022.Repositories
{
    public class ProductRepository
    {
        private readonly Connection _connection;

        public ProductRepository(Connection connection)
        {
            _connection = connection;
        }

        private async Task EnsureConnectionOpenAsync(IDbConnection dbConnection)
        {
            if (dbConnection.State != ConnectionState.Open)
            {
                if (dbConnection is SqlConnection sqlConnection)
                {
                    await sqlConnection.OpenAsync();
                }
                else
                {
                    dbConnection.Open();
                }
            }
        }

        private async Task<bool> ReadAsync(IDataReader reader)
        {
            if (reader is SqlDataReader sqlReader)
            {
                return await sqlReader.ReadAsync();
            }
            else
            {
                return reader.Read(); // Síncrono como fallback
            }
        }

        public async Task<IEnumerable<IProduct>> GetAllProductsAsync()
        {
            var products = new List<IProduct>();

            using (var dbConnection = _connection.CreateConnection())
            {
                await EnsureConnectionOpenAsync(dbConnection);
                using (var command = dbConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [SalesLT].[Product]";
                    using (var reader = command.ExecuteReader())
                    {
                        while (await ReadAsync(reader))
                        {
                            products.Add(new Product
                            {
                                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ProductNumber = reader.GetString(reader.GetOrdinal("ProductNumber")),
                                Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? null : reader.GetString(reader.GetOrdinal("Color")),
                                StandardCost = reader.GetDecimal(reader.GetOrdinal("StandardCost")),
                                ListPrice = reader.GetDecimal(reader.GetOrdinal("ListPrice")),
                                Size = reader.IsDBNull(reader.GetOrdinal("Size")) ? null : reader.GetString(reader.GetOrdinal("Size")),
                                Weight = reader.IsDBNull(reader.GetOrdinal("Weight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Weight")),
                                ProductCategoryID = reader.IsDBNull(reader.GetOrdinal("ProductCategoryID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ProductCategoryID")),
                                ProductModelID = reader.IsDBNull(reader.GetOrdinal("ProductModelID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ProductModelID")),
                                SellStartDate = reader.GetDateTime(reader.GetOrdinal("SellStartDate")),
                                SellEndDate = reader.IsDBNull(reader.GetOrdinal("SellEndDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("SellEndDate")),
                                DiscontinuedDate = reader.IsDBNull(reader.GetOrdinal("DiscontinuedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DiscontinuedDate")),
                                RowGuid = reader.GetGuid(reader.GetOrdinal("RowGuid")),
                                ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                            });
                        }
                    }
                }
            }

            return products;
        }

        public async Task<IProduct?> GetProductByIdAsync(int productId)
        {
            using (var dbConnection = _connection.CreateConnection())
            {
                await EnsureConnectionOpenAsync(dbConnection);
                using (var command = dbConnection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM [SalesLT].[Product] WHERE ProductID = @ProductID";
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@ProductID";
                    parameter.Value = productId;
                    command.Parameters.Add(parameter);

                    using (var reader = command.ExecuteReader())
                    {
                        if (await ReadAsync(reader))
                        {
                            return new Product
                            {
                                ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ProductNumber = reader.GetString(reader.GetOrdinal("ProductNumber")),
                                Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? null : reader.GetString(reader.GetOrdinal("Color")),
                                StandardCost = reader.GetDecimal(reader.GetOrdinal("StandardCost")),
                                ListPrice = reader.GetDecimal(reader.GetOrdinal("ListPrice")),
                                Size = reader.IsDBNull(reader.GetOrdinal("Size")) ? null : reader.GetString(reader.GetOrdinal("Size")),
                                Weight = reader.IsDBNull(reader.GetOrdinal("Weight")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Weight")),
                                ProductCategoryID = reader.IsDBNull(reader.GetOrdinal("ProductCategoryID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ProductCategoryID")),
                                ProductModelID = reader.IsDBNull(reader.GetOrdinal("ProductModelID")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ProductModelID")),
                                SellStartDate = reader.GetDateTime(reader.GetOrdinal("SellStartDate")),
                                SellEndDate = reader.IsDBNull(reader.GetOrdinal("SellEndDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("SellEndDate")),
                                DiscontinuedDate = reader.IsDBNull(reader.GetOrdinal("DiscontinuedDate")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DiscontinuedDate")),
                                RowGuid = reader.GetGuid(reader.GetOrdinal("RowGuid")),
                                ModifiedDate = reader.GetDateTime(reader.GetOrdinal("ModifiedDate"))
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task AddProductAsync(IProduct product)
        {
            using (var dbConnection = _connection.CreateConnection())
            {
                await EnsureConnectionOpenAsync(dbConnection);
                using (var command = dbConnection.CreateCommand())
                {
                    command.CommandText = @"
                INSERT INTO [SalesLT].[Product] 
                (Name, ProductNumber, Color, StandardCost, ListPrice, Size, Weight, ProductCategoryID, ProductModelID, SellStartDate, SellEndDate, DiscontinuedDate, RowGuid, ModifiedDate) 
                VALUES 
                (@Name, @ProductNumber, @Color, @StandardCost, @ListPrice, @Size, @Weight, @ProductCategoryID, @ProductModelID, @SellStartDate, @SellEndDate, @DiscontinuedDate,
                @RowGuid, @ModifiedDate)";
                    var nameParameter = command.CreateParameter();
                    nameParameter.ParameterName = "@Name";
                    nameParameter.Value = product.Name;
                    command.Parameters.Add(nameParameter);

                    var idParameter = command.CreateParameter();
                    idParameter.ParameterName = "@ProductID";
                    idParameter.Value = product.ProductID;
                    command.Parameters.Add(idParameter);

                    var productNumberParameter = command.CreateParameter();
                    productNumberParameter.ParameterName = "@ProductNumber";
                    productNumberParameter.Value = product.ProductNumber;
                    command.Parameters.Add(productNumberParameter);

                    var colorParameter = command.CreateParameter();
                    colorParameter.ParameterName = "@Color";
                    colorParameter.Value = product.Color ?? (object)DBNull.Value;
                    command.Parameters.Add(colorParameter);

                    var standardCostParameter = command.CreateParameter();
                    standardCostParameter.ParameterName = "@StandardCost";
                    standardCostParameter.Value = product.StandardCost;
                    command.Parameters.Add(standardCostParameter);

                    var listPriceParameter = command.CreateParameter();
                    listPriceParameter.ParameterName = "@ListPrice";
                    listPriceParameter.Value = product.ListPrice;
                    command.Parameters.Add(listPriceParameter);

                    var sizeParameter = command.CreateParameter();
                    sizeParameter.ParameterName = "@Size";
                    sizeParameter.Value = product.Size ?? (object)DBNull.Value;
                    command.Parameters.Add(sizeParameter);

                    var weightParameter = command.CreateParameter();
                    weightParameter.ParameterName = "@Weight";
                    weightParameter.Value = product.Weight ?? (object)DBNull.Value;
                    command.Parameters.Add(weightParameter);

                    var productCategoryIDParameter = command.CreateParameter();
                    productCategoryIDParameter.ParameterName = "@ProductCategoryID";
                    productCategoryIDParameter.Value = product.ProductCategoryID ?? (object)DBNull.Value;
                    command.Parameters.Add(productCategoryIDParameter);

                    var productModelIDParameter = command.CreateParameter();
                    productModelIDParameter.ParameterName = "@ProductModelID";
                    productModelIDParameter.Value = product.ProductModelID ?? (object)DBNull.Value;
                    command.Parameters.Add(productModelIDParameter);

                    var sellStartDateParameter = command.CreateParameter();
                    sellStartDateParameter.ParameterName = "@SellStartDate";
                    sellStartDateParameter.Value = product.SellStartDate;
                    command.Parameters.Add(sellStartDateParameter);

                    var sellEndDateParameter = command.CreateParameter();
                    sellEndDateParameter.ParameterName = "@SellEndDate";
                    sellEndDateParameter.Value = product.SellEndDate ?? (object)DBNull.Value;
                    command.Parameters.Add(sellEndDateParameter);

                    var discontinuedDateParameter = command.CreateParameter();
                    discontinuedDateParameter.ParameterName = "@DiscontinuedDate";
                    discontinuedDateParameter.Value = product.DiscontinuedDate ?? (object)DBNull.Value;
                    command.Parameters.Add(discontinuedDateParameter);
                    
                    var rowGuidParameter = command.CreateParameter();
                    rowGuidParameter.ParameterName = "@RowGuid";
                    rowGuidParameter.Value = product.RowGuid;
                    command.Parameters.Add(rowGuidParameter);

                    var modifiedDateParameter = command.CreateParameter();
                    modifiedDateParameter.ParameterName = "@ModifiedDate";
                    modifiedDateParameter.Value = product.ModifiedDate;
                    command.Parameters.Add(modifiedDateParameter);                    

                    if (command is SqlCommand sqlCommand)
                    {
                        await sqlCommand.ExecuteNonQueryAsync();
                    }
                    else
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public async Task UpdateProductAsync(IProduct product)
        {
            using (var dbConnection = _connection.CreateConnection())
            {
                await EnsureConnectionOpenAsync(dbConnection);
                using (var command = dbConnection.CreateCommand())
                {
                    command.CommandText = @"
                UPDATE [SalesLT].[Product] 
                SET Name = @Name, ProductNumber = @ProductNumber, Color = @Color, StandardCost = @StandardCost, ListPrice = @ListPrice, Size = @Size, Weight = @Weight, 
                    ProductCategoryID = @ProductCategoryID, ProductModelID = @ProductModelID, SellStartDate = @SellStartDate, SellEndDate = @SellEndDate, 
                    DiscontinuedDate = @DiscontinuedDate, RowGuid = @RowGuid, 
                    ModifiedDate = @ModifiedDate  WHERE ProductID = @ProductID";
                    var nameParameter = command.CreateParameter();
                    nameParameter.ParameterName = "@Name";
                    nameParameter.Value = product.Name;
                    command.Parameters.Add(nameParameter);

                    var idParameter = command.CreateParameter();
                    idParameter.ParameterName = "@ProductID";
                    idParameter.Value = product.ProductID;
                    command.Parameters.Add(idParameter);

                    var productNumberParameter = command.CreateParameter();
                    productNumberParameter.ParameterName = "@ProductNumber";
                    productNumberParameter.Value = product.ProductNumber;
                    command.Parameters.Add(productNumberParameter);

                    var colorParameter = command.CreateParameter();
                    colorParameter.ParameterName = "@Color";
                    colorParameter.Value = product.Color ?? (object)DBNull.Value;
                    command.Parameters.Add(colorParameter);

                    var standardCostParameter = command.CreateParameter();
                    standardCostParameter.ParameterName = "@StandardCost";
                    standardCostParameter.Value = product.StandardCost;
                    command.Parameters.Add(standardCostParameter);

                    var listPriceParameter = command.CreateParameter();
                    listPriceParameter.ParameterName = "@ListPrice";
                    listPriceParameter.Value = product.ListPrice;
                    command.Parameters.Add(listPriceParameter);

                    var sizeParameter = command.CreateParameter();
                    sizeParameter.ParameterName = "@Size";
                    sizeParameter.Value = product.Size ?? (object)DBNull.Value;
                    command.Parameters.Add(sizeParameter);

                    var weightParameter = command.CreateParameter();
                    weightParameter.ParameterName = "@Weight";
                    weightParameter.Value = product.Weight ?? (object)DBNull.Value;
                    command.Parameters.Add(weightParameter);

                    var productCategoryIDParameter = command.CreateParameter();
                    productCategoryIDParameter.ParameterName = "@ProductCategoryID";
                    productCategoryIDParameter.Value = product.ProductCategoryID ?? (object)DBNull.Value;
                    command.Parameters.Add(productCategoryIDParameter);

                    var productModelIDParameter = command.CreateParameter();
                    productModelIDParameter.ParameterName = "@ProductModelID";
                    productModelIDParameter.Value = product.ProductModelID ?? (object)DBNull.Value;
                    command.Parameters.Add(productModelIDParameter);

                    var sellStartDateParameter = command.CreateParameter();
                    sellStartDateParameter.ParameterName = "@SellStartDate";
                    sellStartDateParameter.Value = product.SellStartDate;
                    command.Parameters.Add(sellStartDateParameter);

                    var sellEndDateParameter = command.CreateParameter();
                    sellEndDateParameter.ParameterName = "@SellEndDate";
                    sellEndDateParameter.Value = product.SellEndDate ?? (object)DBNull.Value;
                    command.Parameters.Add(sellEndDateParameter);

                    var discontinuedDateParameter = command.CreateParameter();
                    discontinuedDateParameter.ParameterName = "@DiscontinuedDate";
                    discontinuedDateParameter.Value = product.DiscontinuedDate ?? (object)DBNull.Value;
                    command.Parameters.Add(discontinuedDateParameter);                                    

                    var rowGuidParameter = command.CreateParameter();
                    rowGuidParameter.ParameterName = "@RowGuid";
                    rowGuidParameter.Value = product.RowGuid;
                    command.Parameters.Add(rowGuidParameter);

                    var modifiedDateParameter = command.CreateParameter();
                    modifiedDateParameter.ParameterName = "@ModifiedDate";
                    modifiedDateParameter.Value = product.ModifiedDate;
                    command.Parameters.Add(modifiedDateParameter);


                    if (command is SqlCommand sqlCommand)
                    {
                        await sqlCommand.ExecuteNonQueryAsync();
                    }
                    else
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public async Task DeleteProductAsync(int productId)
        {
            using (var dbConnection = _connection.CreateConnection())
            {
                await EnsureConnectionOpenAsync(dbConnection);
                using (var command = dbConnection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM [SalesLT].[Product] WHERE ProductID = @ProductID";
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@ProductID";
                    parameter.Value = productId;
                    command.Parameters.Add(parameter);

                    if (command is SqlCommand sqlCommand)
                    {
                        await sqlCommand.ExecuteNonQueryAsync();
                    }
                    else
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
