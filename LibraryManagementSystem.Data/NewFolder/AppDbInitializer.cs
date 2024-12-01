using LibraryManagementSystem.Data.Static;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace LibraryManagementSystem.Data.NewFolder
{
    public class AppDbInitializer
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {

            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())

            {
                //Roles

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))

                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                //Users

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string adminUserEmail = "admin@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);

                if (adminUser == null)
                {

                    var newAdminUser = new ApplicationUser()

                    {

                        UserName = "admin@gmail.com",

                        Email = adminUserEmail,

                        EmailConfirmed = true

                    };

                    await userManager.CreateAsync(newAdminUser, "Admin@123");

                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);

                }               

            }

        }

    }
}