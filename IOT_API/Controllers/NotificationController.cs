using Microsoft.AspNetCore.Mvc;
using IOT_API.Services;
using IOT_API.Models;
using IOT_API.ViewModels;
using IOT_API.Attributes;

namespace IOT_API.Controllers;

[Route("api/notification")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly INotificationService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NotificationController(INotificationService service, IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] NotificationViewModel notificationViewModel)
    {
        if (notificationViewModel == null) return BadRequest();

        try
        {
            var new_notification_id = await _service.Create(notificationViewModel);
            if (new_notification_id == 0) return BadRequest(new { Message = "Can not create notification." });
            return Ok(new { new_notification_id });
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("all")]
    [JwtAuthorize]
    public async Task<IActionResult> GetAll()
    {
        if (_httpContextAccessor.HttpContext == null) return Unauthorized();

        try
        {
            List<NotificationUser> notifications = await _service.GetAll();
            return Ok(new { notifications });
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}