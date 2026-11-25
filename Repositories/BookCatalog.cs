using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Models;
using SchoolLibrary.ViewModels;

namespace SchoolLibrary.Repositories;

public static class BookCatalog
{
    // The database has no Color column. Use colors already in site.css.
    private static readonly string[] Colors =
        { "#af9878", "#927b5c", "#6d5e49", "#8c816f", "#756348" };

    public static BookCard ToCard(Book book) => new(
        book.Id,
        book.Title ?? "",
        book.Author ?? "",
        book.Category ?? "",
        Colors[(int)((uint)book.Id % (uint)Colors.Length)]);

    public static async Task<CatalogViewModel> LoadAsync(
        SchoolLibraryContext context, string? category)
    {
        var books = await context.Books.AsNoTracking()
            .OrderBy(b => b.Id).ToListAsync();
        var cards = books.Select(ToCard).ToList();
        var categories = cards.Select(b => b.Category)
            .Where(c => !string.IsNullOrWhiteSpace(c))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c, StringComparer.OrdinalIgnoreCase).ToList();

        var selected = string.IsNullOrWhiteSpace(category) ? null : category.Trim();
        if (selected != null)
            selected = categories.FirstOrDefault(c =>
                string.Equals(c, selected, StringComparison.OrdinalIgnoreCase)) ?? selected;

        return new CatalogViewModel
        {
            Heading = selected ?? "Everything",
            Category = selected,
            Categories = categories,
            Books = selected == null ? cards : cards.Where(b =>
                string.Equals(b.Category, selected, StringComparison.OrdinalIgnoreCase)).ToList()
        };
    }
}
