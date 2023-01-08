using BlogWebApp.BLL.ViewModels.Roles;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml.Linq;

namespace BlogWebApp.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList<ApplicationRole>();
            var model = new RoleListViewModel(roles);

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateRoleViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            var role = _roleManager.Roles.FirstOrDefault(o => o.Name == model.Name);
            if (role == null) 
            {
                await _roleManager.CreateAsync(new ApplicationRole(model.Name, model.Description));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var role = _roleManager.Roles.FirstOrDefault(o => o.Id == id);
            if (role == null)
            {
                return NotFound($"Не найдена роль с ID '{id}'.");
            }

            var model = new EditRoleViewModel() 
            {
                Id = id,
                Name = role.Name,
                Description = role.Description
            };

            if (role.Name == "Администратор" || role.Name == "Модератор" || role.Name == "Пользователь")
            {
                model.IsProgramRole = true;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            var id = model.Id;
            var role = _roleManager.Roles.FirstOrDefault(o => o.Id == id);
            if (role == null)
            {
                return NotFound($"Не найдена роль с ID '{id}'.");
            }

            role.Name= model.Name;
            role.Description= model.Description;
            await _roleManager.UpdateAsync(role);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var role = _roleManager.Roles.FirstOrDefault(o => o.Id == id);
            if (role == null)
            {
                return NotFound($"Не найдена роль с ID '{id}'.");
            }

            var model = new DeleteRoleViewModel()
            {
                Id = id,
                Name = role.Name,
                Description = role.Description
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteRoleViewModel model)
        {
            var id = model.Id;
            var role = _roleManager.Roles.FirstOrDefault(o => o.Id == id);
            if (role == null)
            {
                return NotFound($"Не найдена роль с ID '{id}'.");
            }

            await _roleManager.DeleteAsync(role);

            return RedirectToAction("Index");
        }
    }
}
