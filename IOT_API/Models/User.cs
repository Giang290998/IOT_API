using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace IOT_API.Models;


public class UserReturn
{
    public string? Username { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
}

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }


    [StringLength(50, MinimumLength = 6, ErrorMessage = "Username must be between 6 - 50 characters")]
    [Column("username")]
    public string Username { get; set; } = "";


    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^\$2[ayb]\$.{56}$", ErrorMessage = "Invalid password")]
    [Column("password")]
    public string Password { get; set; } = "";


    [Required(ErrorMessage = "Phone is required")]
    [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Invalid phone")]
    [Column("phone")]
    public string Phone { get; set; } = "";


    [EmailAddress(ErrorMessage = "Invalid email")]
    [Column("email")]
    public string? Email { get; set; }


    [StringLength(50, MinimumLength = 2, ErrorMessage = "Username must be between 2 - 50 characters")]
    [Column("name")]
    public string Name { get; set; } = "";


    [MaxLength(255)]
    [Column("address")]
    public string? Address { get; set; }

    // [Column("balance")]
    // public decimal? Balance { get; set; }

    // [Column("level")]
    // public short? Level { get; set; }

    // public bool? IsActive { get; set; }

    // public bool? IsAuthenticatedMail { get; set; }

    // public bool? IsBan { get; set; }


    public UserReturn Return()
    {
        return new UserReturn
        {
            Username = Username,
            Phone = Phone,
            Email = Email ?? "",
            Name = Name,
            Address = Address ?? ""
        };
    }
}