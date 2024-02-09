using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.ViewModels
{
    public class SingUpViewModel
    {
        [Display(Name = "User name")]
        public string UserName { get; set; }
       
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Display(Name = "Phone")]
        public string Phone {  get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Password confirm")]
        public string PasswordConfirm { get; set; }
    }
}
