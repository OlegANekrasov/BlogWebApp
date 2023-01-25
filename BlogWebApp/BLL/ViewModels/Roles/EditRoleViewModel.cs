using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Roles
{
    /// <summary>
    /// Data to pass to the Edit view
    /// </summary>
    public class EditRoleViewModel
    {
        public string Id { get; set; }
        public bool IsProgramRole { get; set; } = false;

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Введите название")]
        public string Name { get; set; }

        public string OldName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Описание", Prompt = "Введите описание")]
        public string Description { get; set; }

        public EditRoleViewModel() { }
        public EditRoleViewModel(string id, string name, string description)
        {
            Id = id;
            Name = name;
            OldName = name;
            Description = description;

            if (Name == "Администратор" || Name == "Модератор" || Name == "Пользователь")
            {
                IsProgramRole = true;
            }
        }
    }
}
