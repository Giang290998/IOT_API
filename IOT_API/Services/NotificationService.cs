using IOT_API.Repositories;
using IOT_API.Models;
using IOT_API.ViewModels;

namespace IOT_API.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;

    public NotificationService(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Create(NotificationViewModel notificationViewModel)
    {
        return await _repository.Create(notificationViewModel);
    }

    public Task<List<NotificationUser>> GetAll()
    {
        return _repository.GetAll();
    }
}
