using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Users
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Ваш текущий пароль")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Длина пароля должна быть не менее {2} и не более {1} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повторите пароль")]
        [Compare("NewPassword", ErrorMessage = "Новый пароль и пароль подтверждения не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}
