using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Models;
using Microsoft.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SchoolLibrary.Repositories
{
    public class SchoolLibraryContext : IdentityDbContext<Account>
    {
        public SchoolLibraryContext(DbContextOptions options) : base(options)
        {
        }

        DbSet<BookCard> Books { get; set; }
    }   
}
