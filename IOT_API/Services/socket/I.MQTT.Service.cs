using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Services;

public interface IMQTTService
{
    Task CreateSensorDataInterval(DeviceDataViewModel deviceDataViewModel);
    Task<List<DeviceData>> GetAllDeviceData(int project_id);
    Task CreateSensorIssue(Alarm alarm);
    Task<List<Alarm>> GetAllAlarm(int project_id);
}
