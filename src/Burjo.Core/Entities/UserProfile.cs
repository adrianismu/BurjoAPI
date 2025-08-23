namespace Burjo.Core.Entities;

public class UserProfile
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty; // e.g., "Laki-laki", "Perempuan"
    public double HeightCm { get; set; }
    public double WeightKg { get; set; }
    public string MedicalHistory { get; set; } = string.Empty; // e.g., "Hipertensi, Diabetes"
    public string FitnessLevel { get; set; } = string.Empty; // e.g., "Rendah", "Sedang", "Tinggi"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Foreign key
    public Guid UserId { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
    public HealthCondition? HealthCondition { get; set; }
    
    // Computed property - BMI calculation
    public double Bmi => HeightCm > 0 ? Math.Round(WeightKg / Math.Pow(HeightCm / 100, 2), 2) : 0;
}
