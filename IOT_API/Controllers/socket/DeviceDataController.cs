using Microsoft.AspNetCore.Mvc;
using IOT_API.Services;
using IOT_API.Models;
using IOT_API.ViewModels;
using IOT_API.Attributes;

namespace IOT_API.Controllers;

[Route("api/device-data")]
[ApiController]
public class DeviceDataController : ControllerBase
{
    private readonly IDeviceDataService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeviceDataController(IDeviceDataService service, IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] DeviceDataViewModel notificationViewModel)
    {
        if (notificationViewModel == null) return BadRequest();

        try
        {
            bool is_create = await _service.Create(notificationViewModel);
            if (is_create == false) return BadRequest(new { Message = "Can not create notification." });
            return Ok(new { is_create });
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAll()
    {
        // if (_httpContextAccessor.HttpContext == null) return Unauthorized();

        try
        {
            List<DeviceData> device_data = await _service.GetAll();
            return Ok(new { device_data });
        }
        catch (Exception)
        {
            return BadRequest();
        }
    }
}