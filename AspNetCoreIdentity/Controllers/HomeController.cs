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
            {
                TempData["SuccedMessage"] = "Sing Up Success";
                return RedirectToAction(nameof(HomeController.SignUp));
            }

            foreach(var item in result.Errors)
            {
                ModelState.AddModelError(item.Code, item.Description);
            }
            return View();
        }


        public IActionResult SignIn()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SingIn(SignInViewModel request,string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View();

            returnUrl = returnUrl ?? Url.Action("Index", "Home");

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                ModelState.AddModelError("UserNotFound", "User not found");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("FailSingIn", "Email or password is wrong!");
                return View();
            }
            return Redirect(returnUrl);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}