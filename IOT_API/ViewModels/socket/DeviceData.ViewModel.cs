// using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace IOT_API.ViewModels;

public class DeviceDataViewModel
{
    // [Column("id")]
    // public Guid Id { get; set; }

    // [Column("project_id")]
    [Required(ErrorMessage = "Project id is required")]
    public int ProjectId { get; set; }

    [Required(ErrorMessage = "Data list is required")]
    public List<string>? Data { get; set; }

    public DeviceDataViewModel(int projectId, List<string>? data)
    {
        ProjectId = projectId;
        Data = data;
    }

    // [Column("time")]
    // public DateTimeOffset Time { get; set; }
}

