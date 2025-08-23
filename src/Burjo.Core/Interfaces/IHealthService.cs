using Burjo.Core.DTOs;
using Burjo.Core.Enums;

namespace Burjo.Core.Interfaces;

public interface IHealthService
{
    Task<RiskAssessmentDto> AssessRiskAsync(Guid userProfileId);
    Task<bool> SaveHealthConditionAsync(Guid userProfileId, HealthConditionDto healthConditionDto);
    Task<HealthConditionDto?> GetHealthConditionAsync(Guid userProfileId);
    Task<bool> UpdateHealthConditionAsync(Guid userProfileId, HealthConditionDto healthConditionDto);
}
