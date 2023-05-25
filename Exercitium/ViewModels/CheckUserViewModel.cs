using Microsoft.AspNetCore.Identity;

namespace Exercitium.ViewModels
{
    public class CheckUserViewModel
    {
        public List<UserRoleViewModel> Users { get; set; }
        public RoleManager<IdentityRole> RoleManager { get; set; }
    }
}
