using AutoMapper;
using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.BLL.ViewModels.Roles;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Xml.Linq;

namespace BlogWebApp.Controllers
{
    /// <summary>
    /// Handling incoming requests from the Roles page
    /// </summary>
    [Authorize(Roles = "Администратор")]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public RoleController(RoleManager<ApplicationRole> roleManager, IMapper mapper, IRoleService roleService)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            var roles = _roleService.GetAll(); 
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
            var role = _roleService.GetAll().FirstOrDefault(o => o.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (role == null) 
            {
                if(!await _roleService.AddAsync(model))
                {
                    return RedirectToAction("SomethingWentWrong", "Home", new { str = "Ошибка при добавлении роли." });
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Такая роль уже есть.");
                return View();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var role = _roleService.Get(id);
            if (role == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найдена роль с ID '{id}'." });
            }

            var model = new EditRoleViewModel(id, role.Name, role.Description); 
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            var id = model.Id;
            var role = _roleService.Get(id);
            if (role == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найдена роль с ID '{id}'." });
            }

            var tags = _roleService.GetAll();
            if (tags.FirstOrDefault(o => o.Name.ToUpper() == model.Name.ToUpper() && o.Id != model.Id) != null)
            {
                ModelState.AddModelError(string.Empty, "Такая роль уже есть.");
                return View(model);
            }

            if (!await _roleService.EditAsync(role, model))
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Ошибка при изменении роли с ID '{id}'." });
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var role = _roleService.Get(id);
            if (role == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найдена роль с ID '{id}'." });
            }

            var model = new DeleteRoleViewModel(id, role.Name, role.Description);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteRoleViewModel model)
        {
            var id = model.Id;
            var role = _roleService.Get(id);
            if (role == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найдена роль с ID '{id}'." });
            }

            if (!await _roleService.DeleteAsync(role, model))
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Ошибка при удалении роли с ID '{id}'." });
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Show(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            if (role == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найдена роль с именем '{name}'." });
            }

            var model = _mapper.Map<ShowRoleViewModel>(role);

            return View(model);
        }
    }
}
