using Microsoft.AspNetCore.Mvc;
using IOT_API.Services;
using IOT_API.ViewModels;
using IOT_API.Helper;
using IOT_API.Attributes;

namespace IOT_API.Controllers;

[Route("api/project")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProjectController(
        IHttpContextAccessor httpContextAccessor,
        IProjectService projectService
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _projectService = projectService;
    }

    [HttpPut]
    [Route("update")]
    [JwtAuthorize]
    public async Task<IActionResult> Update(UpdateProjectViewModel updateProjectViewModel)
    {
        if (_httpContextAccessor.HttpContext == null) return Unauthorized();

        try
        {
            bool is_success = await _projectService.Update(updateProjectViewModel);
            if (is_success) return Ok();
            return BadRequest();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // [HttpGet]
    // [Route("otp/send")]
    // [JwtAuthorize]
    // public async Task<IActionResult> SendOTP()
    // {
    //     if (_httpContextAccessor.HttpContext == null) return Unauthorized();

    //     try
    //     {
    //         bool is_send = await _service.SendOTP();
    //         if (is_send) return Ok();
    //         return BadRequest();
    //     }
    //     catch (ArgumentException ex)
    //     {
    //         return BadRequest(new { ex.Message });
    //     }
    // }

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
