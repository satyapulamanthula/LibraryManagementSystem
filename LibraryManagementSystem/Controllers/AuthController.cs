using Microsoft.AspNetCore.Mvc;
using LibraryManagementSystem.SharedModels.Models;
using LibraryManagementSystem.Services.BussinessServices.Services;
using LibraryManagementSystem.Services.BussinessServices.IServices;
using LibraryManagementSystem.Repository.IRepositories;
using Microsoft.AspNetCore.Identity;
using LibraryManagementSystem.Repository.Repositories;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data.Entities;

public class AuthController : Controller
{
    private readonly IAuthenticationService _authenticationService;
    private ILibraryManagementLogger _logger;
    private readonly RoleManager<IdentityRole> _roleManager;


    public AuthController(IAuthenticationService authenticationService, ILibraryManagementLogger libraryManagementLogger, RoleManager<IdentityRole> roleManager)
    {
        _authenticationService = authenticationService;
        _logger = libraryManagementLogger;
        _roleManager = roleManager;
    }

    public IActionResult Login()
    {
        //if (User.Identity.IsAuthenticated)
        //{
        //    return RedirectToAction("Index", "Home");
        //}
        //else
        //{
        //    if (TempData.ContainsKey("SuccessMessage"))
        //    {
        //        ViewBag.SuccessMessage = TempData["SuccessMessage"].ToString();
        //    }

        //    return View();
        //}

        // Check if the user is already authenticated
        if (User.Identity.IsAuthenticated)
        {
            // If the user is authenticated, sign them out
            return RedirectToAction("Logout");
        }
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Get the Result like Email,Password
            var result = await _authenticationService.SignInAsync(model.Email, model.Password, model.RememberMe);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
            _logger.LogError("Email Or Password are not valid");
        }

        // If the login attempt is unsuccessful, add an error message to inform the user
        ViewBag.ErrorMessage = "Invalid email or password.";

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> Register(Regester model)
    {
        if (ModelState.IsValid)
        {
            // Check if a user with the provided email already exists
            var existingUser = await _authenticationService.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                // Display a warning message
                ModelState.AddModelError(string.Empty, "A user with this email already exists.");
                return View("Register", model);
            }

            // Proceed with user registration if the email is not already in use
            var result = await _authenticationService.RegisterAsync(model.Email, model.Password);

            if (result.Succeeded)
            {
                // Get the user
                var user = await _authenticationService.FindByEmailAsync(model.Email);

                // If no role is specified, assign a default role (e.g., "Student")
                var defaultRole = "Student";
                var addToRoleResult = await _authenticationService.AssignRoleAsync(user.Id, defaultRole);

                if (!addToRoleResult.Succeeded)
                {
                    // Handle role assignment failure
                    foreach (var error in addToRoleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    // Rollback user creation if role assignment fails
                    await _authenticationService.DeleteUserAsync(user);
                    return View("Register", model);
                }

                // Set success message
                TempData["SuccessMessage"] = "Registration successful!";
                // Redirect to the "Login" page with a success parameter
                return RedirectToAction("Login", new { email = model.Email, success = true });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View("Register", model);
    }


    public async Task<IActionResult> Logout()
    {
        await _authenticationService.SignOutAsync();

        return RedirectToAction("Login", "Auth");
    }

    //Roles

    [HttpGet]
    public IActionResult CreateRole()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(CreateRoleViewModel roleModel)
    {
        if (ModelState.IsValid)
        {
            var success = await _authenticationService.CreateRole(roleModel?.RoleName);
            if (success)
            {
                return RedirectToAction("ListRols", "Auth");
            }
            else
            {
                ModelState.AddModelError("", "Role Already Exists");
            }
        }

        return View(roleModel);
    }

    [HttpGet]
    public async Task<IActionResult> ListRoles()
    {
        List<IdentityRole> roles = await _authenticationService.GetAllRoles();
        return View(roles);
    }

    [HttpGet]
    public async Task<IActionResult> EditRole(string roleId)
    {
        var model = await _authenticationService.GetRoleWithUsers(roleId);
        if (model == null)
        {
            return View("Error");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditRole(EditRoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _authenticationService.UpdateRole(model.Id, model.RoleName);
            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteRole(string roleId)
    {
        var result = await _authenticationService.DeleteRole(roleId);
        if (result.Succeeded)
        {
            return RedirectToAction("ListRoles");
        }

        // Role deletion failed, handle accordingly
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }

        // Return to the view with errors
        return View("ListRoles", await _roleManager.Roles.ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> EditUsersInRole(string roleId)
    {
        var model = await _authenticationService.GetUsersInRole(roleId);
        if (model == null)
        {
            ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
            return View("NotFound");
        }

        ViewBag.roleId = roleId;
        var role = await _roleManager.FindByIdAsync(roleId);
        ViewBag.RollName = role.Name;

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
    {
        var success = await _authenticationService.EditUsersInRole(model, roleId);
        if (success)
        {
            return RedirectToAction("EditRole", new { roleId = roleId });
        }
        else
        {
            ViewBag.ErrorMessage = $"Failed to edit users in role with Id = {roleId}";
            return View("Error");
        }
    }


    //User

    [HttpGet]
    public async Task<IActionResult> ListUsers()
    {
        var users = await _authenticationService.GetAllUsers();
        return View(users);
    }


    [HttpGet]
    public async Task<IActionResult> EditUser(string userId)
    {
        var user = await _authenticationService.GetUserById(userId);

        if (user == null)
        {
            ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
            return View("NotFound");
        }

        var model = new EditUserViewModel
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            //FirstName = user.FirstName,
            //LastName = user.LastName,
            Claims = await _authenticationService.GetUserClaims(user),
            Roles = await _authenticationService.GetUserRoles(user)
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditUser(EditUserViewModel model)
    {
        var user = await _authenticationService.GetUserById(model.Id);

        if (user == null)
        {
            ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
            return View("NotFound");
        }
        else
        {
            user.Email = model.Email;
            user.UserName = model.UserName;

            bool result = await _authenticationService.UpdateUser(user);

            if (result)
            {
                // Set a success message in TempData
                TempData["SuccessMessage"] = "User updated successfully.";
                return RedirectToAction("ListUsers");
            }
            else
            {
                // In case of any error, stay in the same view and show the model validation error
                ModelState.AddModelError("", "Failed to update user.");
                return View(model);
            }
        }
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        bool isDeleted = await _authenticationService.DeleteUser(userId);

        if (isDeleted)
        {
            // User successfully deleted
            TempData["SuccessMessage"] = "User deleted successfully.";
            return RedirectToAction("ListUsers");
        }
        else
        {
            // User deletion failed or user not found
            ViewBag.ErrorMessage = $"User with Id = {userId} cannot be deleted";
            return View("Error");
        }
    }
}





















































































































































































































































































































































































































































































































































//[HttpPost]
//[ValidateAntiForgeryToken]
//public async Task<IActionResult> Register(Regester model)
//{
//    if (ModelState.IsValid)
//    {
//        var result = await _authenticationService.RegisterAsync(model.Email, model.Password);

//        if (result.Succeeded)
//        {
//            // Get the user
//            var user = await _authenticationService.FindByEmailAsync(model.Email);

//            // Check if the selected role is valid
//            if (!string.IsNullOrEmpty(model.Role))
//            {
//                // Add the user to the specified role using the repository
//                var addToRoleResult = await _authenticationService.AssignRoleAsync(user.Id, model.Role);

//                if (!addToRoleResult.Succeeded)
//                {
//                    // Handle role assignment failure
//                    foreach (var error in addToRoleResult.Errors)
//                    {
//                        ModelState.AddModelError(string.Empty, error.Description);
//                    }

//                    // Rollback user creation if role assignment fails
//                    await _authenticationService.DeleteUserAsync(user);
//                    return View("Register", model);
//                }
//            }

//            // Redirect to the "Login" page with a success parameter
//            return RedirectToAction("Login", new { email = model.Email, success = true });
//        }

//        foreach (var error in result.Errors)
//        {
//            ModelState.AddModelError(string.Empty, error.Description);
//        }
//    }

//    return View("Register", model);
//}



//[HttpGet]
//public async Task<IActionResult> EditRole2(string roleId)
//{
//    //First Get the role information from the database
//    ApplicationRole role = (ApplicationRole)await _roleManager.FindByIdAsync(roleId);
//    if (role == null)
//    {
//        // Handle the scenario when the role is not found
//        return View("Error");
//    }

//    //Populate the EditRoleViewModel from the data retrieved from the database
//    var model = new EditRoleViewModel
//    {
//        Id = role.Id,
//        RoleName = role.Name,
//        //Description = role.Description
//        // You can add other properties here if needed
//    };

//    //Initialize the Users Property to avoid Null Reference Exception while Add the username
//    model.Users = new List<string>();

//    // Retrieve all the Users
//    foreach (var user in _userManager.Users.ToList())
//    {
//        // If the user is in this role, add the username to
//        // Users property of EditRoleViewModel. 
//        // This model object is then passed to the view for display
//        if (await _userManager.IsInRoleAsync(user, role.Name))
//        {
//            model.Users.Add(user.UserName);
//        }
//    }

//    return View(model);
//}

