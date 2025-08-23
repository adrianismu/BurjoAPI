using Burjo.Core.DTOs;
using Burjo.Core.Enums;
using Burjo.Core.Interfaces;

namespace Burjo.Infrastructure.Services;

public class RecommendationService : IRecommendationService
{
    private readonly IHealthService _healthService;
    private readonly IExerciseRepository _exerciseRepository;

    public RecommendationService(
        IHealthService healthService,
        IExerciseRepository exerciseRepository)
    {
        _healthService = healthService;
        _exerciseRepository = exerciseRepository;
    }

    public async Task<IEnumerable<ExerciseDto>> GetRecommendationsAsync(Guid userId)
    {
        try
        {
            // Get user's risk assessment
            var riskAssessment = await _healthService.AssessRiskAsync(userId);
            
            // Get exercises suitable for user's risk category
            var exercises = await _exerciseRepository.GetByTargetRiskCategoryAsync(riskAssessment.RiskCategory);
            
            // Also include exercises for safer categories
            // For example, if user is "PengawasanRingan", also include "Aman" exercises
            var additionalExercises = new List<Core.Entities.Exercise>();
            
            if (riskAssessment.RiskCategory == RiskCategory.PengawasanRingan)
            {
                var safeExercises = await _exerciseRepository.GetByTargetRiskCategoryAsync(RiskCategory.Aman);
                additionalExercises.AddRange(safeExercises);
            }
            else if (riskAssessment.RiskCategory == RiskCategory.SupervisiMedis)
            {
                // For medical supervision, only include very safe exercises
                var safeExercises = await _exerciseRepository.GetByTargetRiskCategoryAsync(RiskCategory.Aman);
                additionalExercises.AddRange(safeExercises);
            }

            // Combine and deduplicate exercises
            var allExercises = exercises.Concat(additionalExercises)
                .GroupBy(e => e.Id)
                .Select(g => g.First())
                .OrderBy(e => e.TargetRiskCategory)
                .ThenBy(e => e.Name);

            // Convert to DTOs
            return allExercises.Select(exercise => new ExerciseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
                TargetRiskCategory = exercise.TargetRiskCategory,
                TargetRiskCategoryText = GetRiskCategoryText(exercise.TargetRiskCategory)
            });
        }
        catch (ArgumentException)
        {
            // If user profile not found, return general safe exercises
            var safeExercises = await _exerciseRepository.GetByTargetRiskCategoryAsync(RiskCategory.Aman);
            
            return safeExercises.Select(exercise => new ExerciseDto
            {
                Id = exercise.Id,
                Name = exercise.Name,
                Description = exercise.Description,
                TargetRiskCategory = exercise.TargetRiskCategory,
                TargetRiskCategoryText = GetRiskCategoryText(exercise.TargetRiskCategory)
            });
        }
    }

    private static string GetRiskCategoryText(RiskCategory riskCategory)
    {
        return riskCategory switch
        {
            RiskCategory.Aman => "Aman",
            RiskCategory.PengawasanRingan => "Pengawasan Ringan",
            RiskCategory.SupervisiMedis => "Supervisi Medis",
            _ => "Unknown"
        };
    }
}
