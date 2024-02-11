using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentity.Areas.Admin.ViewModels
{
    public class UpdateRoleViewModel
    {

        public string Id { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}
