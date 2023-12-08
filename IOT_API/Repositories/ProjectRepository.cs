using Microsoft.EntityFrameworkCore;
using IOT_API.Models;
using IOT_API.ViewModels;
using Microsoft.IdentityModel.Tokens;
using IOT_API.Extensions;

namespace IOT_API.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly PostGISContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProjectRepository(PostGISContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<int> Create(CreateProjectViewModel createProjectViewModel)
    {
        int own = createProjectViewModel.Own;
        string name = createProjectViewModel.Name ?? "";
        string[]? plant_list = createProjectViewModel.PlantList;
        string[]? device_list = createProjectViewModel.DeviceList;
        string? ip = createProjectViewModel.Ip;
        PhysicalAddress? mac = createProjectViewModel.Mac;

        // this function check exist first before insert, if exist return 0.
        string query = $"SELECT create_project('{own}', '{name}', '{plant_list}', '{name}', '{device_list}', '{ip}', '{mac}');";

        var result = _context.Database.SqlQueryRaw<int>(query).ToList();

        return Task.FromResult(result.FirstOrDefault());
    }

    public Task<bool> Authenticate(int project_id)
    {
        int user_id = HttpContextExtension.GetUserId(_httpContextAccessor.HttpContext);

        string query = $"SELECT EXISTS (SELECT 1 FROM projects WHERE id = {project_id} AND own = {user_id} AND is_delete = FALSE) AS result;";

        var result = _context.Database.SqlQueryRaw<bool>(query).ToList();

        if (result.FirstOrDefault())
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.

            _httpContextAccessor.HttpContext.Items["project_id"] = project_id;

#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        return Task.FromResult(result.FirstOrDefault());
    }

    public async Task<bool> UpdateOwn(int? user_id)
    {
        Console.WriteLine(user_id);
        return await Task.FromResult(true);
    }

    public async Task<bool> UpdateStaff(string? user_id, int? level)
    {
        int project_id = HttpContextExtension.GetProjectId(_httpContextAccessor.HttpContext);

        string staff_update = $"'{user_id}': {level}";

        string query = $"UPDATE projects SET staff = staff || '{"2": 1}'::jsonb WHERE id = {project_id};";
        Console.WriteLine(query);
        // _ = _context.Database.SqlQueryRaw<bool>(query).ToList();

        Console.WriteLine(user_id);
        Console.WriteLine(level);
        return await Task.FromResult(true);
    }

    public async Task<bool> UpdateName(string? name)
    {
        Console.WriteLine(name);
        return await Task.FromResult(true);
    }

    public async Task<bool> UpdatePlantList(string[]? plant_list)
    {
        Console.WriteLine(plant_list);
        return await Task.FromResult(true);
    }

    public async Task<bool> UpdateDeviceList(string[]? device_list)
    {
        Console.WriteLine(device_list);
        return await Task.FromResult(true);
    }

    public async Task<bool> UpdateIP(string? IP)
    {
        Console.WriteLine(IP);
        return await Task.FromResult(true);
    }

    public async Task<bool> UpdateMAC(string? MAC)
    {
        Console.WriteLine(MAC);
        return await Task.FromResult(true);
    }
    // public async Task<User?> Login(LoginViewModel loginViewModel)
    // {
    //     string phone = loginViewModel.Phone;
    //     string password = loginViewModel.Password;

    //     IEnumerable<User> result = await _context.Users.FromSqlRaw($"SELECT * FROM users WHERE phone = '{phone}' LIMIT 1;").ToListAsync();

    //     User? user = result.FirstOrDefault();

    //     if (user == null || !BCRYPT.Verify(password, user.Password)) return null;

    //     return user;
    // }

    // public async Task<bool> SendOTP()
    // {
    //     int user_id = HttpContextExtension.GetUserId(_httpContextAccessor.HttpContext);

    //     Random random = new();

    //     string otp = random.Next(100000, 999999).ToString();

    //     string query = $"UPDATE users_dev SET otp = '{otp}', otp_expire = CURRENT_TIMESTAMP + INTERVAL '50 seconds' WHERE user_id = {user_id};";

    //     await _context.Database.SqlQueryRaw<int>(query).ToListAsync();

    //     IEnumerable<User> result = await _context.Users.FromSqlRaw($"SELECT * FROM users WHERE id = {user_id} LIMIT 1;").ToListAsync();

    //     User? user = result.FirstOrDefault();

    //     string message_content = $"This is your SmartAgri OTP code: {otp}";

    //     if (user != null)
    //     {
    //         TWILIO.SendSMS(user.Phone, message_content);
    //         return true;
    //     }
    //     return false;
    // }

    // public bool VerifyOTP(string user_provide_otp)
    // {
    //     int user_id = HttpContextExtension.GetUserId(_httpContextAccessor.HttpContext);

    //     var result = _context.UsersDev.FromSqlRaw($"SELECT * FROM users_dev WHERE user_id = {user_id} LIMIT 1;").ToList();

    //     UserDev? u_dev = result.FirstOrDefault();

    //     if (u_dev == null || u_dev.Otp == null) return false;

    //     bool is_valid = u_dev.Otp_expire > DateTime.UtcNow && user_provide_otp == u_dev.Otp;

    //     if (is_valid)
    //     {
    //         _context.UsersDev.FromSqlRaw($"UPDATE users_dev SET otp = NULL WHERE user_id = {user_id};");
    //         return true;
    //     }

    //     return false;
    // }

    // private void SetActiveUserIsTrue(int user_id)
    // {
    //     try
    //     {
    //         string query = $"UPDATE users SET is_active = TRUE WHERE id = {user_id};";
    //         _ = _context.Database.SqlQueryRaw<int>(query).ToList();
    //     }
    //     catch (Exception)
    //     {
    //         throw new Exception();
    //     }
    // }

    // private bool UserActiveIsFalse(int user_id)
    // {
    //     IEnumerable<User> result = _context.Users.FromSqlRaw($"SELECT * FROM users WHERE id = {user_id} AND is_active = FALSE LIMIT 1;").ToList();

    //     User? user = result.FirstOrDefault();
    //     if (user != null) return true;
    //     return false;
    // }

    // public Task<bool> ActiveUser(string user_provide_otp)
    // {
    //     int user_id = HttpContextExtension.GetUserId(_httpContextAccessor.HttpContext);

    //     bool is_valid_otp = VerifyOTP(user_provide_otp);

    //     bool is_active_false = UserActiveIsFalse(user_id);

    //     if (is_valid_otp && is_active_false)
    //     {
    //         SetActiveUserIsTrue(user_id);
    //         return Task.FromResult(true);
    //     }
    //     return Task.FromResult(false);
    // }

    // public async Task<User?> GetByPhone(string phone)
    // {
    //     IEnumerable<User> result = await _context.Users.FromSqlRaw($"SELECT * FROM users WHERE phone = '{phone}' LIMIT 1;").ToListAsync();

    //     User? user = result.FirstOrDefault();

    //     if (user != null) return user;

    //     return null;
    // }

    // public async Task<User?> GetByEmail(string email)
    // {
    //     return await _context.Users.FindAsync(email);
    // }

    // public async Task<User?> GetByUsername(string username)
    // {
    //     return await _context.Users.FindAsync(username);
    // }
}
