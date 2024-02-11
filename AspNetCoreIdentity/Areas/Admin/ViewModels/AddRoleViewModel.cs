using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Areas.Admin.ViewModels
{
    public class AddRoleViewModel
    {

        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}
