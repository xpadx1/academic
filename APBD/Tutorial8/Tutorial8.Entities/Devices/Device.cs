namespace Tutorial8.Entities;

public abstract class Device
{
    public string Id { get; set; }
    public string Name { get; set; }
    
    public bool IsEnabled { get; set; }

    public abstract string GetDeviceInfo();
}