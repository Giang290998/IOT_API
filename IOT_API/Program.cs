using IOT_API.Models;
using IOT_API.Repositories;
using IOT_API.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IOT_API.Hubs;
using MQTTnet;


DotNetEnv.Env.Load();
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// builder.Services.AddAuthentication("Bearer").AddJwtBearer();

builder.Services.AddControllers();

var host = Environment.GetEnvironmentVariable("HOST");
var port = Environment.GetEnvironmentVariable("PORT");
var database = Environment.GetEnvironmentVariable("DATABASE");
var username = Environment.GetEnvironmentVariable("USERNAME");
var password = Environment.GetEnvironmentVariable("PASSWORD");
string connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";

builder.Services.AddDbContext<PostGISContext>(options =>
    options.UseNpgsql(connectionString)
);
builder.Services.AddSingleton(provider =>
    {
        var contactPoint = Environment.GetEnvironmentVariable("CASSANDRA_HOST") ?? "";
        var username = Environment.GetEnvironmentVariable("CASSANDRA_USERNAME") ?? "";
        var password = Environment.GetEnvironmentVariable("CASSANDRA_PASSWORD") ?? "";
        var keySpace = Environment.GetEnvironmentVariable("CASSANDRA_KEYSPACE") ?? "";

        return new CassandraContext(contactPoint, username, password, keySpace);
    });

//Add repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IDeviceDataRepository, DeviceDataRepository>();

//Add  services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IDeviceDataService, DeviceDataService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(
    options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "your_issuer",
            ValidAudience = "your_audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_AT_KEY") ?? ""))
        };
    }
);
builder.Services.AddHttpContextAccessor();
// builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins(Environment.GetEnvironmentVariable("IOT_CLIENT") ?? "*");
        });
});

// builder.Services.AddSingleton<MqttServerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

// app.MapHub<DeviceDataHub>("/hub/device-data");

app.Run();
