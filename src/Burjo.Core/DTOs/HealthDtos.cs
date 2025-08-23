using System.ComponentModel.DataAnnotations;
using Burjo.Core.Enums;

namespace Burjo.Core.DTOs;

public class HealthConditionDto
{
    [MaxLength(500, ErrorMessage = "Chronic diseases cannot exceed 500 characters")]
    public string ChronicDiseases { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Physical activity complaints cannot exceed 500 characters")]
    public string PhysicalActivityComplaints { get; set; } = string.Empty;

    [Range(0, 480, ErrorMessage = "Daily physical activity minutes must be between 0 and 480 (8 hours)")]
    public int DailyPhysicalActivityMinutes { get; set; }
}

public class RiskAssessmentDto
{
    public RiskCategory RiskCategory { get; set; }
    public string RiskCategoryText { get; set; } = string.Empty;
    public string RecommendationMessage { get; set; } = string.Empty;
    public List<string> RiskFactors { get; set; } = new();
}
