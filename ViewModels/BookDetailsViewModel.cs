namespace SchoolLibrary.ViewModels;

public class BookDetailsViewModel
{
    public BookCard Book { get; set; } =
        new(0, "", "", "", "#af9878");

    public bool IsAvailable { get; set; }

    public string Description { get; set; } = "";
}