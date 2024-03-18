using LibraryManagementSystem.SharedModels.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.BussinessServices.IServices
{
    public interface IAuthenticationService
    {
        Task<SignInResult> SignInAsync(string email, string password, bool rememberMe);
        Task<IdentityResult> RegisterAsync(string email, string password);
        Task<IdentityResult> AssignRoleAsync(string userId, string role);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<IdentityResult> DeleteUserAsync(ApplicationUser user);
        Task SignOutAsync();

        //Roles 
        Task<bool> CreateRole(string roleName);
        Task<List<IdentityRole>> GetAllRoles();
        Task<IdentityResult> UpdateRole(string roleId, string roleName);
        Task<IdentityResult> DeleteRole(string roleId);
        Task<List<UserRoleViewModel>> GetUsersInRole(string roleId);
        Task<bool> EditUsersInRole(List<UserRoleViewModel> model, string roleId);
        Task<EditRoleViewModel> GetRoleWithUsers(string roleId);

        //Users
        Task<List<ApplicationUser>> GetAllUsers();
        Task<ApplicationUser> GetUserById(string userId);
        Task<List<string>> GetUserClaims(ApplicationUser user);
        Task<IList<string>> GetUserRoles(ApplicationUser user);
        Task<bool> UpdateUser(ApplicationUser user);
        Task<bool> DeleteUser(string userId);

    }
}
