using API.Models;
using AutoMapper;
using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.BLL.ViewModels.Roles;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace API.Controllers
{
    public class RolesController : Controller
    {

        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public RolesController(RoleManager<ApplicationRole> roleManager, IMapper mapper, IRoleService roleService)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _roleService = roleService;
        }

        [HttpGet("Get"), Authorize]
        public async Task<ActionResult<ShowRoleViewModel>> Get(string name)
        {
            var role = await _roleManager.FindByNameAsync(name);
            if (role == null)
            {
                return BadRequest($"Не найдена роль с именем '{name}'.");
            }

            var model = _mapper.Map<ShowRoleViewModel>(role);
            return Ok(model);
        }

        [HttpGet("GetAll"), Authorize]
        public ActionResult<RoleListViewModel> GetAll()
        {
            var roles = _roleService.GetAll();
            var model = new RoleListViewModel(roles);

            return Ok(model);
        }

        [HttpPost("Add"), Authorize]
        public async Task<ActionResult> Add(CreateRoleViewModel model)
        {
            var role = _roleService.GetAll().FirstOrDefault(o => o.Name.ToUpper().Equals(model.Name.ToUpper()));
            if (role == null)
            {
                if (!await _roleService.AddAsync(model))
                {
                    return BadRequest("Ошибка при добавлении роли.");
                }
            }
            else
            {
                return BadRequest("Такая роль уже есть.");
            }

            return Ok("Роль успешно добавлена.");
        }

        [HttpPatch("Update"), Authorize]
        public async Task<IActionResult> Update(EditRoleViewModel model)
        {
            var id = model.Id;
            var role = _roleService.Get(id);
            if (role == null)
            {
                return BadRequest($"Не найдена роль с ID '{id}'.");
            }

            var tags = _roleService.GetAll();
            if (tags.FirstOrDefault(o => o.Name.ToUpper() == model.Name.ToUpper() && o.Id != model.Id) != null)
            {
                return BadRequest("Такая роль уже есть.");
            }

            if (!await _roleService.EditAsync(role, model))
            {
                return BadRequest($"Ошибка при изменении роли с ID '{id}'.");
            }

            return Ok("Роль успешно обновлена."); 
        }

        [HttpDelete("Delete"), Authorize]
        public async Task<IActionResult> Delete(DeleteRoleViewModel model)
        {
            var id = model.Id;
            var role = _roleService.Get(id);
            if (role == null)
            {
                return BadRequest($"Не найдена роль с ID '{id}'.");
            }

            if (!await _roleService.DeleteAsync(role, model))
            {
                return BadRequest($"Ошибка при удалении роли с ID '{id}'.");
            }

            return Ok("Роль успешно удалена.");
        }
    }
}
