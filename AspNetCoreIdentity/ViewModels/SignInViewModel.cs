using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.ViewModels
{
    public class SignInViewModel
    {

        [EmailAddress]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }

    }
}
