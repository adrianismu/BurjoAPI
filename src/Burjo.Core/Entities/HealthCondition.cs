namespace Burjo.Core.Entities;

public class HealthCondition
{
    public Guid Id { get; set; }
    public string ChronicDiseases { get; set; } = string.Empty; // Comma-separated: "Diabetes tipe 2, Hipertensi"
    public string PhysicalActivityComplaints { get; set; } = string.Empty; // e.g., "Nyeri dada"
    public int DailyPhysicalActivityMinutes { get; set; } // e.g., 25
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Foreign key
    public Guid UserProfileId { get; set; }
    
    // Navigation property
    public UserProfile UserProfile { get; set; } = null!;
}
