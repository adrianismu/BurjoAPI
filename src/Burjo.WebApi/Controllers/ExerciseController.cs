using Burjo.Core.DTOs;
using Burjo.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Burjo.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExerciseController : ControllerBase
{
    private readonly IRecommendationService _recommendationService;
    private readonly IUserProfileRepository _userProfileRepository;

    public ExerciseController(
        IRecommendationService recommendationService,
        IUserProfileRepository userProfileRepository)
    {
        _recommendationService = recommendationService;
        _userProfileRepository = userProfileRepository;
    }

    /// <summary>
    /// Get personalized exercise recommendations based on user's health risk assessment
    /// </summary>
    [HttpGet("recommendations")]
    public async Task<ActionResult<IEnumerable<ExerciseDto>>> GetRecommendations()
    {
        try
        {
            var userId = GetCurrentUserId();
            var userProfile = await _userProfileRepository.GetByUserIdAsync(userId);
            
            if (userProfile == null)
            {
                return NotFound(new 
                { 
                    message = "User profile not found. Please create your profile first to get personalized recommendations." 
                });
            }

            var recommendations = await _recommendationService.GetRecommendationsAsync(userProfile.Id);
            
            if (!recommendations.Any())
            {
                return Ok(new 
                { 
                    message = "No exercise recommendations available at this time. Please complete your health assessment first.",
                    exercises = new List<ExerciseDto>()
                });
            }

            return Ok(new
            {
                message = "Exercise recommendations based on your health profile",
                totalRecommendations = recommendations.Count(),
                exercises = recommendations
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "An error occurred while getting recommendations", 
                error = ex.Message 
            });
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
