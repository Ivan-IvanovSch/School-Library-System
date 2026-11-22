namespace SchoolLibrary.ViewModels;

public class CatalogViewModel
{
    public string Heading { get; set; } = "Everything";

    public string? Category { get; set; }

    public string[] Categories { get; set; } =
    [
        "Fiction", "Classics", "Science",
        "History", "Poetry", "Fantasy"
    ];

    public IEnumerable<BookCard> Books { get; set; } = [];
}