using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Services;

public interface IProjectService
{
    Task<int> Create(CreateProjectViewModel createProjectViewModel);
    Task<bool> Update(UpdateProjectViewModel updateProjectViewModel);
    Task<ProjectCassandra?> GetProjectDetail(int project_id);
}
