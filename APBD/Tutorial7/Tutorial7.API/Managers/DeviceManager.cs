using Tutorial7.Entities;

namespace Tutorial7.API.Managers;

class DeviceManager
{
    private static readonly List<Device> _devices = new();
    public void LoadDevices(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            switch (parts[0][0])
            {
                case 'S':
                    _devices.Add(new Smartwatch
                    {
                        Id = parts[0],
                        Name = parts[1],
                        IsCellular = bool.Parse(parts[2]),
                        BatteryPercentage = parts[3]
                    });
                    break;
                case 'P':
                    _devices.Add(new PersonalComputer
                    {
                        Id = parts[0],
                        Name = parts[1],
                        OperatingSystem = parts.Length > 3 ? parts[3] : "Unknown OS"
                    });
                    break;
                case 'E':
                    _devices.Add(new EmbeddedDevice
                    {
                        Id = parts[0],
                        Name = parts[1],
                        Ip = parts[2],
                        WifiName = parts[3]
                    });
                    break;
                default:
                    Console.WriteLine($"Unrecognized device type: {line}");
                    break;
            }
        }
    }
    
    public static List<Device> GetAllDevices()
    {
        return _devices;
    }

    public static Device GetDeviceById(string id)
    {
        return _devices.FirstOrDefault(d => d.Id == id);
    }

    public static void AddDevice(Device device)
    {
        _devices.Add(device);
    }

    public static bool UpdateDevice(string id, Device updatedDevice)
    {
        var device = GetDeviceById(id);
        if (device == null)
        {
            return false;
        }

        device.Name = updatedDevice.Name;
        return true;
    }

    public static bool DeleteDevice(string id)
    {
        var device = GetDeviceById(id);
        if (device == null)
        {
            return false;
        }

        _devices.Remove(device);
        return true;
    }
}