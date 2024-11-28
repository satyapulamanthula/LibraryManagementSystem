using LibraryManagementSystem.SharedModels.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Repository.IRepositories
{
    public interface IAuthenticationRepository
    {
        Task<SignInResult> SignInAsync(string email, string password, bool rememberMe);
        Task<IdentityResult> RegisterAsync(string email, string password);
        Task<IdentityResult> AssignRoleAsync(string userId, string role);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<IdentityResult> DeleteUserAsync(ApplicationUser user);
        Task SignOutAsync();

        //Roles
        Task<bool> CreateRoleAsync(string roleName);
        Task<List<IdentityRole>> GetAllRolesAsync();
        Task<IdentityResult> UpdateRoleAsync(string roleId, string roleName);
        Task<IdentityResult> DeleteRoleAsync(string roleId);
        Task<List<UserRoleViewModel>> GetUsersInRoleAsync(string roleId);
        Task<bool> EditUsersInRoleAsync(List<UserRoleViewModel> model, string roleId);
        Task<EditRoleViewModel> GetRoleWithUsers(string roleId);

        //Usres
        Task<List<ApplicationUser>> GetUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(string userId);
        Task<List<string>> GetUserClaimsAsync(ApplicationUser user);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(string userId);
    }
}
