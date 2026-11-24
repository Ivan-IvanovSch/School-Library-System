using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace SchoolLibrary.ViewModels;
public class BookEntryViewModel
{
 public int Id { get; set; }
 [Required, StringLength(200)] public string Title { get; set; } = "";
 [Required, StringLength(150)] public string Author { get; set; } = "";
 public bool IsAvailable { get; set; } = true;
 [Range(0, 100000)] public int AvailableAmount { get; set; }
 [StringLength(5000)] public string? Description { get; set; }
 public IFormFile? CoverImage { get; set; }
 public string? ExistingCoverUrl { get; set; }
}
