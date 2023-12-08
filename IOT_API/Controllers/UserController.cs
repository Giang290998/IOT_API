using Microsoft.AspNetCore.Mvc;
using IOT_API.Services;
using IOT_API.ViewModels;
using IOT_API.Helper;
using IOT_API.Attributes;
using IOT_API.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace IOT_API.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    // private readonly AuthorizationFilterContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserController(IUserService service,
    // , AuthorizationFilterContext context
    IHttpContextAccessor httpContextAccessor
    )
    {
        _service = service;
        // _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
    {
        if (registerViewModel == null) return BadRequest();

        try
        {
            var new_user_id = await _service.Register(registerViewModel);
            if (new_user_id == 0) return BadRequest(new { Message = "User exist." });
            return Ok(new { user_id = new_user_id });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }


    [HttpGet]
    [Route("login")]
    public async Task<IActionResult> Login([FromQuery] LoginViewModel loginViewModel)
    {
        if (loginViewModel == null) return BadRequest();

        try
        {
            var user = await _service.Login(loginViewModel);
            if (user != null)
            {
                string access_token = JWT.CreateAccessToken(user.Id.ToString(), "USER", "1");
                string refresh_token = JWT.CreateRefreshToken(user.Id.ToString(), "USER", "1");
                string persistent_token = JWT.CreatePersistentToken(user.Id.ToString(), "USER", "1");

                // var cookieOptions = new CookieOptions
                // {
                //     Expires = DateTime.Now.AddDays(30),
                //     HttpOnly = true
                // };

                // Response.Cookies.Append("persistent_token", persistent_token, cookieOptions);
                // Response.Cookies.Append("refresh_token", refresh_token, cookieOptions);
                // Response.Cookies.Append("access_token", access_token, cookieOptions);
                return Ok(new { access_token, refresh_token, persistent_token, user = user.Return() });
            }
            return BadRequest(new { Message = "Wrong phone or password." });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpGet]
    [Route("login/persistent")]
    public async Task<IActionResult> LoginPersistent()
    {
        if (HttpContext.Request.Headers.TryGetValue("persistent_token", out var bearerToken))
        {
            if (bearerToken.IsNullOrEmpty()) return BadRequest();

            string token = bearerToken.FirstOrDefault()?.Split(" ")?.Last() ?? "";
            bool is_valid = JWT.ValidatePersistentToken(token);

            if (!is_valid) return Unauthorized();

            string id = JWT.GetIdFromPayloadToken(token);
            string role = JWT.GetRoleFromPayloadToken(token);

            if (role == "USER")
            {
                User? user = await _service.GetById(int.Parse(id));

                if (user == null) return BadRequest();

                string new_access_token = JWT.CreateAccessToken(user.Id.ToString(), "USER", "1");
                string new_refresh_token = JWT.CreateRefreshToken(user.Id.ToString(), "USER", "1");
                string new_persistent_token = JWT.CreatePersistentToken(user.Id.ToString(), "USER", "1");

                return Ok(new { new_access_token, new_refresh_token, new_persistent_token, user = user.Return() });
            }

            if (role == "ADMIN")
            {

            }

            return BadRequest();
        }

        return BadRequest("Persistent Token not found in the request header.");
    }


    [HttpGet]
    [Route("otp/send")]
    [JwtAuthorize]
    public async Task<IActionResult> SendOTP()
    {
        if (_httpContextAccessor.HttpContext == null) return Unauthorized();

        try
        {
            bool is_send = await _service.SendOTP();
            if (is_send) return Ok();
            return BadRequest();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpGet]
    [Route("active")]
    [JwtAuthorize]
    public async Task<IActionResult> ActiveUser([FromQuery(Name = "otp")] string user_provide_otp)
    {
        if (_httpContextAccessor.HttpContext == null) return Unauthorized();

        if (user_provide_otp == null) return BadRequest(new { Message = "Empty OTP" });

        try
        {
            bool isValid = await _service.ActiveUser(user_provide_otp);
            if (isValid) return Ok();
            return BadRequest();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpGet]
    [Route("get")]
    [JwtAuthorize]
    public async Task<IActionResult> GetByPhone([FromBody] string phone)
    {
        if (_httpContextAccessor.HttpContext == null) return Unauthorized();

        try
        {
            User? user = await _service.GetByPhone(phone);
            if (user != null) return Ok(user.Return());
            return BadRequest();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }
}
