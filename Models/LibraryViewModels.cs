using System.ComponentModel.DataAnnotations;
namespace SchoolLibrary.Models;
public record BookCard(int Id, string Title, string Author, string Category, string Color);
public class CatalogViewModel
{
    public string Heading { get; set; } = "Everything";
    public string? Category { get; set; }
    public string[] Categories { get; set; } = ["Fiction", "Classics", "Science", "History", "Poetry", "Fantasy"];
    public IEnumerable<BookCard> Books { get; set; } = [];
}
public class LoginViewModel
{
    [Required, Display(Name = "Username or e-mail")] public string UsernameOrEmail { get; set; } = "";
    [Required, DataType(DataType.Password)] public string Password { get; set; } = "";
}
public class RegisterViewModel
{
    [Required, StringLength(40, MinimumLength = 3)] public string Username { get; set; } = "";
    [Required, EmailAddress, Display(Name = "E-mail")] public string Email { get; set; } = "";
    [Required, StringLength(100, MinimumLength = 8), DataType(DataType.Password)] public string Password { get; set; } = "";
}
public class AccountViewModel
{
    public string Name { get; set; } = "Alex Reader";
    public string Username { get; set; } = "alex.reader";
    public string Email { get; set; } = "alex@example.com";
}

public class BookDetailsViewModel
{
    public BookCard Book { get; set; } = new(0, "", "", "", "#af9878");
    public bool IsAvailable { get; set; }
    public string Description { get; set; } = "";
}
