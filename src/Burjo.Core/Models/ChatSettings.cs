namespace Burjo.Core.Models;

public class ChatSettings
{
    public OperatingHours OperatingHours { get; set; } = new();
    public List<string> EmergencyKeywords { get; set; } = new();
    public string EmergencyMessage { get; set; } = string.Empty;
}

public class OperatingHours
{
    public string StartTime { get; set; } = "08:00";
    public string EndTime { get; set; } = "16:00";
    public string TimeZone { get; set; } = "SE Asia Standard Time";
    public string OutOfHoursMessage { get; set; } = string.Empty;
}
