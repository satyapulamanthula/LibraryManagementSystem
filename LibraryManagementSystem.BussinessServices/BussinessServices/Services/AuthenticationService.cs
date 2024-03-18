using LibraryManagementSystem.Repository.IRepositories;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.SharedModels.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Services.BussinessServices.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;

        public AuthenticationService(IAuthenticationRepository authenticationRepository)
        {
            _authenticationRepository = authenticationRepository;
        }

        public async Task<SignInResult> SignInAsync(string email, string password, bool rememberMe)
        {
            return await _authenticationRepository.SignInAsync(email, password, rememberMe);
        }

        public async Task<IdentityResult> RegisterAsync(string email, string password)
        {
            return await _authenticationRepository.RegisterAsync(email, password);
        }

        public async Task<IdentityResult> AssignRoleAsync(string userId, string role)
        {
            return await _authenticationRepository.AssignRoleAsync(userId, role);
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _authenticationRepository.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            return await _authenticationRepository.DeleteUserAsync(user);
        }

        public async Task SignOutAsync()
        {
            await _authenticationRepository.SignOutAsync();
        }

        public async Task<bool> CreateRole(string roleName)
        {
            return await _authenticationRepository.CreateRoleAsync(roleName);
        }

        public async Task<List<IdentityRole>> GetAllRoles()
        {
            return await _authenticationRepository.GetAllRolesAsync();
        }

        public async Task<EditRoleViewModel> GetRoleWithUsers(string roleId)
        {
            return await _authenticationRepository.GetRoleWithUsers(roleId);
        }


        public async Task<IdentityResult> UpdateRole(string roleId, string roleName)
        {
            return await _authenticationRepository.UpdateRoleAsync(roleId, roleName);
        }

        public async Task<IdentityResult> DeleteRole(string roleId)
        {
            return await _authenticationRepository.DeleteRoleAsync(roleId);
        }

        public async Task<List<UserRoleViewModel>> GetUsersInRole(string roleId)
        {
            return await _authenticationRepository.GetUsersInRoleAsync(roleId);
        }

        public async Task<bool> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            return await _authenticationRepository.EditUsersInRoleAsync(model, roleId);
        }

        //Users
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _authenticationRepository.GetUsersAsync();
        }
        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _authenticationRepository.GetUserByIdAsync(userId);
        }

        public async Task<List<string>> GetUserClaims(ApplicationUser user)
        {
            return await _authenticationRepository.GetUserClaimsAsync(user);
        }
        public async Task<IList<string>> GetUserRoles(ApplicationUser user)
        {
            return await _authenticationRepository.GetUserRolesAsync(user);
        }

        public async Task<bool> DeleteUser(string userId)
        {
            return await _authenticationRepository.DeleteUserAsync(userId);
        }

        public async Task<bool> UpdateUser(ApplicationUser user)
        {
            return await _authenticationRepository.UpdateUserAsync(user);
        }
    }
}
