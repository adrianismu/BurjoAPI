using Burjo.Core.DTOs;
using Burjo.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Burjo.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MoodController : ControllerBase
{
    private readonly IMoodService _moodService;

    public MoodController(IMoodService moodService)
    {
        _moodService = moodService;
    }

    /// <summary>
    /// Log mood for the current user
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> LogMood([FromBody] MoodLogDto moodLogDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var hasLoggedToday = await _moodService.HasLoggedMoodTodayAsync(userId);

            var success = await _moodService.LogMoodAsync(userId, moodLogDto);
            if (!success)
            {
                return BadRequest(new { message = "Failed to log mood" });
            }

            var message = hasLoggedToday 
                ? "Mood berhasil diperbarui untuk hari ini" 
                : "Mood berhasil dicatat untuk hari ini";

            return Ok(new 
            { 
                message = message,
                mood = moodLogDto.Mood.ToString(),
                loggedAt = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "An error occurred while logging mood", 
                error = ex.Message 
            });
        }
    }

    /// <summary>
    /// Get weekly mood history for the current user
    /// </summary>
    [HttpGet("history")]
    public async Task<ActionResult<WeeklyMoodHistoryDto>> GetWeeklyMoodHistory([FromQuery] DateTime? weekStartDate = null)
    {
        try
        {
            var userId = GetCurrentUserId();
            var weeklyHistory = await _moodService.GetWeeklyMoodHistoryAsync(userId, weekStartDate);

            return Ok(new
            {
                message = "Weekly mood history retrieved successfully",
                weekPeriod = $"{weeklyHistory.WeekStartDate:dd/MM/yyyy} - {weeklyHistory.WeekEndDate:dd/MM/yyyy}",
                data = weeklyHistory
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "An error occurred while retrieving mood history", 
                error = ex.Message 
            });
        }
    }

    /// <summary>
    /// Get latest mood for the current user
    /// </summary>
    [HttpGet("latest")]
    public async Task<ActionResult<MoodLogResponseDto>> GetLatestMood()
    {
        try
        {
            var userId = GetCurrentUserId();
            var latestMood = await _moodService.GetLatestMoodAsync(userId);

            if (latestMood == null)
            {
                return NotFound(new { message = "No mood log found. Start tracking your mood today!" });
            }

            var hasLoggedToday = await _moodService.HasLoggedMoodTodayAsync(userId);

            return Ok(new
            {
                message = "Latest mood retrieved successfully",
                hasLoggedToday = hasLoggedToday,
                data = latestMood
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "An error occurred while retrieving latest mood", 
                error = ex.Message 
            });
        }
    }

    /// <summary>
    /// Check if user has logged mood today
    /// </summary>
    [HttpGet("today-status")]
    public async Task<ActionResult> GetTodayMoodStatus()
    {
        try
        {
            var userId = GetCurrentUserId();
            var hasLoggedToday = await _moodService.HasLoggedMoodTodayAsync(userId);

            return Ok(new
            {
                hasLoggedToday = hasLoggedToday,
                message = hasLoggedToday 
                    ? "Anda sudah mencatat mood hari ini" 
                    : "Anda belum mencatat mood hari ini",
                date = DateTime.Today.ToString("dd/MM/yyyy")
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "An error occurred while checking today's mood status", 
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
