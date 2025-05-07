using System.Data;
using Microsoft.Data.SqlClient;
using Tutorial8.Entities;

public class DeviceManagerDB
{
    private readonly string _connectionString;

    public DeviceManagerDB(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<DeviceDto>> GetAllDevicesAsync()
    {
        var devices = new List<DeviceDto>();
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var command = new SqlCommand("SELECT * FROM Device", connection);
        await using var reader = await command.ExecuteReaderAsync();

        if (reader.HasRows) {
            while (await reader.ReadAsync()) {
                var device = new DeviceDto {
                    Id = reader.GetString  (reader.GetOrdinal("Id")),
                    Name = reader.GetString (reader.GetOrdinal("Name")),
                    IsEnabled = reader.GetBoolean(reader.GetOrdinal("isEnabled")),
                };
                devices.Add(device);
            }
        }
        return devices;
    }
    
    public async Task<DeviceDto?> GetDeviceByIdAsync(string id)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        var command = new SqlCommand("SELECT Id, Name, IsEnabled, RowVersion FROM Device WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = await command.ExecuteReaderAsync();

        if (!await reader.ReadAsync())
        {
            return null;
        }

        var device = new DeviceDto
        {
            Id = reader.GetString(reader.GetOrdinal("Id")),
            Name = reader.GetString(reader.GetOrdinal("Name")),
            IsEnabled = reader.GetBoolean(reader.GetOrdinal("IsEnabled")),
            RowVersion = (byte[])reader["RowVersion"]
        };

        reader.Close();
        
        device = await LoadDeviceDetailsAsync(device, connection);

        return device;
    }
    
    private async Task<DeviceDto> LoadDeviceDetailsAsync(DeviceDto device, SqlConnection connection)
    {
        var command = new SqlCommand(@"
                SELECT BatteryPercentage FROM Smartwatch WHERE DeviceId = @DeviceId
                UNION ALL
                SELECT OperationSystem FROM PersonalComputer WHERE DeviceId = @DeviceId
                UNION ALL
                SELECT IpAddress + ' / ' + NetworkName FROM Embedded WHERE DeviceId = @DeviceId
            ", connection);

        command.Parameters.AddWithValue("@DeviceId", device.Id);

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            device.ExtraInfo ??= new List<string>();
            device.ExtraInfo.Add(reader.GetString(0));
        }

        return device;
    }
    
    public async Task CreateDeviceAsync(string id, DeviceCreateRequest request)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var deviceCommand = new SqlCommand(
                    "EXEC AddDevice @Id, @Name, @IsEnabled", connection, (SqlTransaction)transaction);
                deviceCommand.Parameters.AddWithValue("@Id", id);
                deviceCommand.Parameters.AddWithValue("@Name", request.Name);
                deviceCommand.Parameters.AddWithValue("@IsEnabled", request.IsEnabled);

                await deviceCommand.ExecuteNonQueryAsync();

                if (request.DeviceType == "SW")
                {
                    var smartwatchCmd = new SqlCommand(
                        "EXEC AddSmartwatch @Battery, @DeviceId", connection, (SqlTransaction)transaction);
                    smartwatchCmd.Parameters.AddWithValue("@Battery", request.BatteryPercentage);
                    smartwatchCmd.Parameters.AddWithValue("@DeviceId", id);

                    await smartwatchCmd.ExecuteNonQueryAsync();
                }
                else if (request.DeviceType == "ED")
                {
                    var embeddedCmd = new SqlCommand(
                        "EXEC AddEmbedded @Ip, @Network, @DeviceId", connection, (SqlTransaction)transaction);
                    embeddedCmd.Parameters.AddWithValue("@Ip", request.IpAddress);
                    embeddedCmd.Parameters.AddWithValue("@Network", request.NetworkName);
                    embeddedCmd.Parameters.AddWithValue("@DeviceId", id);

                    await embeddedCmd.ExecuteNonQueryAsync();
                }
                else if (request.DeviceType == "P")
                {
                    var pcCmd = new SqlCommand(
                        "EXEC AddPersonalComputer @OS, @DeviceId", connection, (SqlTransaction)transaction);
                    pcCmd.Parameters.AddWithValue("@OS", request.OperationSystem);
                    pcCmd.Parameters.AddWithValue("@DeviceId", id);
                    await pcCmd.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    
    public async Task UpdateDeviceAsync(string id, DeviceUpdateRequest request)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var deviceCmd = new SqlCommand(
                "EXEC UpdateDevice @Id, @Name, @IsEnabled, @OriginalRowVersion", connection, (SqlTransaction)transaction);

            deviceCmd.Parameters.AddWithValue("@Id", id);
            deviceCmd.Parameters.AddWithValue("@Name", request.Name);
            deviceCmd.Parameters.AddWithValue("@IsEnabled", request.IsEnabled);
            deviceCmd.Parameters.AddWithValue("@OriginalRowVersion", request.OriginalRowVersion);

            await deviceCmd.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
        }
        catch (SqlException ex) when (ex.Number == 50001)
        {
            await transaction.RollbackAsync();
            throw new DBConcurrencyException("The device was updated by another user.", ex);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteDeviceAsync(string id)
    {
        using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var command = new SqlCommand("EXEC DeleteDevice @Id", connection, (SqlTransaction)transaction);
            command.Parameters.AddWithValue("@Id", id);
            await command.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    private static readonly Random _random = new Random();

    public static string GenerateId(string deviceType)
    {
        string[] letterCount = ["P", "SW", "ED"];
        int numberCount = _random.Next(1, 3);
        string resultType = "";

        if (letterCount.Contains(deviceType))
        {
            resultType = deviceType;
        }
        else
        {
            resultType = "UNKNOWN";
        }
        
        string numbers = "";
        for (int i = 0; i < numberCount; i++)
        {
            numbers += _random.Next(0, 10);
        }

        return new string(resultType) + "-" + numbers;
    }
}