using System.ComponentModel.DataAnnotations;

namespace IOT_API.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Phone is required")]
    public string Phone { get; set; } = "";

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = "";
}