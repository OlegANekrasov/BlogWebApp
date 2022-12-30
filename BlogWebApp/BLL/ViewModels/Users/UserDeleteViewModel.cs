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
        [Display(Name = "Роль")]
        public string? RoleName { get; set; }
    }
}
