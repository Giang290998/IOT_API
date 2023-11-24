
using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Services;

public interface IAdminService
{
    // Task<int> Register(RegisterViewModel registerViewModel);
    Task<Admin?> Login(LoginViewModel loginViewModel);
    // Task<bool> SendOTP();
    // Task<bool> ActiveUser(string userProvideOTP);
}
