using Tutorial7.API.Managers;
using Tutorial7.Entities;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new DeviceManagerDB(connectionString));

var app = builder.Build();
var api = app.MapGroup("/api");

/*if (File.Exists(filePath))
{
    deviceManager.LoadDevices(filePath);
    Console.WriteLine("Devices loaded successfully from input.txt!");
}
else
{
    Console.WriteLine("Warning: input.txt file not found. Starting with empty device list.");
}*/

// Define API routes
api.MapGet("/devices", async () =>
{
    var deviceManager = new DeviceManagerDB(connectionString);
    var devices = await deviceManager.GetAllDevicesAsync();
    var result = devices.Select(d => new { d.Id, d.Name }).ToList();
    return Results.Ok(result);
});

/*api.MapGet("/devices/{id}", (string id) =>
{
    var device = DeviceManagerDB.GetDeviceById(id);
    return device is null
        ? Results.NotFound($"Device with id {id} not found.")
        : Results.Ok(device);
});

api.MapPost("/devices", (Device device) =>
{
    DeviceManagerDB.AddDevice(device);
    return Results.Created($"/devices/{device.Id}", device);
});

api.MapPut("/devices/{id}", (string id, Device updatedDevice) =>
{
    var success = DeviceManagerDB.UpdateDevice(id, updatedDevice);
    return success
        ? Results.Ok($"Device {id} updated successfully.")
        : Results.NotFound($"Device with id {id} not found.");
});

api.MapDelete("/devices/{id}", (string id) =>
{
    var success = DeviceManagerDB.DeleteDevice(id);
    return success
        ? Results.Ok($"Device {id} deleted successfully.")
        : Results.NotFound($"Device with id {id} not found.");
});*/

app.Run();