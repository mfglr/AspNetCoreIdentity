using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentity.Models
{
    public class AppUser : IdentityUser
    {
        public string? City { get; set; }
        public string? PictureUrl { get; set; }
        public DateTime? BirthDay { get; set; }
        public bool? Gender { get; set; }
    }
}
