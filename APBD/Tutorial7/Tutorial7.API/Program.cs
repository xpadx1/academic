using Tutorial7.Entities;
using Tutorial7.API.Validators;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new DeviceManagerDB(connectionString));
var deviceManager = new DeviceManagerDB(connectionString);

var app = builder.Build();
var api = app.MapGroup("/api");

api.MapGet("/devices", async () =>
{
    var devices = await deviceManager.GetAllDevicesAsync();
    var result = devices.Select(d => new { d.Id, d.Name }).ToList();
    Console.WriteLine(result.Count);
    return Results.Ok(result);
});

api.MapGet("/devices/{id}", async (string id, DeviceManagerDB deviceManager) =>
{
    var device = await deviceManager.GetDeviceByIdAsync(id);
    if (device is null)
    {
        return Results.NotFound($"Device with id '{id}' not found.");
    }
    return Results.Ok(device);
});

api.MapPost("/devices", async (DeviceCreateRequest request, DeviceManagerDB deviceManager) =>
{
    var id = DeviceManagerDB.GenerateRandomId();

    var validationError = DeviceValidator.ValidateNewDevice(request);
    if (validationError != null)
    {
        return Results.BadRequest(validationError);
    }

    await deviceManager.CreateDeviceAsync(id, request);
    return Results.Created($"/api/devices/{id}", new { id });
});

api.MapPut("/devices/{id}", async (string id, DeviceUpdateRequest request, DeviceManagerDB deviceManager) =>
{
    var device = await deviceManager.GetDeviceByIdAsync(id);
    if (device is null)
    {
        return Results.NotFound($"Device with id '{id}' not found.");
    }

    var validationError = DeviceValidator.ValidateUpdateDevice(request);
    if (validationError != null)
    {
        return Results.BadRequest(validationError);
    }

    await deviceManager.UpdateDeviceAsync(id, request);
    return Results.Ok($"Updated device with id {id}.");
});

api.MapDelete("/devices/{id}", async (string id, DeviceManagerDB deviceManager) =>
{
    var device = await deviceManager.GetDeviceByIdAsync(id);
    if (device is null)
    {
        return Results.NotFound($"Device with id '{id}' not found.");
    }

    await deviceManager.DeleteDeviceAsync(id);
    return Results.Ok($"Deleted device with id {id}.");
});

app.Run();