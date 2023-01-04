using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.BlogArticles
{
    public class DeleteBlogArticleViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Теги статей")]
        public string Tags { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Автор")]
        public string UserName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Дата создания")]
        public string DateCreation { get; set; }
    }
}
