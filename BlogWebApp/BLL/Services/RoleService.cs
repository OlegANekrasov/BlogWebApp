using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.BLL.ViewModels.Roles;
using BlogWebApp.Controllers;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace BlogWebApp.BLL.Services
{
    /// <summary>
    /// Describes CRUD operations
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<RoleService> _logger;

        public RoleService(RoleManager<ApplicationRole> roleManager, ILogger<RoleService> logger)
        {
            _roleManager = roleManager; 
            _logger = logger;
        }

        public List<ApplicationRole> GetAll()
        {
            return _roleManager.Roles.ToList<ApplicationRole>();
        }

        public ApplicationRole Get(string id)
        {
            return _roleManager.Roles.FirstOrDefault(o => o.Id == id);
        }

        public async Task<bool> AddAsync(CreateRoleViewModel model)
        {
            var result = await _roleManager.CreateAsync(new ApplicationRole(model.Name, model.Description));
            if (result.Succeeded)
            {
                _logger.LogInformation($"Роль '{model.Name}' добавлена.");
                return true;
            }
            else
            {
                _logger.LogError("Ошибка при добавлении роли.");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(ApplicationRole role, DeleteRoleViewModel model)
        {
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Роль '{model.Name}' удалена.");
                return true;
            }
            else
            {
                _logger.LogError($"Ошибка при удалении роли с ID '{model.Id}'.");
                return false;
            }
        }

        public async Task<bool> EditAsync(ApplicationRole role, EditRoleViewModel model)
        {
            var id = model.Id;
            role.Name = model.Name;
            role.Description = model.Description;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Роль '{model.OldName}' изменена на '{model.Name}'.");
                return true;
            }
            else
            {
                _logger.LogError($"Ошибка при изменении роли с ID '{id}'.");
                return false;               
            }
        }
    }
}
