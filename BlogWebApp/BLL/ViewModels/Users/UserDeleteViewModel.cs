using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Users
{
    public class UserDeleteViewModel
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
        public string? Roles { get; set; }

        public UserDeleteViewModel() { }
        public UserDeleteViewModel(string id, string? userName, string? email, IList<string> roles)
        {
            Id = id;
            UserName = userName;
            Email = email;
            
            bool first = true;
            foreach (var item in roles.OrderBy(o => o))
            {
                if(first)
                {
                    first = false;
                    Roles = item;
                }
                else
                {
                    Roles += (", " + item);
                }
            }
        }
    }
}
