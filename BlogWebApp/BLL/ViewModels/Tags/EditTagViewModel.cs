using BlogWebApp.BLL.Validation.Tags;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Tags
{
    /// <summary>
    /// Data to pass to the Edit view
    /// </summary>
    public class EditTagViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Введите название")]
        [TagName(ErrorMessage = "Название тега - одно слово.")]
        public string Name { get; set; }

        public string OldName { get; set; }
        public string Id { get; set; }

        public EditTagViewModel() { }
        public EditTagViewModel(string id, string name)
        {
            Id= id;
            Name = name;
            OldName = name;
        }
    }
}
