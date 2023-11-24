using Microsoft.EntityFrameworkCore;
using IOT_API.Models;
using IOT_API.ViewModels;
using IOT_API.Extensions;

namespace IOT_API.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly PostGISContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NotificationRepository(PostGISContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<int> Create(NotificationViewModel notificationViewModel)
    {
        string noti_sheet_type = notificationViewModel.noti_sheet_type;
        int user_id = notificationViewModel.User_id;

        string query = $"INSERT INTO notifications (user_id, noti_sheet_type) VALUE ({user_id}, '{noti_sheet_type}');";

        var result = _context.Database.SqlQueryRaw<int>(query).ToList();

        return Task.FromResult(result.FirstOrDefault());
    }

    public Task<List<NotificationUser>> GetAll()
    {
        int user_id = HttpContextExtension.GetUserId(_httpContextAccessor.HttpContext);

        string query = $"SELECT * FROM notification_user WHERE user_id = {user_id} ORDER BY datetime DESC";

        IEnumerable<NotificationUser> result = _context.NotificationUsers.FromSqlRaw(query).ToList();

        List<NotificationUser> notification_list = result.ToList();

        return Task.FromResult(notification_list);
    }
}