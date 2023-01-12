using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Services
{
    public interface IUserService
    {
        Task<UserViewModel> GetUserViewModel(User user);
    }
}
