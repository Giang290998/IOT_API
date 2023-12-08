using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Repositories;

public interface IProjectRepository
{
    Task<int> Create(CreateProjectViewModel createProjectViewModel);
    Task<bool> Authenticate(int project_id);
    Task<bool> UpdateOwn(int? user_id);

    Task<bool> UpdateStaff(string? user_id_staff, int? level);

    Task<bool> UpdateName(string? name);

    Task<bool> UpdatePlantList(string[]? plant_list);

    Task<bool> UpdateDeviceList(string[]? device_list);

    Task<bool> UpdateIP(string? IP);

    Task<bool> UpdateMAC(string? MAC);
}
