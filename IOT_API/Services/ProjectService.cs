using IOT_API.Repositories;
using IOT_API.Models;
using IOT_API.ViewModels;
using Microsoft.IdentityModel.Tokens;
using IOT_API.Filters;
using System.Net;

namespace IOT_API.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;

    public ProjectService(IProjectRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Create(CreateProjectViewModel createProjectViewModel)
    {
        return await _repository.Create(createProjectViewModel);
    }

    public async Task<ProjectCassandra?> GetProjectDetail(int project_id)
    {
        bool is_authenticate = await _repository.Authenticate(project_id);

        if (!is_authenticate) throw new HttpResponseException(HttpStatusCode.Unauthorized);

        return await _repository.GetProjectDetail(project_id);
    }

    public async Task<bool> Update(UpdateProjectViewModel updateProjectViewModel)
    {
        int project_id = updateProjectViewModel.ProjectId;
        int own = int.Parse(updateProjectViewModel.Own ?? "0");
        Dictionary<string, int>? staff = updateProjectViewModel.Staff;
        string? Name = updateProjectViewModel.Name;
        string[]? PlantList = updateProjectViewModel.PlantList;
        string[]? DeviceList = updateProjectViewModel.DeviceList;
        string? IP = updateProjectViewModel.Ip;
        string? MAC = updateProjectViewModel.Mac;

        try
        {

            bool is_authenticate = await _repository.Authenticate(project_id);

            if (!is_authenticate) throw new HttpResponseException(HttpStatusCode.Unauthorized);

            if (own != 0) return await _repository.UpdateOwn(own);
            if (staff != null && staff.Any()) return await _repository.UpdateStaff("2", 1);
            if (!Name.IsNullOrEmpty()) return await _repository.UpdateName(Name);
            if (!PlantList.IsNullOrEmpty()) return await _repository.UpdatePlantList(PlantList);
            if (!DeviceList.IsNullOrEmpty()) return await _repository.UpdateDeviceList(DeviceList);
            if (!IP.IsNullOrEmpty()) return await _repository.UpdateIP(IP);
            if (!MAC.IsNullOrEmpty()) return await _repository.UpdateMAC(MAC);

            return await Task.FromResult(false);
        }
        catch (Exception)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }
    }
}
