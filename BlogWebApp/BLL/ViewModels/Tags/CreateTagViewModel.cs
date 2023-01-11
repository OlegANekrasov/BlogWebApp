using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.Validation.Tags;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Tags
{
    public class CreateTagViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Введите название")]
        [TagName(ErrorMessage = "Название тега - одно слово.")]
        public string Name { get; set; }

        public string BlogArticleId { get; set; }

        public CreateTagViewModel() { }
        public CreateTagViewModel(string name, string blogArticleId)
        {
            Name = name;
            BlogArticleId = blogArticleId;  
        }
    }
}
