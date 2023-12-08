using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Repositories;

public interface IAlarmRepository
{
    Task<bool> CreateSensorIssue(Alarm alarm_data);
    // Task CreateSensorDataInterval(DeviceData device_data);
    // Task<List<DeviceData>> GetAll();
}
