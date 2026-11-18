using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Models;
using SchoolLibrary.ViewModels;

namespace SchoolLibrary.Controllers;
public class BooksController : Controller
{
    public IActionResult Index(string? category)
    {
        var books = DemoCatalog.Books.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(category))
            books = books.Where(b => string.Equals(b.Category, category, StringComparison.OrdinalIgnoreCase));
        return View(new CatalogViewModel { Heading = category ?? "Everything", Category = category, Books = books });
    }
    public IActionResult Details(int id)
    {
        var book = DemoCatalog.Books.FirstOrDefault(b => b.Id == id);
        if (book is null) return NotFound();
        return View(new BookDetailsViewModel
        {
            Book = book,
            IsAvailable = id % 2 != 0,
            Description = $"Explore {book.Title} by {book.Author}, part of our {book.Category.ToLowerInvariant()} collection. This is a sample description; replace it with the book's description from your database."
        });
    }
}
