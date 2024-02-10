using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Controllers
{

    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signManager;

        public MemberController(SignInManager<AppUser> signManager)
        {
            _signManager = signManager;
        }

        public IActionResult Index()
        {
            return View();
        } 

        public async Task<IActionResult> Logout()
        {
            string returnUrl = HttpContext.Request.Query["returnUrl"].ToString() ?? "/Home/Index";

            await _signManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}
