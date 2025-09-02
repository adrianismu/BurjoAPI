using Burjo.Core.Interfaces;
using Burjo.Core.Models;

namespace Burjo.Infrastructure.Services;

public class ChatTimeValidationService : IChatTimeValidationService
{
    private readonly ChatSettings _chatSettings;

    public ChatTimeValidationService(ChatSettings chatSettings)
    {
        _chatSettings = chatSettings;
    }

    public bool IsWithinOperatingHours()
    {
        try
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(_chatSettings.OperatingHours.TimeZone);
            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            
            if (!TimeSpan.TryParse(_chatSettings.OperatingHours.StartTime, out var startTime) ||
                !TimeSpan.TryParse(_chatSettings.OperatingHours.EndTime, out var endTime))
            {
                // If parsing fails, default to always open
                return true;
            }

            var currentTimeSpan = currentTime.TimeOfDay;
            
            // Handle cases where end time is on the next day
            if (endTime < startTime)
            {
                return currentTimeSpan >= startTime || currentTimeSpan <= endTime;
            }
            
            return currentTimeSpan >= startTime && currentTimeSpan <= endTime;
        }
        catch
        {
            // If timezone conversion fails, default to always open
            return true;
        }
    }

    public bool ContainsEmergencyKeywords(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return false;

        var lowerMessage = message.ToLowerInvariant();
        return _chatSettings.EmergencyKeywords.Any(keyword => 
            lowerMessage.Contains(keyword.ToLowerInvariant()));
    }

    public string GetOutOfHoursMessage()
    {
        var timeUntilNext = GetTimeUntilNextOperatingHour();
        var nextOpenTime = DateTime.Now.Add(timeUntilNext);
        
        return $"{_chatSettings.OperatingHours.OutOfHoursMessage}\n\n" +
               $"⏰ Layanan akan tersedia kembali pada: {nextOpenTime:dd/MM/yyyy HH:mm} WIB\n\n" +
               $"💡 Tips sementara:\n" +
               $"• Untuk tips kesehatan umum, Anda bisa browsing artikel kesehatan terpercaya\n" +
               $"• Jaga pola makan dan istirahat yang teratur\n" +
               $"• Lakukan olahraga ringan sesuai kemampuan\n\n" +
               $"Terima kasih atas pengertian Anda! 🙏";
    }

    public string GetEmergencyMessage()
    {
        return _chatSettings.EmergencyMessage;
    }

    public TimeSpan GetTimeUntilNextOperatingHour()
    {
        try
        {
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(_chatSettings.OperatingHours.TimeZone);
            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            
            if (!TimeSpan.TryParse(_chatSettings.OperatingHours.StartTime, out var startTime))
            {
                return TimeSpan.Zero;
            }

            var nextOperatingDateTime = currentTime.Date.Add(startTime);
            
            // If we're past today's start time, move to tomorrow
            if (currentTime.TimeOfDay > startTime)
            {
                nextOperatingDateTime = nextOperatingDateTime.AddDays(1);
            }
            
            return nextOperatingDateTime - currentTime;
        }
        catch
        {
            return TimeSpan.Zero;
        }
    }
}
