using IOT_API.Repositories;
using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Register(RegisterViewModel registerViewModel)
    {
        return await _repository.Create(registerViewModel);
    }

    public async Task<User?> Login(LoginViewModel loginViewModel)
    {
        return await _repository.Login(loginViewModel);
    }

    public async Task<bool> SendOTP()
    {
        return await _repository.SendOTP();
    }

    public Task<bool> ActiveUser(string userProvideOTP)
    {
        return _repository.ActiveUser(userProvideOTP);
    }

    public Task<User?> GetByPhone(string phone)
    {
        return _repository.GetByPhone(phone);
    }

    // Các phương thức khác tương tự như trong IUserRepository
}
