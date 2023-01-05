using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Comment
{
    public class EditCommentViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Комментарий", Prompt = "Введите комментарий")]
        public string Content { get; set; }

        public string Id { get; set; }
        public string BlogArticleId { get; set; }

        public EditCommentViewModel() { }
        public EditCommentViewModel(string name, string id, string blogArticleId)
        {
            Content = name;
            Id = id;
            BlogArticleId = blogArticleId;
        }
    }
}
