using Burjo.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Burjo.Infrastructure.Data.Configurations;

public class HealthConditionConfiguration : IEntityTypeConfiguration<HealthCondition>
{
    public void Configure(EntityTypeBuilder<HealthCondition> builder)
    {
        // Table name
        builder.ToTable("health_conditions");

        // Primary key
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Id)
            .HasColumnName("id")
            .ValueGeneratedNever(); // We'll set Guid manually

        // Properties
        builder.Property(h => h.UserProfileId)
            .HasColumnName("user_profile_id")
            .IsRequired();

        builder.Property(h => h.ChronicDiseases)
            .HasColumnName("chronic_diseases")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(h => h.PhysicalActivityComplaints)
            .HasColumnName("physical_activity_complaints")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(h => h.DailyPhysicalActivityMinutes)
            .HasColumnName("daily_physical_activity_minutes")
            .IsRequired();

        builder.Property(h => h.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(h => h.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        // Relationships
        builder.HasOne(h => h.UserProfile)
            .WithOne(up => up.HealthCondition)
            .HasForeignKey<HealthCondition>(h => h.UserProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(h => h.UserProfileId)
            .IsUnique()
            .HasDatabaseName("ix_health_conditions_user_profile_id");
    }
}
