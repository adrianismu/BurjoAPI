using Burjo.Core.Models;

namespace Burjo.Core.Interfaces;

public interface IChatTimeValidationService
{
    bool IsWithinOperatingHours();
    bool ContainsEmergencyKeywords(string message);
    string GetOutOfHoursMessage();
    string GetEmergencyMessage();
    TimeSpan GetTimeUntilNextOperatingHour();
}
