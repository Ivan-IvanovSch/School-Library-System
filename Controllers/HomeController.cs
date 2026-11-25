using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Models;
using SchoolLibrary.ViewModels;
namespace SchoolLibrary.Controllers;
using SchoolLibrary.Repositories;
using System.Runtime.Intrinsics.X86;
using System.Security.Policy;
using System.Threading.Tasks;
public class HomeController : Controller
{
    private readonly SchoolLibraryContext _context;

    public HomeController(SchoolLibraryContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<IActionResult> Index(string? category)
    {
        var model = await BookCatalog.LoadAsync(_context, category);
        return View(model);
    }


}
