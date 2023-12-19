using IOT_API.Models;
using IOT_API.Repositories;
using IOT_API.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using IOT_API.MQTT;
using IOT_API.Hubs;
using dotenv.net;
using IOT_API.Filters;


DotEnv.Load(options: new DotEnvOptions(ignoreExceptions: false));
var envVars = DotEnv.Read();
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// builder.Services.AddAuthentication("Bearer").AddJwtBearer();

builder.Services.AddControllers();

var host = envVars["HOST"];
var port = envVars["PORT"];
var database = envVars["DATABASE"];
var username = envVars["USERNAME"];
var password = envVars["PASSWORD"];
string connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";

builder.Services.AddDbContext<PostGISContext>(options =>
    options.UseNpgsql(connectionString)
);
builder.Services.AddSingleton(provider =>
    {
        var contactPoint = envVars["CASSANDRA_HOST"];
        var username = envVars["CASSANDRA_USERNAME"];
        var password = envVars["CASSANDRA_PASSWORD"];
        var keySpace = envVars["CASSANDRA_KEYSPACE"];

        return new CassandraContext(contactPoint, username, password, keySpace);
    });

//Add repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IMQTTRepository, MQTTRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

//Add  services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IMQTTService, MQTTService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(envVars["JWT_AT_KEY"] ?? ""))
        };
    }
);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins(envVars["IOT_CLIENT"] ?? "*");
            policy.AllowCredentials();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});
builder.Services.AddControllers(options =>
    {
        options.Filters.Add<HttpResponseExceptionFilter>();
    }
);

var contactPoint = envVars["CASSANDRA_HOST"];
var username_cass = envVars["CASSANDRA_USERNAME"];
var password_cass = envVars["CASSANDRA_PASSWORD"];
var keySpace = envVars["CASSANDRA_KEYSPACE"];
#pragma warning disable CS0612 // Type or member is obsolete
var mqtt = new MQTTManager(new MQTTRepository(new CassandraContext(contactPoint, username_cass, password_cass, keySpace)));
#pragma warning restore CS0612 // Type or member is obsolete

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

app.MapHub<DeviceDataHub>("/hub/device-data");

app.Run();
