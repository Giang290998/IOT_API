using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Repositories;

public interface IMQTTRepository
{
    Task CreateSensorIssue(Alarm alarm);
    Task CreateSensorDataInterval(DeviceDataViewModel deviceDataViewModel);
    Task<List<DeviceData>> GetAllDeviceData(int project_id);
    Task<List<Alarm>> GetAllAlarm(int project_id);
}
