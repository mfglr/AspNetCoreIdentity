using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AspNetCoreIdentity.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm")]
        public string ConfirmPassword { get; set; }
    }
}
