using Microsoft.AspNetCore.Mvc;
using IOT_API.Services;
using IOT_API.ViewModels;
using IOT_API.Attributes;
using IOT_API.Models;

namespace IOT_API.Controllers;

[Route("api/project")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMQTTService _mqttService;

    public ProjectController(
        IHttpContextAccessor httpContextAccessor,
        IProjectService projectService,
        IMQTTService mqttService
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _projectService = projectService;
        _mqttService = mqttService;
    }

    [HttpPut]
    [Route("update")]
    [JwtAuthorize]
    public async Task<IActionResult> Update(UpdateProjectViewModel updateProjectViewModel)
    {
        if (updateProjectViewModel is null) return BadRequest();

        try
        {
            bool is_success = await _projectService.Update(updateProjectViewModel);
            if (is_success) return Ok();
            return BadRequest();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    [Route("device-data")]
    [JwtAuthorize]
    public async Task<IActionResult> GetAllDeviceData([FromQuery] string project_id)
    {
        if (!int.TryParse(project_id, out int p_id)) return BadRequest();

        try
        {
            List<DeviceData>? device_data = await _mqttService.GetAllDeviceData(p_id);

            return Ok(device_data);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpGet]
    [Route("alarm")]
    [JwtAuthorize]
    public async Task<IActionResult> GetAllAlarm([FromQuery] string project_id)
    {
        if (!int.TryParse(project_id, out int p_id)) return BadRequest();

        try
        {
            List<Alarm>? alarm = await _mqttService.GetAllAlarm(p_id);

            return Ok(alarm);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    [Route("detail")]
    [JwtAuthorize]
    public async Task<IActionResult> GetProjectDetail([FromQuery] string project_id)
    {
        if (!int.TryParse(project_id, out int p_id)) return BadRequest();

        try
        {
            ProjectCassandra? detail = await _projectService.GetProjectDetail(p_id);

            return Ok(detail);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    // [HttpGet]
    // [Route("/active")]
    // [JwtAuthorize]
    // public async Task<IActionResult> ActiveUser([FromQuery(Name = "otp")] string user_provide_otp)
    // {
    //     if (_httpContextAccessor.HttpContext == null) return Unauthorized();

    //     if (user_provide_otp == null) return BadRequest();

    //     try
    //     {
    //         bool isValid = await _service.ActiveUser(user_provide_otp);
    //         if (isValid) return Ok();
    //         return BadRequest();
    //     }
    //     catch (ArgumentException ex)
    //     {
    //         return BadRequest(new { ex.Message });
    //     }
    // }
}
