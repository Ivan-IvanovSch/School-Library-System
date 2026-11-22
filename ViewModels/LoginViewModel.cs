using System.ComponentModel.DataAnnotations;

namespace SchoolLibrary.ViewModels;

public class LoginViewModel
{
    [Required]
    [Display(Name = "Username or e-mail")]
    public string UsernameOrEmail { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    public bool RememberMe { get; set; }
}