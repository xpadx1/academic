using Microsoft.Data.SqlClient;
using Tutorial7.Entities;

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

        var command = new SqlCommand("SELECT Id, Name, IsEnabled FROM Device WHERE Id = @Id", connection);
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
            IsEnabled = reader.GetBoolean(reader.GetOrdinal("IsEnabled"))
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
                    "INSERT INTO Device (Id, Name, IsEnabled) VALUES (@Id, @Name, @IsEnabled)", connection, (SqlTransaction)transaction);
                deviceCommand.Parameters.AddWithValue("@Id", id);
                deviceCommand.Parameters.AddWithValue("@Name", request.Name);
                deviceCommand.Parameters.AddWithValue("@IsEnabled", request.IsEnabled);

                await deviceCommand.ExecuteNonQueryAsync();

                if (request.DeviceType == "Smartwatch")
                {
                    var smartwatchCmd = new SqlCommand(
                        "INSERT INTO Smartwatch (BatteryPercentage, DeviceId) VALUES (@Battery, @DeviceId)", connection, (SqlTransaction)transaction);
                    smartwatchCmd.Parameters.AddWithValue("@Battery", request.BatteryPercentage);
                    smartwatchCmd.Parameters.AddWithValue("@DeviceId", id);

                    await smartwatchCmd.ExecuteNonQueryAsync();
                }
                else if (request.DeviceType == "Embedded")
                {
                    var embeddedCmd = new SqlCommand(
                        "INSERT INTO Embedded (IpAddress, NetworkName, DeviceId) VALUES (@Ip, @Network, @DeviceId)", connection, (SqlTransaction)transaction);
                    embeddedCmd.Parameters.AddWithValue("@Ip", request.IpAddress);
                    embeddedCmd.Parameters.AddWithValue("@Network", request.NetworkName);
                    embeddedCmd.Parameters.AddWithValue("@DeviceId", id);

                    await embeddedCmd.ExecuteNonQueryAsync();
                }
                else if (request.DeviceType == "PC")
                {
                    var pcCmd = new SqlCommand(
                        "INSERT INTO PersonalComputer (OperationSystem, DeviceId) VALUES (@OS, @DeviceId)", connection, (SqlTransaction)transaction);
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
                "UPDATE Device SET Name = @Name, IsEnabled = @IsEnabled WHERE Id = @Id", connection, (SqlTransaction)transaction);
            deviceCmd.Parameters.AddWithValue("@Name", request.Name);
            deviceCmd.Parameters.AddWithValue("@IsEnabled", request.IsEnabled);
            deviceCmd.Parameters.AddWithValue("@Id", id);

            await deviceCmd.ExecuteNonQueryAsync();
            await transaction.CommitAsync();
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
        var command = new SqlCommand("DELETE FROM Device WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);
        await command.ExecuteNonQueryAsync();
    }
    
    private static readonly Random _random = new Random();

    public static string GenerateRandomId()
    {
        int letterCount = _random.Next(1, 4);
        int numberCount = _random.Next(1, 3);

        char[] letters = new char[letterCount];
        for (int i = 0; i < letterCount; i++)
        {
            letters[i] = (char)_random.Next('A', 'Z' + 1);
        }

        string numbers = "";
        for (int i = 0; i < numberCount; i++)
        {
            numbers += _random.Next(0, 10);
        }

        return new string(letters) + "-" + numbers;
    }
}