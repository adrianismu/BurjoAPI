using Burjo.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Burjo.Infrastructure.Data.Configurations;

public class MoodLogConfiguration : IEntityTypeConfiguration<MoodLog>
{
    public void Configure(EntityTypeBuilder<MoodLog> builder)
    {
        // Table name
        builder.ToTable("mood_logs");

        // Primary key
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id)
            .HasColumnName("id")
            .ValueGeneratedNever(); // We'll set Guid manually

        // Properties
        builder.Property(m => m.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(m => m.Mood)
            .HasColumnName("mood")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(m => m.LoggedAt)
            .HasColumnName("logged_at")
            .IsRequired();

        builder.Property(m => m.Notes)
            .HasColumnName("notes")
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(m => m.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        // Relationships
        builder.HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(m => m.UserId)
            .HasDatabaseName("ix_mood_logs_user_id");

        builder.HasIndex(m => new { m.UserId, m.LoggedAt })
            .HasDatabaseName("ix_mood_logs_user_id_logged_at");

        builder.HasIndex(m => m.LoggedAt)
            .HasDatabaseName("ix_mood_logs_logged_at");
    }
}
