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

         public SettingsController(UserManager<User> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> UserList(int? pageNumber, string sortOrder, string currentFilter, string searchString)
        {
            List<UserListModel> userList = new List<UserListModel>();
            var users = _userManager.Users;

            foreach (var user in users) 
            {
                string Id = user.Id;
                string? userName = user.FirstName + " " + user.MiddleName + " " + user.LastName;
                string? email = user.Email;

                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Any())
                {
                    string? roleName = "";
                    bool first = true;
                    foreach (var role in userRoles.OrderBy(o => o))
                    {
                        if(first)
                        {
                            roleName = role;
                            first = false;
                        }
                        else
                        {
                            roleName +=  (", " + role);
                        }
                    }

                    userList.Add(new UserListModel(Id, userName, email, roleName));
                }
            }

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

            switch (sortOrder)
            {
                case "Username":
                    userList = userList.OrderBy(s => s.UserName).ToList();
                    break;
                case "Email":
                    userList = userList.OrderBy(s => s.Email).ToList();
                    break;
                case "RoleName":
                    userList = userList.OrderBy(s => s.RoleName).ToList();
                    break;
                default:
                    userList = userList.OrderBy(s => s.UserName).ToList();
                    break;
            }

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
                var model = new UserDeleteViewModel()
                {
                    Id = id,
                    Email = user.Email,
                    UserName = user.FirstName + " " + user.MiddleName + " " + user.LastName,
                    RoleName = userRoles.First()
                };

                return View("Delete", model);
            }

            return Redirect("~/Settings/UserList");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Некорректные данные");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }

            return Redirect("~/Settings/UserList");
        }

        [HttpGet]
        public async Task<IActionResult> SaveRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userRoles = await _userManager.GetRolesAsync(user);
            if (user != null && userRoles.Any())
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

            return Redirect("~/Settings/UserList");
        }

        [HttpPost]
        public async Task<IActionResult> SaveRole(ChangeUserRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    if (userRoles.Any())
                    {
                        await _userManager.RemoveFromRolesAsync(user, userRoles);
                    }

                    foreach (var item in model.Roles)
                    {
                        if(item.IsRoleAssigned)
                        {
                            await _userManager.AddToRoleAsync(user, item.Name);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }

            return Redirect("~/Settings/UserList");
        }
    }
}


