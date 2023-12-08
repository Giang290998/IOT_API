using System.Collections.Generic;
using System.Threading.Tasks;
using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Services;

public interface IUserService
{
    Task<int> Register(RegisterViewModel registerViewModel);
    Task<User?> Login(LoginViewModel loginViewModel);
    Task<bool> SendOTP();
    Task<bool> ActiveUser(string userProvideOTP);
    Task<User?> GetByPhone(string phone);
    Task<User?> GetById(int id);
    // Task<User> GetUserByUsernameAsync(string username);
    // Task<IEnumerable<User>> GetAllUsersAsync();
    // Task UpdateUserAsync(User user);
    // Task DeleteUserAsync(int id);
}
