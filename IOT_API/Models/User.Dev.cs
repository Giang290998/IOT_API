using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IOT_API.Models;

[Table("users_dev")]
public class UserDev
{
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int User_id { get; set; }

    // [Column("pc")]
    // public string Pc { get; set; } = "";

    // [Column("mobile")]
    // public string Mobile { get; set; } = "";

    // [Column("refresh_token")]
    // public string Refresh_token { get; set; } = "";

    // [Column("public_key")]
    // public string Public_key { get; set; } = "";

    // [Column("private_key")]
    // public string Private_key { get; set; } = "";

    [Column("otp")]
    public string Otp { get; set; } = "";

    [Column("otp_expire")]
    public DateTime Otp_expire { get; set; }
}