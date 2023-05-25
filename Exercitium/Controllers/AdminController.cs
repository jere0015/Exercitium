using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Exercitium.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (!string.IsNullOrEmpty(roleName))
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    var newRole = new IdentityRole(roleName);
                    await _roleManager.CreateAsync(newRole);
                }
            }

            return View();
        }
    }
}
