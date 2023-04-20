using Exercitium.Models;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Exercitium.Data
{
    public class Seed
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                string adminUserEmail = "random";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new User()
                    {
                        UserName = adminUserEmail,
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        FirstName = "random",
                        LastName = "random"
                    };
                    await userManager.CreateAsync(newAdminUser, "random");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "random";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new User()
                    {
                        UserName = appUserEmail,
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        FirstName = "random",
                        LastName = "random"
                    };
                    await userManager.CreateAsync(newAppUser, "random");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
