using Burjo.Core.Entities;

namespace Burjo.Core.Interfaces;

public interface IHealthConditionRepository
{
    Task<HealthCondition?> GetByUserProfileIdAsync(Guid userProfileId);
    Task<HealthCondition> CreateAsync(HealthCondition healthCondition);
    Task<HealthCondition> UpdateAsync(HealthCondition healthCondition);
    Task<bool> DeleteAsync(Guid userProfileId);
    Task<bool> ExistsAsync(Guid userProfileId);
}
