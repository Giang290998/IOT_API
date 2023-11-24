using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace IOT_API.Models;
// id SERIAL PRIMARY KEY,
//     username VARCHAR(50),
//     password VARCHAR(100) NOT NULL,
//     phone VARCHAR(20) UNIQUE NOT NULL,
//     name VARCHAR(50) DEFAULT 'admin',
//     email VARCHAR(50),
//     address VARCHAR(255),
//     role SMALLINT CHECK (role >= 0 AND role <= 9) NOT NULL,
//     is_active BOOLEAN DEFAULT FALSE,
//     is_authenticated_mail BOOLEAN DEFAULT FALSE,
//     is_delete BOOLEAN DEFAULT FALSE

[Table("admins")]
public class Admin
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

    [Required(ErrorMessage = "Role is required")]
    [Column("role")]
    public int Role { get; set; }

}