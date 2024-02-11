using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [EmailAddress]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
