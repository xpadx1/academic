namespace Tutorial5.Entities;

public class Smartwatch : Device
{
    public bool IsCellular { get; set; }
    public string BatteryPercentage { get; set; }

    public override string GetDeviceInfo()
    {
        return $"Smartwatch: {Name}, Cellular: {IsCellular}, Battery: {BatteryPercentage}";
    }
}