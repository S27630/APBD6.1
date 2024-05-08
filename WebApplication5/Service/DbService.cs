using System.Data;
using System.Data.SqlClient;
using WebApplication5.DTOs;

namespace WebApplication5.Service;

public interface IDbService
{
    public Task<int?> InsertSomething(CreateProductWarehouseRequest request);
}

public class DbService : IDbService
{
    private readonly IConfiguration _configuration;

    public DbService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    private async Task<SqlConnection> GetConnection()
    {
        var sqlConnection = new SqlConnection(_configuration.GetConnectionString("Default"));
        if (sqlConnection.State != ConnectionState.Open) await sqlConnection.OpenAsync();
        return sqlConnection;
    }


    public async Task<int?> InsertSomething(CreateProductWarehouseRequest request)
    {
        await using var connection = await GetConnection();
        var command1 = new SqlCommand("Select * From Product where IdProcut = @id", connection);
        command1.Parameters.AddWithValue("@1", request.IdProduct);
        var reader = await command1.ExecuteReaderAsync();
        if (!reader.HasRows)
        {
            return null;
        }

        var command2 = new SqlCommand("Select * from Warehouse where IdWarehouse = @1", connection);
        command2.Parameters.AddWithValue("@1", request.IdWarehouse);
        var reader2 = await command2.ExecuteReaderAsync();
        if (!reader2.HasRows)
        {
            return null;
        }

        var command3 = new SqlCommand("SELECT IdOrder  from [Order] where IdProduct = @1 and Amount = @2 AND CreatedAt < @3",
            connection);
        command3.Parameters.AddWithValue("@1", request.IdProduct);
        command3.Parameters.AddWithValue("@2", request.Amount);
        command3.Parameters.AddWithValue("@3", request.CreatedAt);
        var reader3 = await command3.ExecuteReaderAsync();
        if (!reader3.HasRows)
        {
            return null;
        }

        await reader3.ReadAsync();
        var idOrder = reader3.GetInt32(0);
        await reader3.CloseAsync();

        var command4 = new SqlCommand("SELECT * from Product_Warehouse where IdOrder = @1", connection);
        command4.Parameters.AddWithValue("@1", idOrder);
        var reader4 = await command4.ExecuteReaderAsync();
        if (!reader4.HasRows)
        {
            return null;
        }

        var command5 = new SqlCommand("UPDATE [Order] set FullfilledAt = GETDATE() where IdOrder = @1", connection);
        command5.Parameters.AddWithValue("@1", idOrder);
        await command5.ExecuteReaderAsync();

        var command6 = new SqlCommand("Select Price from Product where IdProduct = @1", connection);
        command6.Parameters.AddWithValue("@1", request.IdProduct);
        var reader6 = await command6.ExecuteReaderAsync();

        await reader6.ReadAsync();
        var price = reader6.GetDecimal(0);
        await reader6.CloseAsync();

        var finalCommand = new SqlCommand("INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES (@1, @2, @3, @4, @5, GETDATE())", connection);
        finalCommand.Parameters.AddWithValue("@1", request.IdWarehouse);
        finalCommand.Parameters.AddWithValue("@2", request.IdProduct);
        finalCommand.Parameters.AddWithValue("@3", idOrder);
        finalCommand.Parameters.AddWithValue("@4", request.Amount);
        finalCommand.Parameters.AddWithValue("@5", request.Amount + price);

        await finalCommand.ExecuteReaderAsync();

        var acualFinalCommand = new SqlCommand("SELECT IdProcuctWareHouse From Product_Warehouse where IdOrder = @1", connection);
        acualFinalCommand.Parameters.AddWithValue("@1", idOrder);
        var reader7 = await acualFinalCommand.ExecuteReaderAsync();

        await reader7.ReadAsync();
        var IdToReturn = reader7.GetInt32(0);
        await reader7.CloseAsync();
        
        return IdToReturn;
    }
}