using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class LabController : ControllerBase
{
    // Assume you have some data access layer or service to interact with the database
    // For simplicity, let's just log the received data
    private readonly ILogger<LabController> _logger;

    public LabController(ILogger<LabController> logger)
    {
        _logger = logger;
    }

    [HttpPost("ToggleAction")]
    public IActionResult ToggleAction([FromBody] ToggleActionModel toggle)
    {
        // Log the received toggle action and timestamp
        _logger.LogInformation($"Action: {toggle.Action}, Timestamp: {toggle.Timestamp}");

        // Here you would normally save this information to the database
        // For now, just return OK to acknowledge receipt of the data
        return Ok();
    }
}