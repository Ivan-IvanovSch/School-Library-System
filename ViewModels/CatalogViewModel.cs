using System;
using System.Collections.Generic;

namespace SchoolLibrary.ViewModels;

public class CatalogViewModel
{
    public string Heading { get; set; } = "Everything";
    public string? Category { get; set; }
    public IEnumerable<string> Categories { get; set; } = Array.Empty<string>();
    public IEnumerable<BookCard> Books { get; set; } = Array.Empty<BookCard>();
}
