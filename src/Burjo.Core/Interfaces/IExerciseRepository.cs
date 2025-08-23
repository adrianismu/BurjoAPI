using Burjo.Core.Entities;
using Burjo.Core.Enums;

namespace Burjo.Core.Interfaces;

public interface IExerciseRepository
{
    Task<Exercise?> GetByIdAsync(Guid id);
    Task<IEnumerable<Exercise>> GetAllAsync();
    Task<IEnumerable<Exercise>> GetByTargetRiskCategoryAsync(RiskCategory riskCategory);
    Task<Exercise> CreateAsync(Exercise exercise);
    Task<Exercise> UpdateAsync(Exercise exercise);
    Task<bool> DeleteAsync(Guid id);
}
