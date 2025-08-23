using Microsoft.AspNetCore.Mvc;

namespace Burjo.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult HandleResult<T>(T result)
    {
        if (result == null)
        {
            return NotFound();
        }
        
        return Ok(result);
    }

    protected IActionResult HandleResult<T>(bool success, T result, string? errorMessage = null)
    {
        if (success)
        {
            return Ok(result);
        }

        return BadRequest(errorMessage ?? "An error occurred");
    }
}
