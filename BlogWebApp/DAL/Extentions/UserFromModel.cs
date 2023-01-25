using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.DAL.Extentions
{
    /// <summary>
    /// Расширение, которое позволяет получить из вьюмодели модель пользователя
    /// </summary>
    public static class UserFromModel
    {
        /// <summary>
        /// Converts the viewmodel to a user model
        /// </summary>
        /// <param name="user"></param>
        /// <param name="userEditVM"></param>
        /// <returns></returns>
        public static User Convert(this User user, UserEditViewModel userEditVM)
        {
            user.Image = userEditVM.Image;
            user.LastName = userEditVM.LastName;
            user.MiddleName = userEditVM.MiddleName;
            user.FirstName = userEditVM.FirstName;
            user.Email = userEditVM.Email;
            user.BirthDate = userEditVM.BirthDate;
            user.UserName = userEditVM.UserName;
            user.About = userEditVM.About;

            return user;
        }
    }
}
