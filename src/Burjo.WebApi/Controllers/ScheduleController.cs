using Burjo.Core.DTOs;
using Burjo.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Burjo.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    /// <summary>
    /// Get weekly exercise schedule for the current user
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<WeeklyScheduleDto>> GetWeeklySchedule()
    {
        try
        {
            var userId = GetCurrentUserId();
            var weeklySchedule = await _scheduleService.GetWeeklyScheduleAsync(userId);

            var totalExercises = weeklySchedule.Monday.Count + weeklySchedule.Tuesday.Count + 
                               weeklySchedule.Wednesday.Count + weeklySchedule.Thursday.Count + 
                               weeklySchedule.Friday.Count + weeklySchedule.Saturday.Count + 
                               weeklySchedule.Sunday.Count;

            return Ok(new
            {
                message = "Weekly exercise schedule retrieved successfully",
                totalScheduledExercises = totalExercises,
                schedule = weeklySchedule
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "An error occurred while retrieving schedule", 
                error = ex.Message 
            });
        }
    }

    /// <summary>
    /// Create or update weekly exercise schedule for the current user
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> CreateOrUpdateWeeklySchedule([FromBody] CreateScheduleRequestDto scheduleRequest)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var success = await _scheduleService.CreateOrUpdateWeeklyScheduleAsync(userId, scheduleRequest);

            if (!success)
            {
                return BadRequest(new { message = "Failed to create or update weekly schedule" });
            }

            return Ok(new 
            { 
                message = "Weekly exercise schedule created/updated successfully",
                totalScheduledExercises = scheduleRequest.ScheduleItems.Count
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "An error occurred while creating/updating schedule", 
                error = ex.Message 
            });
        }
    }

    /// <summary>
    /// Delete a specific schedule item
    /// </summary>
    [HttpDelete("{scheduleItemId}")]
    public async Task<ActionResult> DeleteScheduleItem(Guid scheduleItemId)
    {
        try
        {
            var userId = GetCurrentUserId();
            var success = await _scheduleService.DeleteScheduleItemAsync(userId, scheduleItemId);

            if (!success)
            {
                return NotFound(new { message = "Schedule item not found or does not belong to current user" });
            }

            return Ok(new { message = "Schedule item deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "An error occurred while deleting schedule item", 
                error = ex.Message 
            });
        }
    }

    /// <summary>
    /// Clear all schedule items for the current user
    /// </summary>
    [HttpDelete]
    public async Task<ActionResult> ClearWeeklySchedule()
    {
        try
        {
            var userId = GetCurrentUserId();
            var success = await _scheduleService.ClearWeeklyScheduleAsync(userId);

            if (!success)
            {
                return BadRequest(new { message = "Failed to clear weekly schedule" });
            }

            return Ok(new { message = "Weekly schedule cleared successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "An error occurred while clearing schedule", 
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
