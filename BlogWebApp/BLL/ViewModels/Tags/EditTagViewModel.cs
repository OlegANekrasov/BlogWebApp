using BlogWebApp.BLL.Validation.Tags;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Tags
{
    public class EditTagViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Введите название")]
        [TagName(ErrorMessage = "Название тега - одно слово.")]
        public string Name { get; set; }

        public string BlogArticleId { get; set; }
        public string Id { get; set; }

        public EditTagViewModel() { }
        public EditTagViewModel(string id, string name, string blogArticleId)
        {
            Id= id;
            Name = name;
            BlogArticleId = blogArticleId;
        }
    }
}
