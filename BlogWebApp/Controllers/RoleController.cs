using AutoMapper;
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
    [Authorize(Roles = "Администратор")]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ILogger<TagController> _logger;

        public RoleController(RoleManager<ApplicationRole> roleManager, IMapper mapper, ILogger<TagController> logger)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _logger = logger;
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
            var role = _roleManager.Roles.ToList().FirstOrDefault(o => o.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (role == null) 
            {
                var result = await _roleManager.CreateAsync(new ApplicationRole(model.Name, model.Description));
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Роль '{model.Name}' добавлена.");
                }   
                else
                {
                    _logger.LogError("Ошибка при добавлении роли.");
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
            var role = _roleManager.Roles.FirstOrDefault(o => o.Id == id);
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
            var role = _roleManager.Roles.FirstOrDefault(o => o.Id == id);
            if (role == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найдена роль с ID '{id}'." });
            }

            role.Name = model.Name;
            role.Description= model.Description;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Роль '{model.OldName}' изменена на '{model.Name}'.");
            }
            else
            {
                _logger.LogError($"Ошибка при изменении роли с ID '{id}'.");
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Ошибка при изменении роли с ID '{id}'." });
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var role = _roleManager.Roles.FirstOrDefault(o => o.Id == id);
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
            var role = _roleManager.Roles.FirstOrDefault(o => o.Id == id);
            if (role == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найдена роль с ID '{id}'." });
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Роль '{model.Name}' удалена.");
            }
            else
            {
                _logger.LogError($"Ошибка при удалении роли с ID '{id}'.");
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
