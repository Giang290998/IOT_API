using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Repositories;

public interface IDeviceDataRepository
{
    Task<bool> Create(DeviceDataViewModel deviceDataViewModel);
    Task<List<DeviceData>> GetAll();
}
