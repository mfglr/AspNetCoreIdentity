using AspNetCoreIdentity.Models;
using AspNetCoreIdentity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AspNetCoreIdentity.Controllers
{

    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;


        public MemberController(SignInManager<AppUser> signManager, UserManager<AppUser> userManager, AppDbContext context)
        {
            _signManager = signManager;
            _userManager = userManager;
            _context = context;
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

        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> PasswordChange(PasswordChangeViewModel request)
        {
            if (!ModelState.IsValid) return View();


            var user = await _userManager.FindByNameAsync(User.Identity!.Name!);
            if (user == null) throw new Exception("User not found");

            var IsMatchedPassword = await _userManager.CheckPasswordAsync(user, request.OldPassword);

            if (!IsMatchedPassword)
            {
                ModelState.AddModelError(string.Empty, "Not match password");
                return View();
            }

            var result = await _userManager.ChangePasswordAsync(user,request.OldPassword,request.NewPassword);

            if (!result.Succeeded) {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View();
            }

            result = await _userManager.UpdateSecurityStampAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View();
            }

            await _signManager.SignOutAsync();
            await _signManager.PasswordSignInAsync(user, request.NewPassword, true, false);



            return RedirectToAction("signin", "home");
        }



        public async Task<IActionResult> Logout()
        {
            string returnUrl = HttpContext.Request.Query["returnUrl"].ToString() ?? "/Home/Index";

            await _signManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}
