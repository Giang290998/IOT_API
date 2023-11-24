using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Repositories;

public interface IAdminRepository
{
    Task<Admin?> Login(LoginViewModel loginViewModel);
    // Task<List<NotificationUser>> GetAll();
}
