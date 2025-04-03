using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace Icarus_Item_Calculator.Areas.Identity.Pages.Admin
{
    [Authorize(Roles = "Admin")] // Restrict to Admin role
    public class UsersModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IList<IdentityUser> Users { get; set; }
        [TempData]
        public string Message { get; set; }
        [TempData]
        public string Error { get; set; }

        public async Task OnGetAsync()
        {
            Users = _userManager.Users.ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                Error = "User not found.";
                return RedirectToPage();
            }

            // Prevent self-deletion
            if (User.Identity.Name == user.UserName)
            {
                Error = "You cannot delete yourself.";
                return RedirectToPage();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                Message = $"User {user.Email} deleted successfully.";
            }
            else
            {
                Error = "Failed to delete user: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToPage();
        }


        public async Task<IActionResult> OnPostAddRoleAsync(string id, string roleName)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || string.IsNullOrEmpty(roleName))
            {
                Error = "User not found or role name is empty.";
                return RedirectToPage();
            }

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                Message = $"Added {roleName} to {user.Email}.";
            }
            else
            {
                Error = "Failed to add role: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveRoleAsync(string id, string roleName)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null || string.IsNullOrEmpty(roleName))
            {
                Error = "User not found or role name is empty.";
                return RedirectToPage();
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                Message = $"Removed {roleName} from {user.Email}.";
            }
            else
            {
                Error = "Failed to remove role: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToPage();
        }


        public async Task<IList<string>> GetUserRolesAsync(IdentityUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}