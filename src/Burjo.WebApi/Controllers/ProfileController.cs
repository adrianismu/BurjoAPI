using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Burjo.Core.DTOs;
using Burjo.Core.Entities;
using Burjo.Core.Interfaces;

namespace Burjo.WebApi.Controllers;

[Authorize]
public class ProfileController : BaseApiController
{
    private readonly IUserProfileRepository _profileRepository;
    private readonly IUserRepository _userRepository;

    public ProfileController(IUserProfileRepository profileRepository, IUserRepository userRepository)
    {
        _profileRepository = profileRepository;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            var profile = await _profileRepository.GetByUserIdAsync(userId);
            if (profile == null)
            {
                return NotFound(new { message = "Profile not found. Please create your profile first." });
            }

            var profileDto = new UserProfileDto
            {
                FullName = profile.FullName,
                Age = profile.Age,
                Gender = profile.Gender,
                HeightCm = profile.HeightCm,
                WeightKg = profile.WeightKg,
                MedicalHistory = profile.MedicalHistory,
                FitnessLevel = profile.FitnessLevel,
                Bmi = profile.Bmi // Calculated BMI
            };

            return Ok(profileDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to get profile", error = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UserProfileDto profileDto)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            // Check if user exists
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Get existing profile or create new one
            var existingProfile = await _profileRepository.GetByUserIdAsync(userId);
            
            if (existingProfile == null)
            {
                // Create new profile
                var newProfile = new UserProfile
                {
                    UserId = userId,
                    FullName = profileDto.FullName,
                    Age = profileDto.Age,
                    Gender = profileDto.Gender,
                    HeightCm = profileDto.HeightCm,
                    WeightKg = profileDto.WeightKg,
                    MedicalHistory = profileDto.MedicalHistory,
                    FitnessLevel = profileDto.FitnessLevel
                };

                var createdProfile = await _profileRepository.CreateAsync(newProfile);
                
                var responseDto = new UserProfileDto
                {
                    FullName = createdProfile.FullName,
                    Age = createdProfile.Age,
                    Gender = createdProfile.Gender,
                    HeightCm = createdProfile.HeightCm,
                    WeightKg = createdProfile.WeightKg,
                    MedicalHistory = createdProfile.MedicalHistory,
                    FitnessLevel = createdProfile.FitnessLevel,
                    Bmi = createdProfile.Bmi
                };

                return Ok(responseDto);
            }
            else
            {
                // Update existing profile
                existingProfile.FullName = profileDto.FullName;
                existingProfile.Age = profileDto.Age;
                existingProfile.Gender = profileDto.Gender;
                existingProfile.HeightCm = profileDto.HeightCm;
                existingProfile.WeightKg = profileDto.WeightKg;
                existingProfile.MedicalHistory = profileDto.MedicalHistory;
                existingProfile.FitnessLevel = profileDto.FitnessLevel;

                var updatedProfile = await _profileRepository.UpdateAsync(existingProfile);

                var responseDto = new UserProfileDto
                {
                    FullName = updatedProfile.FullName,
                    Age = updatedProfile.Age,
                    Gender = updatedProfile.Gender,
                    HeightCm = updatedProfile.HeightCm,
                    WeightKg = updatedProfile.WeightKg,
                    MedicalHistory = updatedProfile.MedicalHistory,
                    FitnessLevel = updatedProfile.FitnessLevel,
                    Bmi = updatedProfile.Bmi
                };

                return Ok(responseDto);
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to update profile", error = ex.Message });
        }
    }
}
