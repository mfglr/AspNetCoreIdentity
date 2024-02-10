using AspNetCoreIdentity.Areas.Admin.ViewModels;
using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AspNetCoreIdentity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public HomeController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> UserList(CancellationToken cancellationToken)
        {

            var userList = await _userManager
                .Users
                .Select(
                    x => new UserViewModel() {
                        Id = x.Id,
                        Email = x.Email,
                        UserName = x.UserName
                    }
                )
                .ToListAsync(cancellationToken);

            return View(userList);
        }
    }
}
