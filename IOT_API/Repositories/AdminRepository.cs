using Microsoft.EntityFrameworkCore;
using IOT_API.Models;
using IOT_API.ViewModels;
using BCRYPT = BCrypt.Net.BCrypt;

namespace IOT_API.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly PostGISContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AdminRepository(PostGISContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Admin?> Login(LoginViewModel loginViewModel)
    {
        string phone = loginViewModel.Phone;
        string password = loginViewModel.Password;

        string query = $"SELECT * FROM admins WHERE phone = '{phone}' LIMIT 1;";

        IEnumerable<Admin> result = await _context.Admins.FromSqlRaw(query).ToListAsync();

        Admin? admin = result.FirstOrDefault();

        if (admin == null || !BCRYPT.Verify(password, admin.Password)) return null;

        return admin;
    }

    // public Task<List<NotificationUser>> GetAll()
    // {
    //     int user_id = HttpContextExtension.GetUserId(_httpContextAccessor.HttpContext);

    //     string query = $"SELECT * FROM notification_user WHERE user_id = {user_id} ORDER BY datetime DESC";

    //     IEnumerable<NotificationUser> result = _context.NotificationUsers.FromSqlRaw(query).ToList();

    //     List<NotificationUser> notification_list = result.ToList();

    //     return Task.FromResult(notification_list);
    // }
}