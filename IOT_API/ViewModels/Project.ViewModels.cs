using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// using NetTopologySuite.Geometries;

namespace IOT_API.ViewModels;

[Table("projects")]
public class CreateProjectViewModel
{
    // [Column("id")]
    // public int Id { get; set; }

    [Required(ErrorMessage = "Own is required")]
    [Column("own")]
    public int Own { get; set; }

    // Dùng kiểu dữ liệu tương ứng với dạng JSONB trong PostgreSQL
    // [Column("staff")]
    // public Dictionary<int, int>? Staff { get; set; }

    [Column("name")]
    public string? Name { get; set; }

    [Column("plant_list")]
    public string[]? PlantList { get; set; }

    [Column("device_list")]
    public string[]? DeviceList { get; set; }

    [Required(ErrorMessage = "IP is required")]
    [Column("ip")]
    public string? Ip { get; set; }

    [Required(ErrorMessage = "MAC is required")]
    [Column("mac")]
    public PhysicalAddress? Mac { get; set; }

    // public Polygon Area { get; set; }

    // [Column("is_delete")]
    // public bool IsDelete { get; set; }
}

// Định nghĩa lớp PhysicalAddress cho kiểu dữ liệu MACADDR
public class PhysicalAddress
{
    public byte[]? Address { get; set; }

    // Thêm các logic xử lý và chuyển đổi nếu cần thiết
}