using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.ViewModels.Roles
{
    /// <summary>
    /// Passing Data to the List View
    /// </summary>
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
