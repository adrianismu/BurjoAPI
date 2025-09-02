using Burjo.Core.DTOs;

namespace Burjo.Core.Interfaces;

public interface IMoodService
{
    Task<bool> LogMoodAsync(Guid userId, MoodLogDto moodLogDto);
    Task<WeeklyMoodHistoryDto> GetWeeklyMoodHistoryAsync(Guid userId, DateTime? weekStartDate = null);
    Task<MoodLogResponseDto?> GetLatestMoodAsync(Guid userId);
    Task<bool> HasLoggedMoodTodayAsync(Guid userId);
}

public interface IChatService
{
    Task<ChatResponseDto> GetResponseAsync(string userMessage, Guid userId);
    ChatOperatingHoursDto GetOperatingHoursInfo();
}
