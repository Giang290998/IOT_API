using IOT_API.Repositories;
using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Services;

public class DeviceDataService : IDeviceDataService
{
    private readonly IDeviceDataRepository _repository;

    public DeviceDataService(IDeviceDataRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Create(DeviceDataViewModel notificationViewModel)
    {
        return await _repository.Create(notificationViewModel);
    }

    public Task<List<DeviceData>> GetAll()
    {
        return _repository.GetAll();
    }
}
