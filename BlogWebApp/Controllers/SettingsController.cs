using BlogWebApp.BLL.ViewModels;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogWebApp.Controllers
{
    public class SettingsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

         public SettingsController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
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
                    string? roleName = userRoles.First();
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
                var model = new ChangeUserRoleModel()
                {
                    Id = id,
                    Email = user.Email,
                    UserName = user.FirstName + " " + user.MiddleName + " " + user.LastName,
                    RoleName = userRoles.First()
                };

                var roles = _roleManager.Roles;

                var selectListItem = new List<SelectListItem>();
                foreach (var role in roles)
                {
                    selectListItem.Add(new SelectListItem { Text = role.Name, Value = role.Name });
                }

                ViewBag.Roles = selectListItem;

                return View("SaveRole", model);
            }

            return Redirect("~/Settings/UserList");
        }

        [HttpPost]
        public async Task<IActionResult> SaveRole(ChangeUserRoleModel model)
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

                    await _userManager.AddToRoleAsync(user, model.RoleName);
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


