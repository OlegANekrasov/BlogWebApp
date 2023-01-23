using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.Services.Interfaces;
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
            List<UserListModel> userList = await _userService.CreateUserListModel();

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
                userList = userList.Where(s => s.Email != null && s.Email.Contains(searchString)).ToList();
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
            var user = await _userService.FindByIdAsync(id);
            if (user == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найден пользователь с ID '{id}'." });
            }    
                
            var userRoles = await _userManager.GetRolesAsync(user);
           
            var userName = user.FirstName + " " + user.MiddleName + " " + user.LastName;
            var model = new UserDeleteViewModel(id, userName, user.Email, userRoles);

            return View("Delete", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteViewModel model)
        {
            var userId = model.Id;
            var user = await _userService.FindByIdAsync(userId);
            if (user != null)
            {
                if (! await _userService.DeleteAsync(user, model))
                { 
                    return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Ошибка удаления пользователя с ID '{userId}'." });
                }
            }
            else
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найден пользователь с ID '{userId}'." });
            }

            return Redirect("~/Settings/UserList");
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var user = await _userService.FindByIdAsync(id);
            if (user != null)
            {
                var model = await _userService.CreateChangeUserRoleViewModelAsync(user, id);
                return View("EditRole", model);
            }
            else
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось определить роли пользователя с ID '{id}'." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(ChangeUserRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

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


