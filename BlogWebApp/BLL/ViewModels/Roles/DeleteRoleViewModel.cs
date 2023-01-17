using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Roles
{
    public class DeleteRoleViewModel
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Введите название")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Описание", Prompt = "Введите описание")]
        public string Description { get; set; }

        public DeleteRoleViewModel() { }
        public DeleteRoleViewModel(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
