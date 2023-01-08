using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Comments
{
    public class CreateCommentViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Комментарий", Prompt = "Введите комментарий")]
        public string Content { get; set; }

        public string BlogArticleId { get; set; }
        public string UserId { get; set; }

        public CreateCommentViewModel() { }
        public CreateCommentViewModel(string name, string blogArticleId, string userId)
        {
            Content = name;
            BlogArticleId = blogArticleId;
            UserId = userId;
        }
    }
}
