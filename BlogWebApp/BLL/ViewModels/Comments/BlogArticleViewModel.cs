using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Comments
{
    public class BlogArticleViewModel
    {
        public BlogArticleModel blogArticle { get; set; }

        public PaginatedList<CommentsViewModel> PaginatedListComments { get; set; }
    }
}
