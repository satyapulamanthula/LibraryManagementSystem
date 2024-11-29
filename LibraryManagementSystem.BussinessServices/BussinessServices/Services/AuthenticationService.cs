using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagementSystem.Services.BussinessServices.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository AuthenticationRepository;

        public AuthenticationService(IAuthenticationRepository authenticationRepository)
        {
            AuthenticationRepository = authenticationRepository;
        }

        public async Task<SignInResult> SignInAsync(string email, string password, bool rememberMe)
        {
            return await AuthenticationRepository.SignInAsync(email, password, rememberMe);
        }

        public async Task<IdentityResult> RegisterAsync(string email, string password)
        {
            return await AuthenticationRepository.RegisterAsync(email, password);
        }

        public async Task<IdentityResult> AssignRoleAsync(string userId, string role)
        {
            return await AuthenticationRepository.AssignRoleAsync(userId, role);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await AuthenticationRepository.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            return await AuthenticationRepository.DeleteUserAsync(user);
        }

        public async Task SignOutAsync()
        {
            await AuthenticationRepository.SignOutAsync();
        }

        public async Task<bool> CreateRole(string roleName)
        {
            return await AuthenticationRepository.CreateRoleAsync(roleName);
        }

        public async Task<List<IdentityRole>> GetAllRoles()
        {
            return await AuthenticationRepository.GetAllRolesAsync();
        }

        public async Task<EditRoleViewModel> GetRoleWithUsers(string roleId)
        {
            return await AuthenticationRepository.GetRoleWithUsers(roleId);
        }


        public async Task<IdentityResult> UpdateRole(string roleId, string roleName)
        {
            return await AuthenticationRepository.UpdateRoleAsync(roleId, roleName);
        }

        public async Task<IdentityResult> DeleteRole(string roleId)
        {
            return await AuthenticationRepository.DeleteRoleAsync(roleId);
        }

        public async Task<List<UserRoleViewModel>> GetUsersInRole(string roleId)
        {
            return await AuthenticationRepository.GetUsersInRoleAsync(roleId);
        }

        public async Task<bool> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            return await AuthenticationRepository.EditUsersInRoleAsync(model, roleId);
        }

        //Users
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await AuthenticationRepository.GetUsersAsync();
        }
        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await AuthenticationRepository.GetUserByIdAsync(userId);
        }

        public async Task<List<string>> GetUserClaims(ApplicationUser user)
        {
            return await AuthenticationRepository.GetUserClaimsAsync(user);
        }
        public async Task<IList<string>> GetUserRoles(ApplicationUser user)
        {
            return await AuthenticationRepository.GetUserRolesAsync(user);
        }

        public async Task<bool> DeleteUser(string userId)
        {
            return await AuthenticationRepository.DeleteUserAsync(userId);
        }

        public async Task<bool> UpdateUser(ApplicationUser user)
        {
            return await AuthenticationRepository.UpdateUserAsync(user);
        }
    }
}
