using Burjo.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Burjo.Infrastructure.Data.Configurations;

public class UserScheduleItemConfiguration : IEntityTypeConfiguration<UserScheduleItem>
{
    public void Configure(EntityTypeBuilder<UserScheduleItem> builder)
    {
        // Table name
        builder.ToTable("user_schedule_items");

        // Primary key
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .HasColumnName("id")
            .ValueGeneratedNever(); // We'll set Guid manually

        // Properties
        builder.Property(s => s.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(s => s.ExerciseName)
            .HasColumnName("exercise_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Day)
            .HasColumnName("day")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(s => s.DurationMinutes)
            .HasColumnName("duration_minutes")
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        // Relationships
        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(s => s.UserId)
            .HasDatabaseName("ix_user_schedule_items_user_id");

        builder.HasIndex(s => new { s.UserId, s.Day })
            .HasDatabaseName("ix_user_schedule_items_user_id_day");
    }
}
