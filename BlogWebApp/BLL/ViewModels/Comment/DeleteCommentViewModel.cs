using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Comment
{
    public class DeleteCommentViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Комментарий")]
        public string Content { get; set; }

        public string Id { get; set; }
        public string BlogArticleId { get; set; }

        public DeleteCommentViewModel() { }
        public DeleteCommentViewModel(string name, string id, string blogArticleId)
        {
            Content = name;
            Id = id;
            BlogArticleId = blogArticleId;
        }
    }
}
