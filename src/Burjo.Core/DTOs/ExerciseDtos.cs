using System.ComponentModel.DataAnnotations;
using Burjo.Core.Enums;

namespace Burjo.Core.DTOs;

public class ExerciseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RiskCategory TargetRiskCategory { get; set; }
    public string TargetRiskCategoryText { get; set; } = string.Empty;
}

public class UserScheduleItemDto
{
    [Required(ErrorMessage = "Exercise name is required")]
    [MaxLength(100, ErrorMessage = "Exercise name cannot exceed 100 characters")]
    public string ExerciseName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Day is required")]
    public DayOfWeek Day { get; set; }

    [Range(1, 480, ErrorMessage = "Duration must be between 1 and 480 minutes")]
    public int DurationMinutes { get; set; }
}

public class WeeklyScheduleDto
{
    public List<ScheduleItemDto> Monday { get; set; } = new();
    public List<ScheduleItemDto> Tuesday { get; set; } = new();
    public List<ScheduleItemDto> Wednesday { get; set; } = new();
    public List<ScheduleItemDto> Thursday { get; set; } = new();
    public List<ScheduleItemDto> Friday { get; set; } = new();
    public List<ScheduleItemDto> Saturday { get; set; } = new();
    public List<ScheduleItemDto> Sunday { get; set; } = new();
}

public class ScheduleItemDto
{
    public Guid Id { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public DayOfWeek Day { get; set; }
    public string DayText { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
}

public class CreateScheduleRequestDto
{
    [Required]
    public List<UserScheduleItemDto> ScheduleItems { get; set; } = new();
}
