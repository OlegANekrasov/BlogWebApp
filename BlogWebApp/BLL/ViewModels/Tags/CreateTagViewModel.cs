using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.Validation.Tags;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Tags
{
    /// <summary>
    /// Data to pass to the Create view
    /// </summary>
    public class CreateTagViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Введите название")]
        [TagName(ErrorMessage = "Название тега - одно слово.")]
        public string Name { get; set; }
    }
}
