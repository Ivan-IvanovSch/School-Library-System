using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Models;
using SchoolLibrary.ViewModels;
namespace SchoolLibrary.Controllers;
public class HomeController : Controller
{
    public IActionResult Index() => View(new CatalogViewModel { Heading = "New entries", Books = DemoCatalog.Books.Reverse() });
}
