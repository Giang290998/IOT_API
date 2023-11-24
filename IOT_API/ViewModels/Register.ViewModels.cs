using System.ComponentModel.DataAnnotations;

namespace IOT_API.ViewModels;

public class RegisterViewModel
{
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Password is required")]
    [RegularExpression(@"^\$2[ayb]\$.{56}$", ErrorMessage = "Invalid password")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Phone is required")]
    [RegularExpression(@"^\+?\d{10,15}$", ErrorMessage = "Invalid phone")]
    public string Phone { get; set; } = "";

    public string Email { get; set; } = "";

    public string Name { get; set; } = "";

    public string Address { get; set; } = "";


}