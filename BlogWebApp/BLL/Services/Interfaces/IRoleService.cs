using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.ViewModels.Roles;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Services.Interfaces
{
    public interface IRoleService
    {
        List<ApplicationRole> GetAll();
        ApplicationRole Get(string id);
        Task<bool> AddAsync(CreateRoleViewModel model);
        Task<bool> EditAsync(ApplicationRole role, EditRoleViewModel model);
        Task<bool> DeleteAsync(ApplicationRole role, DeleteRoleViewModel model);
    }
}
