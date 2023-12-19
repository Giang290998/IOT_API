using IOT_API.Repositories;
using IOT_API.Models;
using IOT_API.ViewModels;
using IOT_API.Filters;
using System.Net;

namespace IOT_API.Services;

public class MQTTService : IMQTTService
{
    private readonly IMQTTRepository _repository;
    private readonly IProjectRepository _projectRepository;

    public MQTTService(IMQTTRepository repository, IProjectRepository projectRepository)
    {
        _repository = repository;
        _projectRepository = projectRepository;
    }

    public async Task CreateSensorDataInterval(DeviceDataViewModel notificationViewModel)
    {
        await _repository.CreateSensorDataInterval(notificationViewModel);
    }

    public async Task CreateSensorIssue(Alarm alarm)
    {
        await _repository.CreateSensorIssue(alarm);
    }

    public async Task<List<Alarm>> GetAllAlarm(int project_id)
    {
        try
        {
            bool is_authenticate = await _projectRepository.Authenticate(project_id);

            if (!is_authenticate) throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return await _repository.GetAllAlarm(project_id);
        }
        catch (Exception)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }
    }

    public async Task<List<DeviceData>> GetAllDeviceData(int project_id)
    {
        try
        {
            bool is_authenticate = await _projectRepository.Authenticate(project_id);

            if (!is_authenticate) throw new HttpResponseException(HttpStatusCode.Unauthorized);

            return await _repository.GetAllDeviceData(project_id);
        }
        catch (Exception)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }
    }
}
