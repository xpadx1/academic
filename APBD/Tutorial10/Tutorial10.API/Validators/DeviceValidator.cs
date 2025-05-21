using System.Net;
using System.Text.RegularExpressions;
using Tutorial8.Entities;

namespace Tutorial8.API.Validators
{
    public static class DeviceValidator
    {
        public static string ValidateNewDevice(DeviceCreateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return "Device Name is required.";

            if (string.IsNullOrWhiteSpace(request.DeviceType))
                return "DeviceType must be specified (P, ED, SW).";

            if (request.IsEnabled != true && request.IsEnabled != false && request.IsEnabled != null)
                return "IsEnabled must be true, false or null.";

            switch (request.DeviceType.ToLower())
            {
                case "p":
                    if (string.IsNullOrWhiteSpace(request.OperationSystem))
                        return "OperationSystem is required for PC.";
                    break;

                case "sw":
                    if (string.IsNullOrWhiteSpace(request.BatteryPercentage))
                        return "BatteryPercentage is required for Smartwatch.";
                    if (!IsValidBatteryPercentage(request.BatteryPercentage))
                        return "Invalid BatteryPercentage format. Example valid: '27%'.";
                    break;

                case "ed":
                    if (string.IsNullOrWhiteSpace(request.IpAddress) || string.IsNullOrWhiteSpace(request.NetworkName))
                        return "IpAddress and NetworkName are required for Embedded devices.";
                    if (!IsValidIp(request.IpAddress))
                        return "Invalid IpAddress format.";
                    break;

                default:
                    return $"Unknown device type '{request.DeviceType}'. Allowed types: P, ED, SW.";
            }

            return null;
        }

        public static string ValidateUpdateDevice(DeviceUpdateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return "Device Name is required.";

            if (request.IsEnabled != true && request.IsEnabled != false && request.IsEnabled != null)
                return "IsEnabled must be true, false or null.";

            return null;
        }

        private static bool IsValidIp(string ip)
        {
            return IPAddress.TryParse(ip, out _);
        }

        private static bool IsValidBatteryPercentage(string percentage)
        {
            return Regex.IsMatch(percentage, @"^\d{1,3}%$");
        }
    }
}