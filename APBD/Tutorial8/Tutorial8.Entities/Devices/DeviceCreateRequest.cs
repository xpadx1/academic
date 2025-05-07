namespace Tutorial8.Entities;

public class DeviceCreateRequest
{
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
    public string DeviceType { get; set; }
    public string OperationSystem { get; set; }
    public string BatteryPercentage { get; set; }
    public string IpAddress { get; set; }
    public string NetworkName { get; set; }
}