using AspNetCoreIdentity.Models;
using AspNetCoreIdentity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.FileProviders;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AspNetCoreIdentity.Controllers
{

    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IFileProvider _fileProvider;

        public MemberController(SignInManager<AppUser> signManager, UserManager<AppUser> userManager, AppDbContext context, IFileProvider fileProvider)
        {
            _signManager = signManager;
            _userManager = userManager;
            _context = context;
            _fileProvider = fileProvider;
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

        public async Task<IActionResult> UpdateUserAsync()
        {

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user == null) throw new Exception("User not found");

            var model = new UserEditViewModel()
            {
                BirthDay = user.BirthDay,
                City = user.City,
                Email = user.Email,
                UserName = user.UserName,
                Gender = user.Gender,
                Phone = user.PhoneNumber,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserEditViewModel request)
        {

            if (!ModelState.IsValid) return View();
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (request.Picture != null) {
                if(request.Picture.Length <= 0)
                {
                    ModelState.AddModelError(string.Empty, "error");
                    return View();
                }
                var wwwrootFolder = _fileProvider.GetDirectoryContents("wwwroot");
                var fileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(request.Picture.FileName)}";

                var path = Path.Combine(wwwrootFolder.First(x => x.Name == "userPictures").PhysicalPath, fileName);

                using var stream = new FileStream(path, FileMode.Create);
                await request.Picture.CopyToAsync(stream);
                
                user.PictureUrl = fileName;
            }
           

            user.PhoneNumber = request.Phone;
            user.UserName = request.UserName;
            user.Gender = request.Gender;
            user.BirthDay = request.BirthDay;
            user.Email = request.Email;
            user.City = request.City;

            var result = await _userManager.UpdateAsync(user);

            if(!result.Succeeded) {

                foreach (var item in result.Errors)
                    ModelState.AddModelError(string.Empty, item.Description);
                return View();
            }

            await _userManager.UpdateSecurityStampAsync(user);
            await _signManager.SignOutAsync();
            await _signManager.SignInAsync(user, true);


            return View(request);
        }


        public IActionResult AccessDenied(string returnUrl)
        {
            return View();
        }

    }
}
