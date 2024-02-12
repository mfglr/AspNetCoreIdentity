using AspNetCoreIdentity.Areas.Admin.ViewModels;
using AspNetCoreIdentity.Models;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreIdentity.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RolesController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();


            return View(roles);
        }

        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(AddRoleViewModel request)
        {

            var result = await _roleManager.CreateAsync(new AppRole() { Name = request.Name });

            if (!result.Succeeded) {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View();
                }
            }
            return RedirectToAction(nameof(RolesController.Index));
        }

        public async Task<IActionResult> UpdateRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ModelState.AddModelError(string.Empty, "Role not found");
                return View();
            }

            return View(new UpdateRoleViewModel() { Id = role.Id,Name = role.Name });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(UpdateRoleViewModel request)
        {
            if (!ModelState.IsValid) return View();


            var role = await _roleManager.FindByIdAsync(request.Id);

            if (role == null)
            {
                ModelState.AddModelError(string.Empty, "Role not found");
                return View();
            }

            role.Name = request.Name;

            await _roleManager.UpdateAsync(role);

            return RedirectToAction(nameof(RolesController.Index));
        }


        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                throw new Exception("error");
            }

            await _roleManager.DeleteAsync(role);
            
            return RedirectToAction(nameof(RolesController.Index));

        }


        public async Task<IActionResult> AssignRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) throw new Exception("The user was not found");
            
            ViewBag.Id = id;
            
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);
            
            var model = roles
                .Select(
                    role => new AssignRoleViewModel()
                    {
                        Id = role.Id,
                        Name = role.Name!,
                        Exist = userRoles.Contains(role.Name!)
                    }
                )
                .ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId,List<AssignRoleViewModel> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) throw new Exception("error");

            foreach (var role in roles)
            {
                if (role.Exist)
                    await _userManager.AddToRoleAsync(user, role.Name);
                else
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
            }

            return RedirectToAction("UserList", "Home");
        }

    }
}
