using Microsoft.AspNetCore.Mvc;
using SchoolLibrary.Models;
namespace SchoolLibrary.Controllers;
// UI scaffold only. Replace POST actions with ASP.NET Core Identity before real use.
public class AccountController : Controller
{
    public IActionResult Index() => View("AccountWarning");
    public IActionResult Preview() => View("Index", new AccountViewModel());
    public IActionResult AccountWarning() => View();
    public IActionResult Login() => View(new LoginViewModel());
    public IActionResult Register() => View(new RegisterViewModel());
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid) ModelState.AddModelError("", "Demo only: connect ASP.NET Core Identity to enable login.");
        return View(model);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Register(RegisterViewModel model)
    {
        if (ModelState.IsValid) ModelState.AddModelError("", "Demo only: connect ASP.NET Core Identity to create accounts.");
        return View(model);
    }
}
