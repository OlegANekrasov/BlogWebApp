using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserViewModel> GetUserViewModel(User user);
        Task<List<UserListModel>> CreateUserListModel(IQueryable<User> users);
    }
}
