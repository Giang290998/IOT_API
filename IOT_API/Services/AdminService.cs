using IOT_API.Repositories;
using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Services;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _repository;

    public AdminService(IAdminRepository repository)
    {
        _repository = repository;
    }

    // public async Task<int> Register(RegisterViewModel registerViewModel)
    // {
    //     return await _repository.Create(registerViewModel);
    // }

    public async Task<Admin?> Login(LoginViewModel loginViewModel)
    {
        return await _repository.Login(loginViewModel);
    }

}
