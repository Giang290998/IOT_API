using Microsoft.EntityFrameworkCore;
using IOT_API.Models;
using IOT_API.ViewModels;
using IOT_API.Helper;
using BCRYPT = BCrypt.Net.BCrypt;
using IOT_API.Extensions;

namespace IOT_API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly PostGISContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserRepository(PostGISContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<int> Create(RegisterViewModel registerViewModel)
    {
        string username = registerViewModel.Username.Length > 0 ? registerViewModel.Username : registerViewModel.Phone;
        string password = registerViewModel.Password;
        string phone = registerViewModel.Phone;
        string name = registerViewModel.Name ?? "";
        string email = registerViewModel.Email ?? "";
        string address = registerViewModel.Address ?? "";

        // this function check exist first before insert, if exist return 0.
        string query = $"SELECT create_user('{username}', '{password}', '{phone}', '{name}', '{email}', '{address}');";

        var result = _context.Database.SqlQueryRaw<int>(query).ToList();

        return Task.FromResult(result.FirstOrDefault());
    }

    public async Task<User?> Login(LoginViewModel loginViewModel)
    {
        string phone = loginViewModel.Phone;
        string password = loginViewModel.Password;

        IEnumerable<User> result = await _context.Users.FromSqlRaw($"SELECT * FROM users WHERE phone = '{phone}' LIMIT 1;").ToListAsync();

        User? user = result.FirstOrDefault();

        if (user == null || !BCRYPT.Verify(password, user.Password)) return null;

        return user;
    }

    public async Task<bool> SendOTP()
    {
        int user_id = HttpContextExtension.GetUserId(_httpContextAccessor.HttpContext);

        string otp = Generate.OTP_6();

        string query = $"UPDATE users_dev SET otp = '{otp}', otp_expire = CURRENT_TIMESTAMP + INTERVAL '50 seconds' WHERE user_id = {user_id};";

        await _context.Database.SqlQueryRaw<int>(query).ToListAsync();

        IEnumerable<User> result = await _context.Users.FromSqlRaw($"SELECT * FROM users WHERE id = {user_id} LIMIT 1;").ToListAsync();

        User? user = result.FirstOrDefault();

        string message_content = $"This is your SmartAgri OTP code: {otp}";

        if (user != null)
        {
            TWILIO.SendSMS(user.Phone, message_content);
            return true;
        }
        return false;
    }

    public bool VerifyOTP(string user_provide_otp)
    {
        int user_id = HttpContextExtension.GetUserId(_httpContextAccessor.HttpContext);

        var result = _context.UsersDev.FromSqlRaw($"SELECT * FROM users_dev WHERE user_id = {user_id} LIMIT 1;").ToList();

        UserDev? u_dev = result.FirstOrDefault();

        if (u_dev == null || u_dev.Otp == null) return false;

        bool is_valid = u_dev.Otp_expire > DateTime.UtcNow && user_provide_otp == u_dev.Otp;

        if (is_valid)
        {
            _context.UsersDev.FromSqlRaw($"UPDATE users_dev SET otp = NULL WHERE user_id = {user_id};");
            return true;
        }

        return false;
    }

    private void SetActiveUserIsTrue(int user_id)
    {
        try
        {
            string query = $"UPDATE users SET is_active = TRUE WHERE id = {user_id};";
            _ = _context.Database.SqlQueryRaw<int>(query).ToList();
        }
        catch (Exception)
        {
            throw new Exception();
        }
    }

    private bool UserActiveIsFalse(int user_id)
    {
        IEnumerable<User> result = _context.Users.FromSqlRaw($"SELECT * FROM users WHERE id = {user_id} AND is_active = FALSE LIMIT 1;").ToList();

        User? user = result.FirstOrDefault();
        if (user != null) return true;
        return false;
    }

    public Task<bool> ActiveUser(string user_provide_otp)
    {
        int user_id = HttpContextExtension.GetUserId(_httpContextAccessor.HttpContext);

        bool is_valid_otp = VerifyOTP(user_provide_otp);

        bool is_active_false = UserActiveIsFalse(user_id);

        if (is_valid_otp && is_active_false)
        {
            SetActiveUserIsTrue(user_id);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }

    public async Task<User?> GetByPhone(string phone)
    {
        IEnumerable<User> result = await _context.Users.FromSqlRaw($"SELECT * FROM users WHERE phone = '{phone}' LIMIT 1;").ToListAsync();

        User? user = result.FirstOrDefault();

        if (user != null) return user;

        return null;
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await _context.Users.FindAsync(email);
    }

    public async Task<User?> GetByUsername(string username)
    {
        return await _context.Users.FindAsync(username);
    }


    // public async Task<User> GetUserByIdAsync(int id)
    // {
    //     return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    // }

    // public async Task<User> GetUserByUsernameAsync(string username)
    // {
    //     return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    // }

    // public async Task<IEnumerable<User>> GetAllUsersAsync()
    // {
    //     return await _context.Users.ToListAsync();
    // }

    // public async Task UpdateUserAsync(User user)
    // {
    //     _context.Users.Update(user);
    //     await _context.SaveChangesAsync();
    // }

    // public async Task DeleteUserAsync(int id)
    // {
    //     var user = await GetUserByIdAsync(id);
    //     if (user != null)
    //     {
    //         _context.Users.Remove(user);
    //         await _context.SaveChangesAsync();
    //     }
    // }
}
