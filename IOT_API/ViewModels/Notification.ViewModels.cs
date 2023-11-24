using System.ComponentModel.DataAnnotations;

namespace IOT_API.ViewModels;

public class NotificationViewModel
{
    [Required(ErrorMessage = "Uid is required")]
    public int User_id { get; set; }

    [Required(ErrorMessage = "noti_sheet_type is required")]
    public string noti_sheet_type { get; set; } = "";
}