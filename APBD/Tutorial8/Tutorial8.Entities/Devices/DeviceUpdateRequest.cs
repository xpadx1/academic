namespace Tutorial8.Entities;

public class DeviceUpdateRequest
{
    public string Name { get; set; }
    public bool IsEnabled { get; set; }
    public byte[] OriginalRowVersion { get; set; }
}