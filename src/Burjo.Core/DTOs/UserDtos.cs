using System.ComponentModel.DataAnnotations;

namespace Burjo.Core.DTOs;

public class RegisterUserDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; } = string.Empty;
}

public class LoginUserDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}

public class UserProfileDto
{
    [Required(ErrorMessage = "Full name is required")]
    [MaxLength(200, ErrorMessage = "Full name cannot exceed 200 characters")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Age is required")]
    [Range(1, 150, ErrorMessage = "Age must be between 1 and 150")]
    public int Age { get; set; }

    [Required(ErrorMessage = "Gender is required")]
    public string Gender { get; set; } = string.Empty; // e.g., "Laki-laki", "Perempuan"

    [Required(ErrorMessage = "Height is required")]
    [Range(50, 300, ErrorMessage = "Height must be between 50 and 300 cm")]
    public double HeightCm { get; set; }

    [Required(ErrorMessage = "Weight is required")]
    [Range(20, 500, ErrorMessage = "Weight must be between 20 and 500 kg")]
    public double WeightKg { get; set; }

    [MaxLength(1000, ErrorMessage = "Medical history cannot exceed 1000 characters")]
    public string MedicalHistory { get; set; } = string.Empty; // e.g., "Hipertensi, Diabetes"

    [Required(ErrorMessage = "Fitness level is required")]
    public string FitnessLevel { get; set; } = string.Empty; // e.g., "Rendah", "Sedang", "Tinggi"

    public double Bmi { get; set; } // Calculated field
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UserProfileDto? Profile { get; set; }
}
