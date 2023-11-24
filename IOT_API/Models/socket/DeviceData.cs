// using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOT_API.Models;

[Table("device_data")]
public class DeviceData
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("project_id")]
    public int ProjectId { get; set; }

    [Column("data")]
    public List<string>? Data { get; set; }

    [Column("time")]
    public DateTimeOffset Time { get; set; }

    public DeviceData() { }

    public DeviceData(Guid id, int projectId, List<string>? data, DateTimeOffset time)
    {
        Id = id;
        ProjectId = projectId;
        Data = data;
        Time = time;
    }
}

