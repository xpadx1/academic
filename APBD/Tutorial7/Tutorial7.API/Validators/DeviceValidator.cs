using System.Net;
using System.Text.RegularExpressions;
using Tutorial7.Entities;

namespace Tutorial7.API.Validators
{
    public static class DeviceValidator
    {
        public static string ValidateNewDevice(DeviceCreateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return "Device name is required.";

            if (string.IsNullOrWhiteSpace(request.DeviceType))
                return "Device type must be specified (PC, Embedded, Smartwatch).";

            if (request.IsEnabled != true && request.IsEnabled != false && request.IsEnabled != null)
                return "IsEnabled must be true, false or null.";

            switch (request.DeviceType.ToLower())
            {
                case "pc":
                    if (string.IsNullOrWhiteSpace(request.OperationSystem))
                        return "Operation system is required for PC.";
                    break;

                case "smartwatch":
                    if (string.IsNullOrWhiteSpace(request.BatteryPercentage))
                        return "Battery percentage is required for Smartwatch.";
                    if (!IsValidBatteryPercentage(request.BatteryPercentage))
                        return "Invalid battery percentage format. Example valid: '27%'.";
                    break;

                case "embedded":
                    if (string.IsNullOrWhiteSpace(request.IpAddress) || string.IsNullOrWhiteSpace(request.NetworkName))
                        return "IP address and Network name are required for Embedded devices.";
                    if (!IsValidIp(request.IpAddress))
                        return "Invalid IP address format.";
                    break;

                default:
                    return $"Unknown device type '{request.DeviceType}'. Allowed types: PC, Embedded, Smartwatch.";
            }

            return null; // all good
        }

        public static string ValidateUpdateDevice(DeviceUpdateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return "Device name is required.";

            if (request.IsEnabled != true && request.IsEnabled != false && request.IsEnabled != null)
                return "IsEnabled must be true, false or null.";

            return null; // all good
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