namespace Tutorial5.Entities;

public class EmbeddedDevice : Device
{
    public string Ip { get; set; }
    public string WifiName { get; set; }

    public override string GetDeviceInfo()
    {
        return $"Embedded Device: {Name}, IP: {Ip}, WiFi: {WifiName}";
    }
}