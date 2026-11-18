using SchoolLibrary.ViewModels;

namespace SchoolLibrary.Models;
public static class DemoCatalog
{
    public static readonly BookCard[] Books = [
        new(1, "The Little Prince", "Antoine de Saint-Exupéry", "Fiction", "#b19b79"),
        new(2, "Pride and Prejudice", "Jane Austen", "Classics", "#8b9b85"),
        new(3, "A Brief History of Time", "Stephen Hawking", "Science", "#879ba2"),
        new(4, "The Hobbit", "J. R. R. Tolkien", "Fantasy", "#9b896d"),
        new(5, "Selected Poems", "Emily Dickinson", "Poetry", "#b08e91"),
        new(6, "The Histories", "Herodotus", "History", "#b79a74")
    ];
}
