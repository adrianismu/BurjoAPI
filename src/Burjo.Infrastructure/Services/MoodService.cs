using Burjo.Core.DTOs;
using Burjo.Core.Entities;
using Burjo.Core.Enums;
using Burjo.Core.Interfaces;

namespace Burjo.Infrastructure.Services;

public class MoodService : IMoodService
{
    private readonly IMoodLogRepository _moodLogRepository;

    public MoodService(IMoodLogRepository moodLogRepository)
    {
        _moodLogRepository = moodLogRepository;
    }

    public async Task<bool> LogMoodAsync(Guid userId, MoodLogDto moodLogDto)
    {
        try
        {
            var moodLog = new MoodLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Mood = moodLogDto.Mood,
                Notes = moodLogDto.Notes,
                LoggedAt = DateTime.UtcNow
            };

            await _moodLogRepository.CreateAsync(moodLog);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<WeeklyMoodHistoryDto> GetWeeklyMoodHistoryAsync(Guid userId, DateTime? weekStartDate = null)
    {
        // If no week start date provided, use current week
        var startDate = weekStartDate ?? GetStartOfWeek(DateTime.Today);
        var endDate = startDate.AddDays(7).AddTicks(-1); // End of the week

        var moodLogs = await _moodLogRepository.GetByUserIdAndDateRangeAsync(userId, startDate, endDate);

        var moodLogDtos = moodLogs.Select(ml => new MoodLogResponseDto
        {
            Id = ml.Id,
            Mood = ml.Mood,
            MoodText = GetMoodText(ml.Mood),
            LoggedAt = ml.LoggedAt,
            Notes = ml.Notes
        }).ToList();

        var statistics = CalculateStatistics(moodLogDtos);

        return new WeeklyMoodHistoryDto
        {
            WeekStartDate = startDate,
            WeekEndDate = endDate,
            MoodLogs = moodLogDtos,
            Statistics = statistics
        };
    }

    public async Task<MoodLogResponseDto?> GetLatestMoodAsync(Guid userId)
    {
        var latestMood = await _moodLogRepository.GetLatestByUserIdAsync(userId);
        if (latestMood == null)
            return null;

        return new MoodLogResponseDto
        {
            Id = latestMood.Id,
            Mood = latestMood.Mood,
            MoodText = GetMoodText(latestMood.Mood),
            LoggedAt = latestMood.LoggedAt,
            Notes = latestMood.Notes
        };
    }

    public async Task<bool> HasLoggedMoodTodayAsync(Guid userId)
    {
        return await _moodLogRepository.HasMoodLogTodayAsync(userId);
    }

    private static DateTime GetStartOfWeek(DateTime date)
    {
        // Get Monday of the current week
        var diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
        return date.AddDays(-1 * diff).Date;
    }

    private static string GetMoodText(MoodType mood)
    {
        return mood switch
        {
            MoodType.SangatBaik => "Sangat Baik",
            MoodType.Baik => "Baik",
            MoodType.Sedang => "Sedang",
            MoodType.Buruk => "Buruk",
            _ => "Unknown"
        };
    }

    private static MoodStatisticsDto CalculateStatistics(List<MoodLogResponseDto> moodLogs)
    {
        if (!moodLogs.Any())
        {
            return new MoodStatisticsDto
            {
                TotalLogs = 0,
                MoodDistribution = new Dictionary<string, int>()
            };
        }

        var moodCounts = moodLogs
            .GroupBy(ml => ml.Mood)
            .ToDictionary(g => GetMoodText(g.Key), g => g.Count());

        var mostFrequentMood = moodLogs
            .GroupBy(ml => ml.Mood)
            .OrderByDescending(g => g.Count())
            .First().Key;

        // Calculate average mood score (1=SangatBaik, 4=Buruk, so lower is better)
        var averageScore = moodLogs.Average(ml => (int)ml.Mood);

        return new MoodStatisticsDto
        {
            TotalLogs = moodLogs.Count,
            MostFrequentMood = mostFrequentMood,
            MostFrequentMoodText = GetMoodText(mostFrequentMood),
            AverageMoodScore = Math.Round(averageScore, 2),
            MoodDistribution = moodCounts
        };
    }
}
