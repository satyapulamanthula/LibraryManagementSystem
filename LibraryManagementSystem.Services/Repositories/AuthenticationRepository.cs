using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LibraryManagementSystem.Repository.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly RoleManager<IdentityRole> RoleManager;

        public AuthenticationRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)  // Add roleManager as a parameter
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public async Task<SignInResult> SignInAsync(string email, string password, bool rememberMe)
        {
            // Implement your custom logic if needed
            return await SignInManager.PasswordSignInAsync(email, password, rememberMe, false);
        }

        public async Task<IdentityResult> RegisterAsync(string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            // Implement your custom logic if needed
            return await UserManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> AssignRoleAsync(string userId, string role)
        {
            // Get the user
            var user = await UserManager.FindByIdAsync(userId);

            // Check if the user exists
            if (user == null)
            {
                // Handle the situation when the user is not found
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            // Check if the role exists using RoleManager
            if (!await RoleManager.RoleExistsAsync(role))
            {
                // If the role doesn't exist, create it
                var createRoleResult = await RoleManager.CreateAsync(new IdentityRole(role));

                // Check if role creation succeeded
                if (!createRoleResult.Succeeded)
                {
                    // Handle the situation when role creation fails
                    return createRoleResult;
                }
            }

            // Check if the user is already in the role
            if (!await UserManager.IsInRoleAsync(user, role))
            {
                // Add the user to the specified role
                return await UserManager.AddToRoleAsync(user, role);
            }

            // Return success if the user is already in the role
            return IdentityResult.Success;
        }


        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var identityUser = await UserManager.FindByEmailAsync(email);

            if (identityUser != null)
            {
                // Create a new ApplicationUser based on the IdentityUser
                var applicationUser = new ApplicationUser
                {
                    Id = identityUser.Id,
                    UserName = identityUser.UserName,
                    Email = identityUser.Email,
                    // Copy any other properties you need
                };

                return applicationUser;
            }

            return null;
        }

        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            return await UserManager.DeleteAsync(user);
        }

        public async Task SignOutAsync()
        {
            await SignInManager.SignOutAsync();
        }

        //Roles
        public async Task<bool> CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException(nameof(roleName));

            // Check if the role already exists
            bool roleExists = await RoleManager.RoleExistsAsync(roleName);
            if (roleExists)
                return false; // Role already exists

            // Create the role
            IdentityRole identityRole = new IdentityRole
            {
                Name = roleName
            };

            // Saves the role in the underlying AspNetRoles table
            IdentityResult result = await RoleManager.CreateAsync(identityRole);
            return result.Succeeded;
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            return await RoleManager.Roles.ToListAsync();
        }

        public async Task<IdentityResult> UpdateRoleAsync(string roleId, string roleName)
        {
            var role = await RoleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Role with Id = {roleId} not found" });
            }

            role.Name = roleName;
            // Update other properties if needed

            return await RoleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            var role = await RoleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Role with Id = {roleId} not found" });
            }

            return await RoleManager.DeleteAsync(role);
        }

        public async Task<List<UserRoleViewModel>> GetUsersInRoleAsync(string roleId)
        {
            var role = await RoleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return null;
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in UserManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = await UserManager.IsInRoleAsync(user, role.Name)
                };

                model.Add(userRoleViewModel);
            }

            return model;
        }

        public async Task<EditRoleViewModel> GetRoleWithUsers(string roleId)
        {
            var role = await RoleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return null; // or throw exception, handle as needed
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
                Users = new List<string>()
            };

            foreach (var user in UserManager.Users.ToList())
            {
                if (await UserManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return model;
        }

        public async Task<bool> EditUsersInRoleAsync(List<UserRoleViewModel> model, string roleId)
        {
            var role = await RoleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return false; // Role not found
            }

            foreach (var userRoleViewModel in model)
            {
                var user = await UserManager.FindByIdAsync(userRoleViewModel.UserId);
                if (user == null)
                {
                    continue; // Skip if user not found
                }

                IdentityResult result;
                if (userRoleViewModel.IsSelected && !(await UserManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await UserManager.AddToRoleAsync(user, role.Name);
                }
                else if (!userRoleViewModel.IsSelected && await UserManager.IsInRoleAsync(user, role.Name))
                {
                    result = await UserManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue; // No action needed
                }

                if (!result.Succeeded)
                {
                    return false; // Failed to add/remove user from role
                }
            }

            return true; // Success
        }


        //User
        public async Task<List<ApplicationUser>> GetUsersAsync()
        {
            return await UserManager.Users.ToListAsync();
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await UserManager.FindByIdAsync(userId);
        }

        public async Task<List<string>> GetUserClaimsAsync(ApplicationUser user)
        {
            var claims = await UserManager.GetClaimsAsync(user);
            return claims.Select(c => c.Value).ToList();
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await UserManager.GetRolesAsync(user);
        }

        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            var result = await UserManager.UpdateAsync(user);
            return result.Succeeded;
        }


        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);

            if (user == null)
            {
                // User not found, handle accordingly
                return false;
            }

            var result = await UserManager.DeleteAsync(user);

            return result.Succeeded;
        }
    }
}


////For my reference ignore it.
//public async Task<IEnumerable<CreateRoleViewModel>> AssignRole()
//{
//    var usersWithRoles = from u in _DbContext.AspNetUsers
//                         join ur in _DbContext.AspNetUserRoles on u.Id equals ur.UserId into userRoles
//                         from ur in userRoles.DefaultIfEmpty()
//                         join r in _DbContext.AspNetRoles on ur.RoleId equals r.Id into roles
//                         from r in roles.DefaultIfEmpty()
//                         select new CreateRoleViewModel
//                         {
//                             UserId = u.Id,
//                             UserName = u.UserName,
//                             RoleId = r != null ? r.Id : null,
//                             RoleName = r != null ? r.Name : null
//                         };

//    return await usersWithRoles.ToListAsync();}

