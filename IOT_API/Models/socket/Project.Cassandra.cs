// using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOT_API.Models;

// [Table("")]
public class ProjectCassandra
{
    // [Column("id")]
    public Guid? Id { get; set; }

    // [Column("project_id")]
    public int? project_id { get; set; }

    // [Column("device_name")]
    public List<string>? device_name { get; set; }

    // [Column("device_spec")]
    public Dictionary<string, List<short>>? device_spec { get; set; }

    // public ProjectCassandra() { }

    // public ProjectCassandra(int project_id, int device, string reason, string message)
    // {
    //     ProjectId = project_id;
    //     Device = device;
    //     Reason = reason;
    //     Message = message;
    // }
}

