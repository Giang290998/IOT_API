using System.ComponentModel.DataAnnotations;
// using NetTopologySuite.Geometries;

namespace IOT_API.ViewModels;

public class UpdateProjectViewModel
{
    [Required(ErrorMessage = "ProjectId is required")]
    public int ProjectId { get; set; }
    public string? Own { get; set; }
    public Dictionary<string, int>? Staff { get; set; }
    public string? Name { get; set; }
    public string[]? PlantList { get; set; }
    public string[]? DeviceList { get; set; }
    public string? Ip { get; set; }
    public string? Mac { get; set; }
    // public Polygon Area { get; set; }
}
