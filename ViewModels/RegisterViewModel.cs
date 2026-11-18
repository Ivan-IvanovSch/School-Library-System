using System.ComponentModel.DataAnnotations;

namespace SchoolLibrary.ViewModels;

public class RegisterViewModel
{
    [Required]
    [StringLength(40, MinimumLength = 3)]
    public string Username { get; set; } = "";

    [Required]
    [EmailAddress]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = "";

    [Required]
    [StringLength(100, MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = "";
}