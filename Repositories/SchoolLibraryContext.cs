using Microsoft.EntityFrameworkCore;
using SchoolLibrary.ViewModels;
using SchoolLibrary.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SchoolLibrary.Repositories
{
    public class SchoolLibraryContext : IdentityDbContext<Account>
    {
        public SchoolLibraryContext(DbContextOptions<SchoolLibraryContext> options) : base(options)
        {
        }

        DbSet<BookCard> Books { get; set; }
    }   
}
