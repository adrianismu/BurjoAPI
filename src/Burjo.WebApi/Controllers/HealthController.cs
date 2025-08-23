using Burjo.Core.DTOs;
using Burjo.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Burjo.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HealthController : ControllerBase
{
    private readonly IHealthService _healthService;
    private readonly IUserProfileRepository _userProfileRepository;

    public HealthController(
        IHealthService healthService,
        IUserProfileRepository userProfileRepository)
    {
        _healthService = healthService;
        _userProfileRepository = userProfileRepository;
    }

    /// <summary>
    /// Get health condition data for the current user
    /// </summary>
    [HttpGet("conditions")]
    public async Task<ActionResult<HealthConditionDto>> GetHealthCondition()
    {
        try
        {
            var userId = GetCurrentUserId();
            var userProfile = await _userProfileRepository.GetByUserIdAsync(userId);
            
            if (userProfile == null)
            {
                return NotFound(new { message = "User profile not found. Please create your profile first." });
            }

            var healthCondition = await _healthService.GetHealthConditionAsync(userProfile.Id);
            if (healthCondition == null)
            {
                return NotFound(new { message = "Health condition data not found" });
            }

            return Ok(healthCondition);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving health condition", error = ex.Message });
        }
    }

    /// <summary>
    /// Save or update health condition data for the current user
    /// </summary>
    [HttpPost("conditions")]
    public async Task<ActionResult> SaveHealthCondition([FromBody] HealthConditionDto healthConditionDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var userProfile = await _userProfileRepository.GetByUserIdAsync(userId);
            
            if (userProfile == null)
            {
                return NotFound(new { message = "User profile not found. Please create your profile first." });
            }

            var success = await _healthService.SaveHealthConditionAsync(userProfile.Id, healthConditionDto);
            if (!success)
            {
                return BadRequest(new { message = "Failed to save health condition data" });
            }

            return Ok(new { message = "Health condition data saved successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while saving health condition", error = ex.Message });
        }
    }

    /// <summary>
    /// Update health condition data for the current user
    /// </summary>
    [HttpPut("conditions")]
    public async Task<ActionResult> UpdateHealthCondition([FromBody] HealthConditionDto healthConditionDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var userProfile = await _userProfileRepository.GetByUserIdAsync(userId);
            
            if (userProfile == null)
            {
                return NotFound(new { message = "User profile not found. Please create your profile first." });
            }

            var success = await _healthService.UpdateHealthConditionAsync(userProfile.Id, healthConditionDto);
            if (!success)
            {
                return BadRequest(new { message = "Failed to update health condition data" });
            }

            return Ok(new { message = "Health condition data updated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating health condition", error = ex.Message });
        }
    }

    /// <summary>
    /// Get risk assessment for the current user based on health conditions
    /// </summary>
    [HttpGet("risk-assessment")]
    public async Task<ActionResult<RiskAssessmentDto>> GetRiskAssessment()
    {
        try
        {
            var userId = GetCurrentUserId();
            var userProfile = await _userProfileRepository.GetByUserIdAsync(userId);
            
            if (userProfile == null)
            {
                return NotFound(new { message = "User profile not found. Please create your profile first." });
            }

            var riskAssessment = await _healthService.AssessRiskAsync(userProfile.Id);
            return Ok(riskAssessment);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while assessing risk", error = ex.Message });
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user ID in token");
        }
        return userId;
    }
}
