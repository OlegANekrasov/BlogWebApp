using BlogWebApp.BLL.Validation.Tags;
using BlogWebApp.BLL.Validation.Users;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Users
{
    /// <summary>
    /// User Role data to pass to the view
    /// </summary>
    public class ChangeUserRoleViewModel
    {
        [Required]
        [Display(Name = "Идентификатор пользователя")]
        public string Id { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "ФИО")]
        public string? UserName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Роли")]
        [UserRole(ErrorMessage = "Пользователю не назначена роль.")]
        public List<UserRole> Roles { get; set; } = new List<UserRole>();

        public ChangeUserRoleViewModel() { }
        public ChangeUserRoleViewModel(string id, string userName, string email, IList<string> roles, List<string> allRoles) 
        { 
            Id = id;
            UserName = userName;
            Email = email;

            foreach(var role in allRoles.OrderBy(o => o))
            {
                UserRole userRole = new UserRole() { Name = role };
                if(roles.FirstOrDefault(o => o == role) != null)
                {
                    userRole.IsRoleAssigned = true;
                }

                Roles.Add(userRole);
            }
        }
    }
}
