using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.BlogArticles
{
    public class EditBlogArticleViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Введите название")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Описание", Prompt = "Введите описание")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Теги статей (через запятую)", Prompt = "Введите теги статей")]
        public string Tags { get; set; }

        public string Id { get; set; }
        public string UserId { get; set; }
    }
}
