using Burjo.Core.Entities;

namespace Burjo.Core.Interfaces;

public interface IUserScheduleRepository
{
    Task<UserScheduleItem?> GetByIdAsync(Guid id);
    Task<IEnumerable<UserScheduleItem>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<UserScheduleItem>> GetByUserIdAndDayAsync(Guid userId, DayOfWeek day);
    Task<UserScheduleItem> CreateAsync(UserScheduleItem scheduleItem);
    Task<UserScheduleItem> UpdateAsync(UserScheduleItem scheduleItem);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> DeleteAllByUserIdAsync(Guid userId);
}
