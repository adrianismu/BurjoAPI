using Burjo.Core.DTOs;

namespace Burjo.Core.Interfaces;

public interface IRecommendationService
{
    Task<IEnumerable<ExerciseDto>> GetRecommendationsAsync(Guid userId);
}

public interface IScheduleService
{
    Task<WeeklyScheduleDto> GetWeeklyScheduleAsync(Guid userId);
    Task<bool> CreateOrUpdateWeeklyScheduleAsync(Guid userId, CreateScheduleRequestDto scheduleRequest);
    Task<bool> DeleteScheduleItemAsync(Guid userId, Guid scheduleItemId);
    Task<bool> ClearWeeklyScheduleAsync(Guid userId);
}
