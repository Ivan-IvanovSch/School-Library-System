using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolLibrary.Models;
using SchoolLibrary.Repositories;
using SchoolLibrary.ViewModels;

namespace SchoolLibrary.Controllers;

public class BooksController : Controller
{
    private readonly SchoolLibraryContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<BooksController> _logger;
    private const int MaxCoverBytes = 5 * 1024 * 1024;

    public BooksController(SchoolLibraryContext context,
        IWebHostEnvironment environment, ILogger<BooksController> logger)
    {
        _context = context;
        _environment = environment;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? category)
    {
        var model = await BookCatalog.LoadAsync(_context, category);
        return View("~/Views/Home/Index.cshtml", model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var book = await _context.Books.AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
        if (book is null) return NotFound();

        return View(new BookDetailsViewModel
        {
            Book = BookCatalog.ToCard(book),
            IsAvailable = book.IsAvailable ?? false,
            Description = book.Description ?? ""
        });
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View("EntryCreateEdit", new BookEntryViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookEntryViewModel model)
    {
        // A new entry must not accept an ID or an existing cover URL from the client.
        model.Id = 0;
        model.ExistingCoverUrl = null;
        ModelState.Remove(nameof(model.Id));
        var cover = await ReadCoverAsync(model.CoverImage);
        if (!ModelState.IsValid) return View("EntryCreateEdit", model);

        var book = new Book();
        ApplyEntry(book, model);
        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        if (cover != null && !await SaveCoverAsync(book.Id, cover))
            return RedirectToAction(nameof(Edit), new { id = book.Id });

        return RedirectToAction(nameof(Details), new { id = book.Id });
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _context.Books.AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
        if (book is null) return NotFound();

        var model = new BookEntryViewModel
        {
            Id = book.Id,
            Title = book.Title ?? "",
            Author = book.Author ?? "",
            Category = book.Category ?? "",
            IsAvailable = book.IsAvailable ?? false,
            AvailableAmount = book.AvailableAmount ?? 0,
            Description = book.Description,
            ExistingCoverUrl = ExistingCoverUrl(book.Id)
        };
        return View("EntryCreateEdit", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(BookEntryViewModel model)
    {
        if (model.Id <= 0) return NotFound();
        // If the form URL carries an ID, it must agree with the hidden ID.
        if (RouteData.Values.TryGetValue("id", out var routeId) && routeId != null &&
            (!int.TryParse(routeId.ToString(), out var urlId) || urlId != model.Id))
            return BadRequest();

        var book = await _context.Books.FindAsync(model.Id);
        if (book is null) return NotFound();

        model.ExistingCoverUrl = ExistingCoverUrl(book.Id);
        var cover = await ReadCoverAsync(model.CoverImage);
        if (!ModelState.IsValid) return View("EntryCreateEdit", model);

        ApplyEntry(book, model);
        await _context.SaveChangesAsync();

        if (cover != null && !await SaveCoverAsync(book.Id, cover))
            return RedirectToAction(nameof(Edit), new { id = book.Id });

        return RedirectToAction(nameof(Details), new { id = book.Id });
    }

    private static void ApplyEntry(Book book, BookEntryViewModel model)
    {
        book.Title = model.Title.Trim();
        book.Author = model.Author.Trim();
        book.Category = model.Category.Trim();
        book.IsAvailable = model.IsAvailable;
        book.AvailableAmount = model.AvailableAmount;
        book.Description = model.Description;
    }

    // Covers are optional files, not database entities. The generated file name
    // uses the database ID, never the original uploaded file name.
    private string CoverPath(int id) => Path.Combine(_environment.ContentRootPath,
        "App_Data", "BookCovers", $"{id}.cover");

    private string? ExistingCoverUrl(int id) => System.IO.File.Exists(CoverPath(id))
        ? Url.Action(nameof(Cover), "Books", new { id }) : null;

    [HttpGet]
    public async Task<IActionResult> Cover(int id)
    {
        if (id <= 0 || !await _context.Books.AnyAsync(b => b.Id == id)) return NotFound();
        var path = CoverPath(id);
        if (!System.IO.File.Exists(path)) return NotFound();
        var bytes = await System.IO.File.ReadAllBytesAsync(path);
        var contentType = CoverContentType(bytes);
        if (contentType == null) return NotFound();
        Response.Headers["X-Content-Type-Options"] = "nosniff";
        Response.Headers["Cache-Control"] = "no-store";
        return File(bytes, contentType);
    }

    private async Task<byte[]?> ReadCoverAsync(IFormFile? file)
    {
        if (file == null) return null;
        if (file.Length <= 0 || file.Length > MaxCoverBytes)
        {
            ModelState.AddModelError(nameof(BookEntryViewModel.CoverImage),
                "Choose a non-empty book cover up to 5 MB.");
            return null;
        }
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        var bytes = stream.ToArray();
        if (bytes.Length > MaxCoverBytes || CoverContentType(bytes) == null)
        {
            ModelState.AddModelError(nameof(BookEntryViewModel.CoverImage),
                "Book covers must be JPG, PNG or WebP images up to 5 MB.");
            return null;
        }
        return bytes;
    }

    // Check the file signature instead of trusting its extension or browser MIME type.
    private static string? CoverContentType(byte[] bytes)
    {
        if (bytes.Length >= 3 && bytes[0] == 0xff && bytes[1] == 0xd8 && bytes[2] == 0xff)
            return "image/jpeg";
        if (bytes.Length >= 8 && bytes.Take(8).SequenceEqual(
            new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 })) return "image/png";
        if (bytes.Length >= 12 && bytes[0] == 82 && bytes[1] == 73 && bytes[2] == 70 &&
            bytes[3] == 70 && bytes[8] == 87 && bytes[9] == 69 && bytes[10] == 66 && bytes[11] == 80)
            return "image/webp";
        return null;
    }

    private async Task<bool> SaveCoverAsync(int id, byte[] bytes)
    {
        var path = CoverPath(id);
        var temporary = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            await System.IO.File.WriteAllBytesAsync(temporary, bytes);
            System.IO.File.Move(temporary, path, overwrite: true);
            return true;
        }
        catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
        {
            _logger.LogError(ex, "Could not save cover for book {BookId}", id);
            TempData["EntryMessage"] = "The book details were saved, but the cover could not be saved. Please select it again.";
            return false;
        }
        finally
        {
            try { System.IO.File.Delete(temporary); }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            { _logger.LogWarning(ex, "Could not clean up temporary book cover"); }
        }
    }
}
