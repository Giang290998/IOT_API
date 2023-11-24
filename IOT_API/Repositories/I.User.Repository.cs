using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Repositories;

public interface IUserRepository
{
    Task<int> Create(RegisterViewModel registerViewModel);
    Task<User?> Login(LoginViewModel loginViewModel);
    Task<bool> SendOTP();
    bool VerifyOTP(string otp);
    Task<bool> ActiveUser(string otp);
    Task<User?> GetByPhone(string phone);
    // Task<User?> GetByEmail(string email);
    // Task<User?> GetByUsername(string username);
    // Task<User> GetUserByUsernameAsync(string username);
    // Task<IEnumerable<User>> GetAllUsersAsync();
    // Task UpdateUserAsync(User user);
    // Task DeleteUserAsync(int id);
}
