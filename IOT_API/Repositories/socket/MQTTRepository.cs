using IOT_API.Models;
using ISession = Cassandra.ISession;
using IOT_API.ViewModels;
using IOT_API.Helper;
using Cassandra.Mapping;
using IOT_API.Filters;
using System.Net;

namespace IOT_API.Repositories;

public class MQTTRepository : IMQTTRepository
{
    private readonly CassandraContext _cassandraContext;

    public MQTTRepository(CassandraContext cassandraContext)
    {
        _cassandraContext = cassandraContext;
    }

    public Task CreateSensorDataInterval(DeviceDataViewModel deviceDataViewModel)
    {
        if (deviceDataViewModel.Data == null) throw new HttpResponseException(HttpStatusCode.BadRequest);
        string data = ConvertCustom.ListStringToString(deviceDataViewModel.Data);

        string query = $"INSERT INTO device_data (id, project_id, data, time) VALUES (uuid(), 1, {data}, toTimestamp(now()));";

        ISession session = _cassandraContext.GetSession();

        var result = session.Execute(query);

        if (result != null) return Task.FromResult(true);

        return Task.FromResult(false);
    }

    // public Task<bool> Create(DeviceDataViewModel deviceDataViewModel)
    // {
    //     if (deviceDataViewModel.Data == null) throw new NotImplementedException();
    //     string data = ConvertCustom.ListStringToString(deviceDataViewModel.Data);

    //     string query = $"INSERT INTO device_data (id, project_id, data, time) VALUES (uuid(), 1, {data}, toTimestamp(now()));";

    //     ISession session = _cassandraContext.GetSession();

    //     var result = session.Execute(query);

    //     if (result != null) return Task.FromResult(true);

    //     return Task.FromResult(false);
    // }

    public Task CreateSensorIssue(Alarm alarmData)
    {
        int project_id = alarmData.ProjectId;
        int device = alarmData.Device;
        string reason = alarmData.Reason ?? "";
        string message = alarmData.Message ?? "";

        ISession session = _cassandraContext.GetSession();

        SendAlarmSMS(project_id, device, reason, session);

        string query = $"INSERT INTO statistic.alarm (id, project_id, device, reason, message, time) VALUES (uuid(), {project_id}, {device}, '{reason}', '{message}', toTimestamp(now()));";

        var result = session.Execute(query);

        if (result != null) return Task.FromResult(true);

        return Task.FromResult(false);
    }

    private static bool SendAlarmSMS(int project_id, int device, string reason, ISession session)
    {
        IMapper mapper = new Mapper(session);

        string query = $"SELECT * FROM project WHERE project_id = {project_id};";

        IEnumerable<ProjectCassandra>? result = mapper.Fetch<ProjectCassandra>(query);

        ProjectCassandra? p_cass = result.FirstOrDefault();

        if (p_cass == null) return false;

        string message = $"Device {p_cass.device_name?[device]} is having issue: {reason}";

        TWILIO.SendSMS(p_cass.phone ?? "0", message);

        return true;
    }

    public Task<List<Alarm>> GetAllAlarm(int project_id)
    {
        ISession session = _cassandraContext.GetSession();

        IMapper mapper = new Mapper(session);

        string query = $"SELECT * FROM alarm WHERE project_id = {project_id};";

        IEnumerable<Alarm> result = mapper.Fetch<Alarm>(query);

        return Task.FromResult(result.ToList());
    }

    public Task<List<DeviceData>> GetAllDeviceData(int project_id)
    {
        ISession session = _cassandraContext.GetSession();

        IMapper mapper = new Mapper(session);

        string query = $"SELECT * FROM device_data WHERE project_id = {project_id};";

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