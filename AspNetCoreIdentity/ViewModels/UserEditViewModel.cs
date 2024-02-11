using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.ViewModels
{
    public class UserEditViewModel
    {

        
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        
        [Display(Name = "User Name")]
        public string? UserName { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        [Display(Name = "City")]
        public string? City { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? Picture { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "BirtDay")]
        public DateTime? BirthDay { get; set; }

        [Display(Name = "Gender")]
        public bool? Gender { get; set; }
    }
}
