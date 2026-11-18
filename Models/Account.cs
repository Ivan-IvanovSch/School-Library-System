using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace SchoolLibrary.Models
{
    public class Account : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
