using LibraryManagementSystem.Data.Entities;
using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Repository.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)  // Add roleManager as a parameter
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<SignInResult> SignInAsync(string email, string password, bool rememberMe)
        {
            // Implement your custom logic if needed
            return await _signInManager.PasswordSignInAsync(email, password, rememberMe, false);
        }

        public async Task<IdentityResult> RegisterAsync(string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            // Implement your custom logic if needed
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> AssignRoleAsync(string userId, string role)
        {
            // Get the user
            var user = await _userManager.FindByIdAsync(userId);

            // Check if the user exists
            if (user == null)
            {
                // Handle the situation when the user is not found
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            // Check if the role exists using RoleManager
            if (!await _roleManager.RoleExistsAsync(role))
            {
                // If the role doesn't exist, create it
                var createRoleResult = await _roleManager.CreateAsync(new IdentityRole(role));

                // Check if role creation succeeded
                if (!createRoleResult.Succeeded)
                {
                    // Handle the situation when role creation fails
                    return createRoleResult;
                }
            }

            // Check if the user is already in the role
            if (!await _userManager.IsInRoleAsync(user, role))
            {
                // Add the user to the specified role
                return await _userManager.AddToRoleAsync(user, role);
            }

            // Return success if the user is already in the role
            return IdentityResult.Success;
        }


        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var identityUser = await _userManager.FindByEmailAsync(email);

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
            return await _userManager.DeleteAsync(user);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        //Roles
        public async Task<bool> CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException(nameof(roleName));

            // Check if the role already exists
            bool roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
                return false; // Role already exists

            // Create the role
            IdentityRole identityRole = new IdentityRole
            {
                Name = roleName
            };

            // Saves the role in the underlying AspNetRoles table
            IdentityResult result = await _roleManager.CreateAsync(identityRole);
            return result.Succeeded;
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public async Task<IdentityResult> UpdateRoleAsync(string roleId, string roleName)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Role with Id = {roleId} not found" });
            }

            role.Name = roleName;
            // Update other properties if needed

            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Role with Id = {roleId} not found" });
            }

            return await _roleManager.DeleteAsync(role);
        }

        public async Task<List<UserRoleViewModel>> GetUsersInRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return null;
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                };

                model.Add(userRoleViewModel);
            }

            return model;
        }

        public async Task<EditRoleViewModel> GetRoleWithUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
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

            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return model;
        }

        public async Task<bool> EditUsersInRoleAsync(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                return false; // Role not found
            }

            foreach (var userRoleViewModel in model)
            {
                var user = await _userManager.FindByIdAsync(userRoleViewModel.UserId);
                if (user == null)
                {
                    continue; // Skip if user not found
                }

                IdentityResult result;
                if (userRoleViewModel.IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!userRoleViewModel.IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
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
            return await _userManager.Users.ToListAsync();
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<List<string>> GetUserClaimsAsync(ApplicationUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            return claims.Select(c => c.Value).ToList();
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }


        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                // User not found, handle accordingly
                return false;
            }

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }
    }
}







































































































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

