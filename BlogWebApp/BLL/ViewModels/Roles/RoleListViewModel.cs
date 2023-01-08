using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.ViewModels.Roles
{
    public class RoleListViewModel
    {
        public List<RoleViewModel> RoleList { get; set; } = new List<RoleViewModel>();
        
        public RoleListViewModel(List<ApplicationRole> roles) 
        {
            foreach (var role in roles)
            {
                RoleList.Add(new RoleViewModel(role.Id, role.Name, role.Description));
            }
        }
    }
}
