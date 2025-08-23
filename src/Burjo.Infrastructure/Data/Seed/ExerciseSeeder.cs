using Burjo.Core.Entities;
using Burjo.Core.Enums;
using Burjo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Burjo.Infrastructure.Data.Seed;

public static class ExerciseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Exercises.AnyAsync())
        {
            return; // Already seeded
        }

        var exercises = new List<Exercise>
        {
            // Exercises for "Aman" (Safe) category
            new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Jalan Kaki",
                Description = "Aktivitas jalan kaki ringan selama 20-30 menit dengan intensitas rendah. Cocok untuk semua kalangan dan aman untuk pemula.",
                TargetRiskCategory = RiskCategory.Aman,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Yoga",
                Description = "Latihan yoga dengan gerakan dasar untuk meningkatkan fleksibilitas dan ketenangan pikiran. Fokus pada pernapasan dan peregangan ringan.",
                TargetRiskCategory = RiskCategory.Aman,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Peregangan",
                Description = "Latihan peregangan untuk meningkatkan fleksibilitas otot dan mengurangi ketegangan. Dapat dilakukan di rumah tanpa alat khusus.",
                TargetRiskCategory = RiskCategory.Aman,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Tai Chi",
                Description = "Gerakan lambat dan terkontrol yang membantu keseimbangan, koordinasi, dan relaksasi. Sangat aman untuk semua usia.",
                TargetRiskCategory = RiskCategory.Aman,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },

            // Exercises for "Pengawasan Ringan" (Light Supervision) category
            new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Bersepeda Santai",
                Description = "Bersepeda dengan intensitas ringan di jalur datar. Monitor detak jantung dan istirahat jika merasa lelah berlebihan.",
                TargetRiskCategory = RiskCategory.PengawasanRingan,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Renang Ringan",
                Description = "Berenang dengan gaya bebas atau gaya dada dengan intensitas rendah. Pastikan ada pengawas dan tidak memaksakan diri.",
                TargetRiskCategory = RiskCategory.PengawasanRingan,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Senam Aerobik Ringan",
                Description = "Gerakan senam aerobik dengan intensitas rendah. Fokus pada pergerakan yang terkontrol dan hindari lompatan tinggi.",
                TargetRiskCategory = RiskCategory.PengawasanRingan,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Latihan Beban Ringan",
                Description = "Menggunakan dumbbell atau resistance band dengan beban minimal. Fokus pada repetisi tinggi dengan beban rendah.",
                TargetRiskCategory = RiskCategory.PengawasanRingan,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },

            // Exercises for "Supervisi Medis" (Medical Supervision) category
            new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Terapi Fisik Terpandu",
                Description = "Program latihan yang dirancang khusus oleh fisioterapis. Harus dilakukan dengan pengawasan medis langsung.",
                TargetRiskCategory = RiskCategory.SupervisiMedis,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Jalan Terapi",
                Description = "Program jalan kaki yang sangat terkontrol dengan monitoring detak jantung dan tekanan darah secara berkala.",
                TargetRiskCategory = RiskCategory.SupervisiMedis,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Latihan Pernapasan",
                Description = "Teknik pernapasan khusus untuk meningkatkan kapasitas paru-paru. Dilakukan dengan pengawasan tenaga medis.",
                TargetRiskCategory = RiskCategory.SupervisiMedis,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new Exercise
            {
                Id = Guid.NewGuid(),
                Name = "Rehabilitasi Kardiak",
                Description = "Program khusus untuk pemulihan jantung. Hanya boleh dilakukan di fasilitas medis dengan pengawasan dokter spesialis.",
                TargetRiskCategory = RiskCategory.SupervisiMedis,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };

        context.Exercises.AddRange(exercises);
        await context.SaveChangesAsync();
    }
}
