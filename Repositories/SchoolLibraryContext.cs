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

        public DbSet<Book> Books { get; set; }
    }   
}
