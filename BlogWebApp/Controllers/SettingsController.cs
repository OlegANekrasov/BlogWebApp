using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.ViewModels;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace BlogWebApp.Controllers
{
    [Authorize(Roles = "Администратор")]
    public class SettingsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IUserService _userService;
        private readonly ILogger<TagController> _logger;

        public SettingsController(UserManager<User> userManager, 
                                  RoleManager<ApplicationRole> roleManager, 
                                  IUserService userService, 
                                  ILogger<TagController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> UserList(int? pageNumber, string sortOrder, string currentFilter, string searchString)
        {
            var users = _userManager.Users;
            List<UserListModel> userList = await _userService.CreateUserListModel(users);

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                userList = userList.Where(s => s.Email.Contains(searchString)).ToList();
            }

            ViewData["CurrentSort"] = sortOrder;
            userList = ((UserService)_userService).SortOrder(userList, sortOrder);

            var pageSize = 5;
            var userQueryable = userList.AsQueryable();
            var model = PaginatedList<UserListModel>.CreateAsync(userQueryable, pageNumber ?? 1, pageSize);

            return View("UserList", model); 
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userRoles = await _userManager.GetRolesAsync(user);
            if (user != null && userRoles.Any())
            {
                var userName = user.FirstName + " " + user.MiddleName + " " + user.LastName;
                var model = new UserDeleteViewModel(id, userName, user.Email, userRoles);

                return View("Delete", model);
            }

            return Redirect("~/Settings/UserList");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteViewModel model)
        {
            var userId = model.Id;
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Пользователь '{model.Email}' удален.");
                }
                else
                {
                    _logger.LogError($"Ошибка удаления пользователя с ID '{userId}'.");
                    return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Ошибка удаления пользователя с ID '{userId}'." });
                }
            }
            else
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось загрузить пользователя с ID '{userId}'." });
            }

            return Redirect("~/Settings/UserList");
        }

        [HttpGet]
        public async Task<IActionResult> SaveRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            { 
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Any())
                {
                    var roles = _roleManager.Roles;

                    var allRoles = new List<string>();
                    foreach (var role in roles)
                    {
                        allRoles.Add(role.Name);
                    }

                    var useName = user.FirstName + " " + user.MiddleName + " " + user.LastName;
                    var model = new ChangeUserRoleViewModel(id, useName, user.Email, userRoles, allRoles);

                    return View("SaveRole", model);
                }
                else
                {
                    return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось определить роли пользователя с ID '{id}'." });
                }
            }
            else
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось определить роли пользователя с ID '{id}'." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveRole(ChangeUserRoleViewModel model)
        {
            var userId = model.Id;
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Any())
                {
                    var result = await _userManager.RemoveFromRolesAsync(user, userRoles);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation($"Роли пользователя '{user.Email}' удалены.");
                    }
                    else
                    {
                        _logger.LogError($"Ошибка при удалении ролей пользователя '{user.Email}'.");
                        return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Ошибка при удалении ролей пользователя '{user.Email}'." });
                    }
                }

                foreach (var item in model.Roles)
                {
                    if(item.IsRoleAssigned)
                    {
                        var result = await _userManager.AddToRoleAsync(user, item.Name);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation($"Роль '{item.Name}' добавлена пользователю '{user.Email}'.");
                        }
                        else
                        {
                            _logger.LogError($"Ошибка при добавлении роли '{item.Name}' пользователю '{user.Email}'.");
                            return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Ошибка при добавлении роли '{item.Name}' пользователю '{user.Email}'." });
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось загрузить пользователя с ID '{userId}'." });
            }

            return Redirect("~/Settings/UserList");
        }
    }
}


