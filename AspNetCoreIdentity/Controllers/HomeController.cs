﻿using AspNetCoreIdentity.Models;
using AspNetCoreIdentity.Services;
using AspNetCoreIdentity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace AspNetCoreIdentity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;



        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {

            var user = User;
            var claims = User.Claims.ToList();

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
            var user = new AppUser()
            {
                UserName = request.UserName,
                Email = request.Email,
                PhoneNumber = request.Phone
            };
            var result = await _userManager.CreateAsync(
                user,
                request.Password
            );

            if (result.Succeeded)
            {

                await _userManager.AddClaimAsync(
                    user,
                    new Claim("ExpireDateOfFreeAccess", DateTime.Now.AddDays(10).ToString())
                );
                return RedirectToAction(nameof(HomeController.SignIn));
            }

            foreach (var item in result.Errors)
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
            string? resetLink = Url.Action("ResetPassword", "Home", new { userId = user.Id, token = token },"https", "localhost:7178");

            await _emailService.SendResetPasswordEmail(resetLink, user.Email);

            TempData["success"] = "Email has been sent your address.";
            return RedirectToAction(nameof(ForgetPassword));

        }


        public async Task<IActionResult> ResetPassword(string userId, string token)
        {
            ViewBag.Error = false;
            AppUser? user;

            if (userId == null || (user = await _userManager.FindByIdAsync(userId)) == null)
            {
                ViewBag.Error = true;
                ModelState.AddModelError(String.Empty, "User not found!");
                return View();
            }

            if(token == null)
            {
                ViewBag.Error = true;
                ModelState.AddModelError(String.Empty, "Token not found!");
                return View();
            }

            var purpose = UserManager<AppUser>.ResetPasswordTokenPurpose;
            var isValid = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, purpose, token);

            if (!isValid)
            {
                ViewBag.Error = true;
                ModelState.AddModelError(String.Empty, "Token is not valid!");
                return View();
            }
            
            TempData["UserId"] = userId;
            TempData["Token"] = token;
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {

            ViewBag.Error = false;

            var userId = TempData["UserId"];
            var token = TempData["token"];

            if (userId == null || token == null)
            {
                throw new Exception("error");
            }

            var user = await _userManager.FindByIdAsync(userId.ToString()!);

            var result = await _userManager.ResetPasswordAsync(user!, token.ToString()!, request.Password);

            if (!result.Succeeded)
            {
                ViewBag.Error = true;
                foreach(var item in result.Errors)
                    ModelState.AddModelError(String.Empty, item.Description);
                return View();
            }

            return RedirectToAction(nameof(HomeController.SignIn));
        }

        [HttpGet]
        public IActionResult LoginByFacebook()
        {
            var redirectUrl = Url.Action("ExternalResponse", "Home");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult("Facebook", properties);
        }

        public async Task<IActionResult> ExternalResponse()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
                throw new Exception("Third party authentication failed!"    );

            var email = info.Principal.FindFirst(ClaimTypes.Email)!.Value;

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new AppUser()
                {
                    Email = email,
                    UserName = email
                };

                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                    throw new Exception("");

                result = await _userManager.AddLoginAsync(user, info);
                if (!result.Succeeded)
                    throw new Exception("");
            }
            await _signInManager.SignInAsync(user, true);

            return RedirectToAction("index");

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