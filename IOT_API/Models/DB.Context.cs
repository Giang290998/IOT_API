using Microsoft.EntityFrameworkCore;
using dotenv.net.Utilities;

namespace IOT_API.Models;

public class PostGISContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public PostGISContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    // public PostGISContext(DbContextOptions<PostGISContext> options) : base(options)
    // {

    // }

    public DbSet<User> Users { get; set; }
    public DbSet<UserDev> UsersDev { get; set; }
    public DbSet<NotificationUser> NotificationUsers { get; set; }
    public DbSet<Admin> Admins { get; set; }

    // Thêm các DbSet cho các bảng cơ sở dữ liệu khác nếu cần

    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);

    //     // Định nghĩa các chi tiết về quan hệ, chỉ định index, v.v.
    //     // Ví dụ: modelBuilder.Entity<IotUser>().HasIndex(u => u.Username).IsUnique();
    // }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // DotNetEnv.Env.Load();
        var host = EnvReader.GetStringValue("HOST");
        var port = EnvReader.GetStringValue("PORT");
        var database = EnvReader.GetStringValue("DATABASE");
        var username = EnvReader.GetStringValue("USERNAME");
        var password = EnvReader.GetStringValue("PASSWORD");
        string connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";
        options.UseNpgsql(connectionString);
    }
}
