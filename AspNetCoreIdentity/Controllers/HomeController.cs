using AspNetCoreIdentity.Models;
using AspNetCoreIdentity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspNetCoreIdentity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SingUpViewModel request)
        {

            if(!ModelState.IsValid) return View();

            var result = await _userManager.CreateAsync(
                new AppUser()
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    PhoneNumber = request.Phone
                },
                request.Password
            );

            if (result.Succeeded)
                return RedirectToAction(nameof(HomeController.SignIn));

            foreach(var item in result.Errors)
            {
                ModelState.AddModelError(String.Empty, item.Description);
            }
            return View();
        }


        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user == null)
            {
                ModelState.AddModelError(String.Empty, "User not found!");
                return View();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string? resetLink = Url.Action("ResetPassword", "Home", new { userId = user.Id, token = token });



            TempData["success"] = "Email has been sent your address.";
            return RedirectToAction(nameof(ForgetPassword));

        }

        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel request,string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View();

            returnUrl = returnUrl ?? Url.Action("Index","Member");

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                ModelState.AddModelError(String.Empty, "User not found");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            

            if(result.Succeeded) return Redirect(returnUrl);
            else
            {
                if (result.IsLockedOut)
                    ModelState.AddModelError(String.Empty, "You must try to singin after 3 minutes!");
                else
                    ModelState.AddModelError(String.Empty, "Email or password is wrong!");
            }
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}