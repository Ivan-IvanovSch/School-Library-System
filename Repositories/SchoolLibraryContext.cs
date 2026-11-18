using Microsoft.EntityFrameworkCore;
using SchoolLibrary.Models;

namespace SchoolLibrary.Repositories
{
    public class SchoolLibraryContext : DbContext
    {
        public SchoolLibraryContext(DbContextOptions options) : base(options)
        {
        }

        DbSet<Account> Accounts { get; set; }
    }
}
