// using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOT_API.Models;

// [Table("")]
public class ProjectCassandra
{
    [Column("project_id")]
    public int ProjectId { get; set; }

    [Column("device_name")]
    public List<string>? DeviceName { get; set; }

    [Column("device_spec")]
    public string? DeviceSpec { get; set; }

    // public ProjectCassandra() { }

    // public ProjectCassandra(int project_id, int device, string reason, string message)
    // {
    //     ProjectId = project_id;
    //     Device = device;
    //     Reason = reason;
    //     Message = message;
    // }
}

