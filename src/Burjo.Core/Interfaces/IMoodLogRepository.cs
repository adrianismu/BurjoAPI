using Burjo.Core.Entities;
using Burjo.Core.Enums;

namespace Burjo.Core.Interfaces;

public interface IMoodLogRepository
{
    Task<MoodLog?> GetByIdAsync(Guid id);
    Task<IEnumerable<MoodLog>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<MoodLog>> GetByUserIdAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
    Task<MoodLog?> GetLatestByUserIdAsync(Guid userId);
    Task<MoodLog> CreateAsync(MoodLog moodLog);
    Task<MoodLog> UpdateAsync(MoodLog moodLog);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> HasMoodLogTodayAsync(Guid userId);
}
