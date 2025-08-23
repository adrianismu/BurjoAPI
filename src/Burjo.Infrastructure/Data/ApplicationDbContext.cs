using Burjo.Core.Entities;
using Burjo.Infrastructure.Data.Seed;
using Microsoft.EntityFrameworkCore;

namespace Burjo.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<HealthCondition> HealthConditions { get; set; }
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<UserScheduleItem> UserScheduleItems { get; set; }
    public DbSet<MoodLog> MoodLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Apply all entity configurations from assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public async Task SeedDataAsync()
    {
        await ExerciseSeeder.SeedAsync(this);
    }
}
