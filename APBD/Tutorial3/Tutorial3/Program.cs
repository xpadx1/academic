using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DeviceManagement
{
    /// <summary> Base class representing a generic device. </summary>
    public abstract class Device
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public abstract void DisplayInfo();
    }

    /// <summary> Smartwatch device class. </summary>
    public class Smartwatch : Device
    {
        public bool IsCellular { get; set; }
        public string BatteryPercentage { get; set; }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Smartwatch: {Name}, Cellular: {IsCellular}, Battery: {BatteryPercentage}");
        }
    }

    /// <summary> Personal Computer device class. </summary>
    public class PersonalComputer : Device
    {
        public string OperatingSystem { get; set; } = "Unknown OS";

        public override void DisplayInfo()
        {
            Console.WriteLine($"PC: {Name}, OS: {OperatingSystem}");
        }
    }

    /// <summary> Embedded Device class. </summary>
    public class EmbeddedDevice : Device
    {
        public string IP { get; set; }
        public string WifiName { get; set; }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Embedded Device: {Name}, IP: {IP}, WiFi: {WifiName}");
        }
    }

    /// <summary> Device Manager class for handling device parsing and management. </summary>
    public class DeviceManager
    {
        private readonly List<Device> _devices = new List<Device>();

        /// <summary> Parses the input file and initializes the device list. </summary>
        /// <param name="filePath">Path to the input file.</param>
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
                            IP = parts[2],
                            WifiName = parts[3]
                        });
                        break;
                    default:
                        Console.WriteLine($"Unrecognized device type: {line}");
                        break;
                }
            }
        }

        /// <summary> Displays all parsed devices. </summary>
        public void DisplayDevices()
        {
            foreach (var device in _devices)
            {
                device.DisplayInfo();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var manager = new DeviceManager();
            const string filePath = "C:\\Programming\\Repositories\\academic\\APBD\\Tutorial3\\Tutorial3\\input.txt";

            if (File.Exists(filePath))
            {
                manager.LoadDevices(filePath);
                manager.DisplayDevices();
            }
            else
            {
                Console.WriteLine("Error: input.txt file not found.");
            }
        }
    }
}
