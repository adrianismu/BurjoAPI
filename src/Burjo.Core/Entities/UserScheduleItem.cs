namespace Burjo.Core.Entities;

public class UserScheduleItem
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public DayOfWeek Day { get; set; }
    public int DurationMinutes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation property
    public User User { get; set; } = null!;
}
