using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Controllers
{
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signManager;

        public MemberController(SignInManager<AppUser> signManager)
        {
            _signManager = signManager;
        }

        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
