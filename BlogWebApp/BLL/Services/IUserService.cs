using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Services
{
    public interface IUserService
    {
        UserViewModel GetUserViewModel(User user);
    }
}
