using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetRFID.Models;
using System.Linq;
using System.Threading.Tasks;

public class UserRoleController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRoleController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var userRoles = await _userManager.GetRolesAsync(user);
        var allRoles = _roleManager.Roles.ToList();

        var model = new EditUserRoleViewModel
        {
            UserId = user.Id,
            UserName = user.UserName,
            CurrentRole = userRoles.FirstOrDefault(),
            NewRole = userRoles.FirstOrDefault() // Or set it to an appropriate default value
        };

        ViewBag.Roles = new SelectList(allRoles, "Name", "Name");

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditUserRoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var currentRole = userRoles.FirstOrDefault();
            if (currentRole != null && currentRole != model.NewRole)
            {
                await _userManager.RemoveFromRoleAsync(user, currentRole);
            }

            if (!await _userManager.IsInRoleAsync(user, model.NewRole))
            {
                await _userManager.AddToRoleAsync(user, model.NewRole);
            }

            return RedirectToAction("Index");
        }

        var allRoles = _roleManager.Roles.ToList();
        ViewBag.Roles = new SelectList(allRoles, "Name", "Name");

        return View(model);
    }
}