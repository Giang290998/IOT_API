using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Services;

public interface IDeviceDataService
{
    Task<bool> Create(DeviceDataViewModel deviceDataViewModel);
    Task<List<DeviceData>> GetAll();
}
