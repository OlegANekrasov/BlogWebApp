using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Comments
{
    /// <summary>
    /// Article data to pass to the view
    /// </summary>
    public class BlogArticleModel
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
        [Display(Name = "Теги")]
        public string Tags { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Автор")]
        public string UserName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Дата создания")]
        public string DateCreation { get; set; }
    }
}
