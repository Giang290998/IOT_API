using IOT_API.Models;
using ISession = Cassandra.ISession;
using IOT_API.ViewModels;
using IOT_API.Helper;
using Cassandra.Mapping;

namespace IOT_API.Repositories;

public class DeviceDataRepository : IDeviceDataRepository
{
    private readonly CassandraContext _cassandraContext;

    public DeviceDataRepository(CassandraContext cassandraContext)
    {
        _cassandraContext = cassandraContext;
    }

    public Task<bool> Create(DeviceDataViewModel deviceDataViewModel)
    {
        if (deviceDataViewModel.Data == null) throw new NotImplementedException();
        string data = ConvertCustom.ListStringToString(deviceDataViewModel.Data);

        string query = $"INSERT INTO device_data (id, project_id, data, time) VALUES (uuid(), 1, {data}, toTimestamp(now()));";

        ISession session = _cassandraContext.GetSession();

        var result = session.Execute(query);

        if (result != null) return Task.FromResult(true);

        return Task.FromResult(false);
    }


    public Task<List<DeviceData>> GetAll()
    {
        ISession session = _cassandraContext.GetSession();

        IMapper mapper = new Mapper(session);

        string query = $"SELECT * FROM device_data WHERE project_id = 1;";

        IEnumerable<DeviceData> device_data = mapper.Fetch<DeviceData>(query);

        return Task.FromResult(device_data.ToList());
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