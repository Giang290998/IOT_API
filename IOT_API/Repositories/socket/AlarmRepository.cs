using IOT_API.Models;
using ISession = Cassandra.ISession;
using IOT_API.ViewModels;
using IOT_API.Helper;
using Cassandra.Mapping;

namespace IOT_API.Repositories;

public class AlarmRepository : IAlarmRepository
{
    private readonly CassandraContext _cassandraContext;

    public AlarmRepository(CassandraContext cassandraContext)
    {
        _cassandraContext = cassandraContext;
    }

    // public Task CreateSensorDataInterval(DeviceData deviceData)
    // {
    //     int project_id = deviceData.ProjectId;
    //     string data = ConvertCustom.ListStringToString(deviceData.Data);

    //     string query = $"INSERT INTO statistic.device_data (id, project_id, data, time) " +
    //         $"VALUES (uuid(), {project_id}, {data}, toTimestamp(now()));";

    //     ISession session = _cassandraContext.GetSession();

    //     IMapper mapper = new Mapper(session);

    //     IEnumerable<DeviceData> result = mapper.Fetch<DeviceData>(query);

    //     List<DeviceData> device_data = result.ToList();

    //     throw new NotImplementedException();
    // }

    public Task<bool> CreateSensorIssue(Alarm alarmData)
    {
        int project_id = alarmData.ProjectId;
        int device = alarmData.Device;
        string reason = alarmData.Reason ?? "";
        string message = alarmData.Message ?? "";

        ISession session = _cassandraContext.GetSession();

        IMapper mapper = new Mapper(session);

        string query = $"INSERT INTO statistic.alarm (project_id, device, reason, message, time) " +
            $"VALUES ({project_id}, {device}, '{reason}', '{message}', toTimestamp(now()));";

        IEnumerable<Alarm> result = mapper.Fetch<Alarm>(query);

        var data = result.ToList();

        Console.WriteLine(data);
        Console.WriteLine("Alarm Repo");

        return Task.FromResult(true);
    }
}