using AspNetCoreIdentity.Models;
using AspNetCoreIdentity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Controllers
{

    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signManager;
        private readonly UserManager<AppUser> _userManager;

        public MemberController(SignInManager<AppUser> signManager, UserManager<AppUser> userManager)
        {
            _signManager = signManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);

            if (user == null) throw new Exception("User not found!");

            var userViewModel = new UserViewModel()
            {
                Email = user.Email!,
                UserName = user.UserName,
                Phone = user.PhoneNumber
            };

            return View(userViewModel);
        } 

        public async Task<IActionResult> Logout()
        {
            string returnUrl = HttpContext.Request.Query["returnUrl"].ToString() ?? "/Home/Index";

            await _signManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}
