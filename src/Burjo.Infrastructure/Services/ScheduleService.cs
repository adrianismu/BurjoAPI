using Burjo.Core.DTOs;
using Burjo.Core.Entities;
using Burjo.Core.Interfaces;

namespace Burjo.Infrastructure.Services;

public class ScheduleService : IScheduleService
{
    private readonly IUserScheduleRepository _scheduleRepository;

    public ScheduleService(IUserScheduleRepository scheduleRepository)
    {
        _scheduleRepository = scheduleRepository;
    }

    public async Task<WeeklyScheduleDto> GetWeeklyScheduleAsync(Guid userId)
    {
        var scheduleItems = await _scheduleRepository.GetByUserIdAsync(userId);
        
        var weeklySchedule = new WeeklyScheduleDto();
        
        foreach (var item in scheduleItems)
        {
            var scheduleItemDto = new ScheduleItemDto
            {
                Id = item.Id,
                ExerciseName = item.ExerciseName,
                Day = item.Day,
                DayText = GetDayText(item.Day),
                DurationMinutes = item.DurationMinutes
            };

            switch (item.Day)
            {
                case DayOfWeek.Monday:
                    weeklySchedule.Monday.Add(scheduleItemDto);
                    break;
                case DayOfWeek.Tuesday:
                    weeklySchedule.Tuesday.Add(scheduleItemDto);
                    break;
                case DayOfWeek.Wednesday:
                    weeklySchedule.Wednesday.Add(scheduleItemDto);
                    break;
                case DayOfWeek.Thursday:
                    weeklySchedule.Thursday.Add(scheduleItemDto);
                    break;
                case DayOfWeek.Friday:
                    weeklySchedule.Friday.Add(scheduleItemDto);
                    break;
                case DayOfWeek.Saturday:
                    weeklySchedule.Saturday.Add(scheduleItemDto);
                    break;
                case DayOfWeek.Sunday:
                    weeklySchedule.Sunday.Add(scheduleItemDto);
                    break;
            }
        }

        return weeklySchedule;
    }

    public async Task<bool> CreateOrUpdateWeeklyScheduleAsync(Guid userId, CreateScheduleRequestDto scheduleRequest)
    {
        try
        {
            // Clear existing schedule for the user
            await _scheduleRepository.DeleteAllByUserIdAsync(userId);

            // Create new schedule items
            foreach (var scheduleItemDto in scheduleRequest.ScheduleItems)
            {
                var scheduleItem = new UserScheduleItem
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    ExerciseName = scheduleItemDto.ExerciseName,
                    Day = scheduleItemDto.Day,
                    DurationMinutes = scheduleItemDto.DurationMinutes
                };

                await _scheduleRepository.CreateAsync(scheduleItem);
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteScheduleItemAsync(Guid userId, Guid scheduleItemId)
    {
        var scheduleItem = await _scheduleRepository.GetByIdAsync(scheduleItemId);
        
        // Ensure the schedule item belongs to the requesting user
        if (scheduleItem == null || scheduleItem.UserId != userId)
            return false;

        return await _scheduleRepository.DeleteAsync(scheduleItemId);
    }

    public async Task<bool> ClearWeeklyScheduleAsync(Guid userId)
    {
        return await _scheduleRepository.DeleteAllByUserIdAsync(userId);
    }

    private static string GetDayText(DayOfWeek day)
    {
        return day switch
        {
            DayOfWeek.Monday => "Senin",
            DayOfWeek.Tuesday => "Selasa",
            DayOfWeek.Wednesday => "Rabu",
            DayOfWeek.Thursday => "Kamis",
            DayOfWeek.Friday => "Jumat",
            DayOfWeek.Saturday => "Sabtu",
            DayOfWeek.Sunday => "Minggu",
            _ => "Unknown"
        };
    }
}
