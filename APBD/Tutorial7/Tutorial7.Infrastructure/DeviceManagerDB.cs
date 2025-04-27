using Microsoft.Data.SqlClient;
using Tutorial7.Entities;

public class DeviceManagerDB
{
    private readonly string _connectionString;

    public DeviceManagerDB(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Device>> GetAllDevicesAsync()
    {
        var devices = new List<Device>();

        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var command = new SqlCommand("SELECT * FROM Devices", connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            // Map SQL rows to Device objects
        }

        return devices;
    }

    // Similarly: GetById, Create, Update, Delete
}