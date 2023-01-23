using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> FindByIdAsync(string id);
        Task<bool> DeleteAsync(User user, UserDeleteViewModel model);
        Task<UserViewModel> GetUserViewModel(User user);
        Task<ChangeUserRoleViewModel> CreateChangeUserRoleViewModelAsync(User user, string id);
        Task<List<UserListModel>> CreateUserListModel();
    }
}
