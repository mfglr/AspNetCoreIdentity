using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.ViewModels
{
    public class PasswordChangeViewModel
    {

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        [MinLength(9)]
        public string NewPassword { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        [Display(Name = "Confirm Password")]
        [MinLength(9)]
        public string ConfirmPassword { get; set;}
    }
}
