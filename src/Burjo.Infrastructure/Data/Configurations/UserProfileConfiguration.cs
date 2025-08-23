using Burjo.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Burjo.Infrastructure.Data.Configurations;

public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.ToTable("UserProfiles");
        
        builder.HasKey(p => p.Id);
        
        // Remove PostgreSQL-specific default value for SQLite compatibility
        // Entity Framework will handle Guid generation
        
        builder.Property(p => p.FullName)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(p => p.Gender)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(p => p.HeightCm)
            .HasPrecision(5, 2);
            
        builder.Property(p => p.WeightKg)
            .HasPrecision(5, 2);
            
        builder.Property(p => p.MedicalHistory)
            .HasMaxLength(1000);
            
        builder.Property(p => p.FitnessLevel)
            .IsRequired()
            .HasMaxLength(50);
            
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
        builder.Property(p => p.UpdatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
        // Ignore computed properties
        builder.Ignore(p => p.Bmi);
        
        // Foreign key
        builder.HasIndex(p => p.UserId)
            .IsUnique()
            .HasDatabaseName("IX_UserProfiles_UserId");
    }
}
