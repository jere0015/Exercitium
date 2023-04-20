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
                string adminUserEmail = "jeremy@live.dk";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new User()
                    {
                        UserName = adminUserEmail,
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        FirstName = "Jeremy",
                        LastName = "Andersen"
                    };
                    await userManager.CreateAsync(newAdminUser, "!Strong99");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "jeremyandersen99@gmail.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new User()
                    {
                        UserName = appUserEmail,
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        FirstName = "Jerry",
                        LastName = "Anderson"
                    };
                    await userManager.CreateAsync(newAppUser, "?Strong99");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
