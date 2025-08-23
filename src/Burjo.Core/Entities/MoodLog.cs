using Burjo.Core.Enums;

namespace Burjo.Core.Entities;

public class MoodLog
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public MoodType Mood { get; set; }
    public DateTime LoggedAt { get; set; }
    public string? Notes { get; set; } // Optional notes about the mood
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation property
    public User User { get; set; } = null!;
}
