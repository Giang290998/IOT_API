using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Services;

public interface INotificationService
{
    Task<int> Create(NotificationViewModel notificationViewModel);
    Task<List<NotificationUser>> GetAll();
}
