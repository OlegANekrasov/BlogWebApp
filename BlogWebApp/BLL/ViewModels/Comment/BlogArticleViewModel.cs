using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Comment
{
    public class BlogArticleViewModel
    {
        public BlogArticleModel blogArticle { get; set; }

        public List<CommentsViewModel> ListComments { get; set; }
    }
}
