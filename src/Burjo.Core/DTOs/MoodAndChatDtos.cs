using System.ComponentModel.DataAnnotations;
using Burjo.Core.Enums;

namespace Burjo.Core.DTOs;

public class MoodLogDto
{
    [Required(ErrorMessage = "Mood is required")]
    public MoodType Mood { get; set; }

    [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
    public string? Notes { get; set; }
}

public class MoodLogResponseDto
{
    public Guid Id { get; set; }
    public MoodType Mood { get; set; }
    public string MoodText { get; set; } = string.Empty;
    public DateTime LoggedAt { get; set; }
    public string? Notes { get; set; }
}

public class WeeklyMoodHistoryDto
{
    public DateTime WeekStartDate { get; set; }
    public DateTime WeekEndDate { get; set; }
    public List<MoodLogResponseDto> MoodLogs { get; set; } = new();
    public MoodStatisticsDto Statistics { get; set; } = new();
}

public class MoodStatisticsDto
{
    public int TotalLogs { get; set; }
    public MoodType? MostFrequentMood { get; set; }
    public string? MostFrequentMoodText { get; set; }
    public double AverageMoodScore { get; set; }
    public Dictionary<string, int> MoodDistribution { get; set; } = new();
}

public class ChatMessageDto
{
    [Required(ErrorMessage = "Message is required")]
    [MaxLength(1000, ErrorMessage = "Message cannot exceed 1000 characters")]
    public string Message { get; set; } = string.Empty;
}

public class ChatResponseDto
{
    public string Response { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string? SuggestedActions { get; set; }
    public List<string> QuickReplies { get; set; } = new();
}
