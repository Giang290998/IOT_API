using Microsoft.AspNetCore.Mvc;
using IOT_API.Services;
using IOT_API.ViewModels;
using IOT_API.Helper;
using IOT_API.Attributes;

namespace IOT_API.Controllers;

[Route("api/admin")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IProjectService _projectService;

    public AdminController(
        IAdminService service,
        IHttpContextAccessor httpContextAccessor,
        IProjectService projectService
    )
    {
        _adminService = service;
        _httpContextAccessor = httpContextAccessor;
        _projectService = projectService;
    }

    // [HttpPost]
    // [Route("register")]
    // public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
    // {
    //     if (registerViewModel == null) return BadRequest();

    //     try
    //     {
    //         var new_user_id = await _adminService.Register(registerViewModel);
    //         if (new_user_id == 0) return BadRequest(new { Message = "User exist." });
    //         return Ok(new { user_id = new_user_id });
    //     }
    //     catch (ArgumentException ex)
    //     {
    //         return BadRequest(new { ex.Message });
    //     }
    // }


    [HttpGet]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
    {
        if (loginViewModel == null) return BadRequest();

        try
        {
            var admin = await _adminService.Login(loginViewModel);
            if (admin != null)
            {
                string access_token = JWT.CreateAccessToken(admin.Id.ToString(), "ADMIN", admin.Role.ToString());
                string refresh_token = JWT.CreateRefreshToken(admin.Id.ToString(), "ADMIN", admin.Role.ToString());
                string persistent_token = JWT.CreatePersistentToken(admin.Id.ToString(), "ADMIN", admin.Role.ToString());

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true
                };

                Response.Cookies.Append("persistent_token", persistent_token, cookieOptions);
                Response.Cookies.Append("refresh_token", refresh_token, cookieOptions);
                Response.Cookies.Append("access_token", access_token, cookieOptions);
                return Ok(admin);
            }
            return BadRequest(new { Message = "Wrong phone or password." });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }


    [HttpPost]
    [Route("project/create")]
    [JwtAuthorize]
    public async Task<IActionResult> CreateProject(CreateProjectViewModel createProjectViewModel)
    {
        if (_httpContextAccessor.HttpContext == null) return Unauthorized();

        try
        {
            int project_id = await _projectService.Create(createProjectViewModel);
            if (project_id != 0) return Ok(project_id);
            return BadRequest();
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
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
