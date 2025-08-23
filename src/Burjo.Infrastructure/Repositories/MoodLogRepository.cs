using Burjo.Core.Entities;
using Burjo.Core.Interfaces;
using Burjo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Burjo.Infrastructure.Repositories;

public class MoodLogRepository : IMoodLogRepository
{
    private readonly ApplicationDbContext _context;

    public MoodLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MoodLog?> GetByIdAsync(Guid id)
    {
        return await _context.MoodLogs
            .Include(m => m.User)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<MoodLog>> GetByUserIdAsync(Guid userId)
    {
        return await _context.MoodLogs
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.LoggedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<MoodLog>> GetByUserIdAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
    {
        return await _context.MoodLogs
            .Where(m => m.UserId == userId && 
                       m.LoggedAt >= startDate && 
                       m.LoggedAt <= endDate)
            .OrderByDescending(m => m.LoggedAt)
            .ToListAsync();
    }

    public async Task<MoodLog?> GetLatestByUserIdAsync(Guid userId)
    {
        return await _context.MoodLogs
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.LoggedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<MoodLog> CreateAsync(MoodLog moodLog)
    {
        moodLog.CreatedAt = DateTime.UtcNow;
        moodLog.UpdatedAt = DateTime.UtcNow;
        
        _context.MoodLogs.Add(moodLog);
        await _context.SaveChangesAsync();
        return moodLog;
    }

    public async Task<MoodLog> UpdateAsync(MoodLog moodLog)
    {
        moodLog.UpdatedAt = DateTime.UtcNow;
        
        _context.MoodLogs.Update(moodLog);
        await _context.SaveChangesAsync();
        return moodLog;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var moodLog = await GetByIdAsync(id);
        if (moodLog == null)
            return false;

        _context.MoodLogs.Remove(moodLog);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> HasMoodLogTodayAsync(Guid userId)
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        
        return await _context.MoodLogs
            .AnyAsync(m => m.UserId == userId && 
                          m.LoggedAt >= today && 
                          m.LoggedAt < tomorrow);
    }
}
