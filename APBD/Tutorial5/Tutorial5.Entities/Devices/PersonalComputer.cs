namespace Tutorial5.Entities;

public class PersonalComputer : Device
{
    public string OperatingSystem { get; set; } = "Unknown OS";

    public override string GetDeviceInfo()
    {
        return $"PC: {Name}, OS: {OperatingSystem}";
    }
}