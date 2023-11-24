using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOT_API.Models;

[Table("notifications")]
public class Notification
{
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int User_id { get; set; }

    [Column("datetime")]
    public DateTime date_time { get; set; }

    [Column("noti_sheet_type")]
    public string noti_sheet_type { get; set; } = "";

}


[Table("notification_sheets")]
public class NotificationSheet
{
    [Column("type")]
    public string Type { get; set; } = "";

    [Column("serious_level")]
    public string Serious_level { get; set; } = "";

    [Column("content")]
    public string Content { get; set; } = "";
}


[Table("notification_user")]
public class NotificationUser
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("serious_level")]
    public string SeriousLevel { get; set; } = "";

    [Column("content")]
    public string Content { get; set; } = "";

    [Column("datetime")]
    public DateTime DateTime { get; set; }
}