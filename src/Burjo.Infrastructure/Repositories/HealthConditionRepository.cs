using Burjo.Core.Entities;
using Burjo.Core.Interfaces;
using Burjo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Burjo.Infrastructure.Repositories;

public class HealthConditionRepository : IHealthConditionRepository
{
    private readonly ApplicationDbContext _context;

    public HealthConditionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<HealthCondition?> GetByUserProfileIdAsync(Guid userProfileId)
    {
        return await _context.HealthConditions
            .Include(h => h.UserProfile)
            .FirstOrDefaultAsync(h => h.UserProfileId == userProfileId);
    }

    public async Task<HealthCondition> CreateAsync(HealthCondition healthCondition)
    {
        _context.HealthConditions.Add(healthCondition);
        await _context.SaveChangesAsync();
        return healthCondition;
    }

    public async Task<HealthCondition> UpdateAsync(HealthCondition healthCondition)
    {
        _context.HealthConditions.Update(healthCondition);
        await _context.SaveChangesAsync();
        return healthCondition;
    }

    public async Task<bool> DeleteAsync(Guid userProfileId)
    {
        var healthCondition = await GetByUserProfileIdAsync(userProfileId);
        if (healthCondition == null)
            return false;

        _context.HealthConditions.Remove(healthCondition);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(Guid userProfileId)
    {
        return await _context.HealthConditions
            .AnyAsync(h => h.UserProfileId == userProfileId);
    }
}
