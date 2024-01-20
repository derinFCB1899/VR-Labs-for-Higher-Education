using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using VR_Labs_for_Higher_Education.Services;


namespace VR_Labs_for_Higher_Education.Controllers;

[Authorize(Roles = "Student")]
[Route("api/[controller]")]
[ApiController]
public class LabController : ControllerBase
{
    private readonly LabService _labService;
    private readonly ILogger<LabController> _logger;

    public LabController(LabService labService, ILogger<LabController> logger)
    {
        _labService = labService;
        _logger = logger;
    }

    // The method that communicates with the Unity lab to fetch
    [HttpPost("ToggleAction")]
    public async Task<IActionResult> ToggleAction([FromBody] ToggleActionModel toggle)
    {
        try
        {
            var toggleIndex = toggle.Action; // This should match the index of the toggle, like "wearGloves"

            var fullName = User.FindFirst(c => c.Type == "name")?.Value;
            await _labService.UpdateCheckpointTimestamp(fullName, "titrationLab", toggleIndex, toggle.Timestamp);
            _logger.LogInformation(fullName);
            // Log the action
            _logger.LogInformation($"Action: {toggle.Action}, Timestamp: {toggle.Timestamp}");

            // Acknowledge the receipt of the data
            return Ok();
        }
        catch (Exception ex)
        {
            // Log the error
            _logger.LogError(ex, "Error updating toggle action timestamp");
            return StatusCode(500, "Internal server error");
        }
    }
}