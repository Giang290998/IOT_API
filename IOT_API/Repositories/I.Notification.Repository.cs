using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Repositories;

public interface INotificationRepository
{
    Task<int> Create(NotificationViewModel notificationViewModel);
    Task<List<NotificationUser>> GetAll();
}
