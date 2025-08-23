using Burjo.Core.DTOs;
using Burjo.Core.Entities;
using Burjo.Core.Enums;
using Burjo.Core.Interfaces;

namespace Burjo.Infrastructure.Services;

public class HealthService : IHealthService
{
    private readonly IHealthConditionRepository _healthConditionRepository;
    private readonly IUserProfileRepository _userProfileRepository;

    public HealthService(
        IHealthConditionRepository healthConditionRepository,
        IUserProfileRepository userProfileRepository)
    {
        _healthConditionRepository = healthConditionRepository;
        _userProfileRepository = userProfileRepository;
    }

    public async Task<RiskAssessmentDto> AssessRiskAsync(Guid userProfileId)
    {
        var userProfile = await _userProfileRepository.GetByIdAsync(userProfileId);
        if (userProfile == null)
            throw new ArgumentException("User profile not found", nameof(userProfileId));

        var healthCondition = await _healthConditionRepository.GetByUserProfileIdAsync(userProfileId);
        if (healthCondition == null)
        {
            return new RiskAssessmentDto
            {
                RiskCategory = RiskCategory.Aman,
                RiskCategoryText = "Aman",
                RecommendationMessage = "Belum ada data kondisi kesehatan. Silakan lengkapi data kesehatan untuk penilaian risiko yang akurat.",
                RiskFactors = new List<string> { "Data kondisi kesehatan belum tersedia" }
            };
        }

        var riskFactors = new List<string>();
        var chronicDiseases = ParseChronicDiseases(healthCondition.ChronicDiseases);
        var physicalComplaints = ParsePhysicalComplaints(healthCondition.PhysicalActivityComplaints);

        // Rule 1: Heart disease OR chest pain = Supervisi Medis
        if (chronicDiseases.Contains("penyakit jantung") || 
            physicalComplaints.Contains("nyeri dada"))
        {
            riskFactors.Add("Memiliki penyakit jantung atau keluhan nyeri dada");
            return new RiskAssessmentDto
            {
                RiskCategory = RiskCategory.SupervisiMedis,
                RiskCategoryText = "Supervisi Medis",
                RecommendationMessage = "Diperlukan supervisi medis sebelum memulai program latihan. Konsultasikan dengan dokter untuk program latihan yang aman.",
                RiskFactors = riskFactors
            };
        }

        // Rule 2: Hypertension AND less than 30 minutes daily activity = Pengawasan Ringan
        if (chronicDiseases.Contains("hipertensi") && 
            healthCondition.DailyPhysicalActivityMinutes < 30)
        {
            riskFactors.Add("Memiliki hipertensi");
            riskFactors.Add("Aktivitas fisik harian kurang dari 30 menit");
            return new RiskAssessmentDto
            {
                RiskCategory = RiskCategory.PengawasanRingan,
                RiskCategoryText = "Pengawasan Ringan",
                RecommendationMessage = "Disarankan memulai latihan dengan intensitas ringan dan meningkatkan secara bertahap. Monitor tekanan darah secara berkala.",
                RiskFactors = riskFactors
            };
        }

        // Check for other risk factors
        if (chronicDiseases.Contains("diabetes"))
            riskFactors.Add("Memiliki diabetes");
        
        if (chronicDiseases.Contains("hipertensi"))
            riskFactors.Add("Memiliki hipertensi");

        if (healthCondition.DailyPhysicalActivityMinutes < 30)
            riskFactors.Add("Aktivitas fisik harian kurang dari 30 menit");

        if (physicalComplaints.Contains("sesak napas"))
            riskFactors.Add("Mengalami keluhan sesak napas");

        if (physicalComplaints.Contains("pusing"))
            riskFactors.Add("Mengalami keluhan pusing");

        // Rule 3: Default = Aman
        return new RiskAssessmentDto
        {
            RiskCategory = RiskCategory.Aman,
            RiskCategoryText = "Aman",
            RecommendationMessage = riskFactors.Any() 
                ? "Anda dapat memulai program latihan dengan tetap memperhatikan kondisi kesehatan. Konsultasikan dengan ahli jika mengalami keluhan selama latihan."
                : "Anda dalam kondisi baik untuk memulai program latihan. Tetap jaga konsistensi dan tingkatkan intensitas secara bertahap.",
            RiskFactors = riskFactors.Any() ? riskFactors : new List<string> { "Tidak ada faktor risiko yang teridentifikasi" }
        };
    }

    public async Task<bool> SaveHealthConditionAsync(Guid userProfileId, HealthConditionDto healthConditionDto)
    {
        var userProfile = await _userProfileRepository.GetByIdAsync(userProfileId);
        if (userProfile == null)
            return false;

        var existingHealthCondition = await _healthConditionRepository.GetByUserProfileIdAsync(userProfileId);
        
        if (existingHealthCondition != null)
        {
            // Update existing
            existingHealthCondition.ChronicDiseases = healthConditionDto.ChronicDiseases;
            existingHealthCondition.PhysicalActivityComplaints = healthConditionDto.PhysicalActivityComplaints;
            existingHealthCondition.DailyPhysicalActivityMinutes = healthConditionDto.DailyPhysicalActivityMinutes;
            existingHealthCondition.UpdatedAt = DateTime.UtcNow;

            await _healthConditionRepository.UpdateAsync(existingHealthCondition);
        }
        else
        {
            // Create new
            var healthCondition = new HealthCondition
            {
                Id = Guid.NewGuid(),
                UserProfileId = userProfileId,
                ChronicDiseases = healthConditionDto.ChronicDiseases,
                PhysicalActivityComplaints = healthConditionDto.PhysicalActivityComplaints,
                DailyPhysicalActivityMinutes = healthConditionDto.DailyPhysicalActivityMinutes,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _healthConditionRepository.CreateAsync(healthCondition);
        }

        return true;
    }

    public async Task<HealthConditionDto?> GetHealthConditionAsync(Guid userProfileId)
    {
        var healthCondition = await _healthConditionRepository.GetByUserProfileIdAsync(userProfileId);
        if (healthCondition == null)
            return null;

        return new HealthConditionDto
        {
            ChronicDiseases = healthCondition.ChronicDiseases,
            PhysicalActivityComplaints = healthCondition.PhysicalActivityComplaints,
            DailyPhysicalActivityMinutes = healthCondition.DailyPhysicalActivityMinutes
        };
    }

    public async Task<bool> UpdateHealthConditionAsync(Guid userProfileId, HealthConditionDto healthConditionDto)
    {
        return await SaveHealthConditionAsync(userProfileId, healthConditionDto);
    }

    private static List<string> ParseChronicDiseases(string chronicDiseases)
    {
        if (string.IsNullOrWhiteSpace(chronicDiseases))
            return new List<string>();

        return chronicDiseases
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(d => d.Trim().ToLowerInvariant())
            .ToList();
    }

    private static List<string> ParsePhysicalComplaints(string physicalComplaints)
    {
        if (string.IsNullOrWhiteSpace(physicalComplaints))
            return new List<string>();

        return physicalComplaints
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(c => c.Trim().ToLowerInvariant())
            .ToList();
    }
}
