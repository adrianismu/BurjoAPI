using Burjo.Core.Entities;
using Burjo.Core.Interfaces;
using Burjo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Burjo.Infrastructure.Repositories;

public class UserScheduleRepository : IUserScheduleRepository
{
    private readonly ApplicationDbContext _context;

    public UserScheduleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserScheduleItem?> GetByIdAsync(Guid id)
    {
        return await _context.UserScheduleItems
            .Include(s => s.User)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<UserScheduleItem>> GetByUserIdAsync(Guid userId)
    {
        return await _context.UserScheduleItems
            .Where(s => s.UserId == userId)
            .OrderBy(s => s.Day)
            .ThenBy(s => s.ExerciseName)
            .ToListAsync();
    }

    public async Task<IEnumerable<UserScheduleItem>> GetByUserIdAndDayAsync(Guid userId, DayOfWeek day)
    {
        return await _context.UserScheduleItems
            .Where(s => s.UserId == userId && s.Day == day)
            .OrderBy(s => s.ExerciseName)
            .ToListAsync();
    }

    public async Task<UserScheduleItem> CreateAsync(UserScheduleItem scheduleItem)
    {
        scheduleItem.CreatedAt = DateTime.UtcNow;
        scheduleItem.UpdatedAt = DateTime.UtcNow;
        
        _context.UserScheduleItems.Add(scheduleItem);
        await _context.SaveChangesAsync();
        return scheduleItem;
    }

    public async Task<UserScheduleItem> UpdateAsync(UserScheduleItem scheduleItem)
    {
        scheduleItem.UpdatedAt = DateTime.UtcNow;
        
        _context.UserScheduleItems.Update(scheduleItem);
        await _context.SaveChangesAsync();
        return scheduleItem;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var scheduleItem = await GetByIdAsync(id);
        if (scheduleItem == null)
            return false;

        _context.UserScheduleItems.Remove(scheduleItem);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAllByUserIdAsync(Guid userId)
    {
        var scheduleItems = await _context.UserScheduleItems
            .Where(s => s.UserId == userId)
            .ToListAsync();

        if (!scheduleItems.Any())
            return true;

        _context.UserScheduleItems.RemoveRange(scheduleItems);
        await _context.SaveChangesAsync();
        return true;
    }
}
