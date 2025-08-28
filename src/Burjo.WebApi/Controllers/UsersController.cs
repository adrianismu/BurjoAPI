using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Burjo.Core.DTOs;
using Burjo.Core.Interfaces;

namespace Burjo.WebApi.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IUserProfileRepository _profileRepository;

    public UsersController(IUserRepository userRepository, IUserProfileRepository profileRepository)
    {
        _userRepository = userRepository;
        _profileRepository = profileRepository;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var profile = await _profileRepository.GetByUserIdAsync(userId);

            var response = new
            {
                id = user.Id,
                email = user.Email,
                isActive = user.IsActive,
                createdAt = user.CreatedAt,
                profile = profile != null ? new UserProfileDto
                {
                    FullName = profile.FullName,
                    Age = profile.Age,
                    Gender = profile.Gender,
                    HeightCm = profile.HeightCm,
                    WeightKg = profile.WeightKg,
                    MedicalHistory = profile.MedicalHistory,
                    FitnessLevel = profile.FitnessLevel,
                    Bmi = profile.Bmi
                } : null
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to get user data", error = ex.Message });
        }
    }

    [HttpDelete("me")]
    public async Task<IActionResult> DeleteCurrentUser()
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            var result = await _userRepository.DeleteAsync(userId);
            if (!result)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { message = "User account deleted successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to delete user account", error = ex.Message });
        }
    }

    // Admin endpoint to get all users (can be protected with admin role later)
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userRepository.GetAllAsync();
            var response = new List<object>();

            foreach (var user in users)
            {
                var profile = await _profileRepository.GetByUserIdAsync(user.Id);
                response.Add(new
                {
                    id = user.Id,
                    email = user.Email,
                    isActive = user.IsActive,
                    createdAt = user.CreatedAt,
                    hasProfile = profile != null,
                    profileSummary = profile != null ? new
                    {
                        fullName = profile.FullName,
                        age = profile.Age,
                        gender = profile.Gender,
                        fitnessLevel = profile.FitnessLevel
                    } : null
                });
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Failed to get users", error = ex.Message });
        }
    }
}
