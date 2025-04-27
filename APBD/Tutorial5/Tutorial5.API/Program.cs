using Tutorial5.API.Managers;
using Tutorial5.Entities;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var deviceManager = new DeviceManager();
var filePath = Path.Combine(AppContext.BaseDirectory, "input.txt");

if (File.Exists(filePath))
{
    deviceManager.LoadDevices(filePath);
    Console.WriteLine("Devices loaded successfully from input.txt!");
}
else
{
    Console.WriteLine("Warning: input.txt file not found. Starting with empty device list.");
}

// Define API routes
app.MapGet("/devices", () =>
{
    var devices = DeviceManager.GetAllDevices()
        .Select(d => new { d.Id, d.Name })
        .ToList();
    return Results.Ok(devices);
});

app.MapGet("/devices/{id}", (string id) =>
{
    var device = DeviceManager.GetDeviceById(id);
    return device is null
        ? Results.NotFound($"Device with id {id} not found.")
        : Results.Ok(device);
});

app.MapPost("/devices", (Device device) =>
{
    DeviceManager.AddDevice(device);
    return Results.Created($"/devices/{device.Id}", device);
});

app.MapPut("/devices/{id}", (string id, Device updatedDevice) =>
{
    var success = DeviceManager.UpdateDevice(id, updatedDevice);
    return success
        ? Results.Ok($"Device {id} updated successfully.")
        : Results.NotFound($"Device with id {id} not found.");
});

app.MapDelete("/devices/{id}", (string id) =>
{
    var success = DeviceManager.DeleteDevice(id);
    return success
        ? Results.Ok($"Device {id} deleted successfully.")
        : Results.NotFound($"Device with id {id} not found.");
});

app.Run();