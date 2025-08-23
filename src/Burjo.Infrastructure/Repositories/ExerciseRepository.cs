using Burjo.Core.Entities;
using Burjo.Core.Enums;
using Burjo.Core.Interfaces;
using Burjo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Burjo.Infrastructure.Repositories;

public class ExerciseRepository : IExerciseRepository
{
    private readonly ApplicationDbContext _context;

    public ExerciseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Exercise?> GetByIdAsync(Guid id)
    {
        return await _context.Exercises.FindAsync(id);
    }

    public async Task<IEnumerable<Exercise>> GetAllAsync()
    {
        return await _context.Exercises
            .OrderBy(e => e.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Exercise>> GetByTargetRiskCategoryAsync(RiskCategory riskCategory)
    {
        return await _context.Exercises
            .Where(e => e.TargetRiskCategory == riskCategory)
            .OrderBy(e => e.Name)
            .ToListAsync();
    }

    public async Task<Exercise> CreateAsync(Exercise exercise)
    {
        exercise.CreatedAt = DateTime.UtcNow;
        exercise.UpdatedAt = DateTime.UtcNow;
        
        _context.Exercises.Add(exercise);
        await _context.SaveChangesAsync();
        return exercise;
    }

    public async Task<Exercise> UpdateAsync(Exercise exercise)
    {
        exercise.UpdatedAt = DateTime.UtcNow;
        
        _context.Exercises.Update(exercise);
        await _context.SaveChangesAsync();
        return exercise;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var exercise = await GetByIdAsync(id);
        if (exercise == null)
            return false;

        _context.Exercises.Remove(exercise);
        await _context.SaveChangesAsync();
        return true;
    }
}
