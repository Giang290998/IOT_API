// using Microsoft.AspNetCore.Mvc;
// using IOT_API.Services;
// using IOT_API.Models;
// using IOT_API.ViewModels;

// namespace IOT_API.Controllers;

// public class MQTTController 
// {
//     private readonly IDeviceDataService _service;
//     private readonly IHttpContextAccessor _httpContextAccessor;

//     public MQTTController(IDeviceDataService service, IHttpContextAccessor httpContextAccessor)
//     {
//         _service = service;
//         _httpContextAccessor = httpContextAccessor;
//     }

//     public async Task<IActionResult> CreateSensorIssue(string issue)
//     {
//         try
//         {
//             bool is_create = await _service.Create(notificationViewModel);
//             if (is_create == false) return BadRequest(new { Message = "Can not create notification." });
//             return Ok(new { is_create });
//         }
//         catch (Exception)
//         {
//             return BadRequest();
//         }
//     }

//     public async Task<IActionResult> GetAll()
//     {
//         // if (_httpContextAccessor.HttpContext == null) return Unauthorized();

//         try
//         {
//             List<DeviceData> device_data = await _service.GetAll();
//             return Ok(new { device_data });
//         }
//         catch (Exception)
//         {
//             return BadRequest();
//         }
//     }
// }