// using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOT_API.Models;

[Table("alarm")]
public class Alarm
{
    [Column("project_id")]
    public int ProjectId { get; set; }

    [Column("device")]
    public int Device { get; set; }

    [Column("reason")]
    public string? Reason { get; set; }

    [Column("message")]
    public string? Message { get; set; }

    [Column("time")]
    public DateTimeOffset? Time { get; set; }

    public Alarm() { }

    public Alarm(int project_id, int device, string reason, string message)
    {
        ProjectId = project_id;
        Device = device;
        Reason = reason;
        Message = message;
    }
}

