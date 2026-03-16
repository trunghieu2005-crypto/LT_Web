using Microsoft.AspNetCore.Identity;

namespace Bai6_DATM.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Address { get; set; }
    }
}
