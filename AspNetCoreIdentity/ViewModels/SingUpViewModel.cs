using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.ViewModels
{
    public class SingUpViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone")]
        public string Phone {  get; set; }

        [PasswordPropertyText]
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Compare(nameof(Password))]
        [PasswordPropertyText]
        [Required]
        [Display(Name = "Password confirm")]
        public string PasswordConfirm { get; set; }
    }
}
