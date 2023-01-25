using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Comments
{
    /// <summary>
    /// Implements paging of the data in the view
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BlogArticleViewModel
    {
        public BlogArticleModel blogArticle { get; set; }

        public PaginatedList<CommentsViewModel> PaginatedListComments { get; set; }
    }
}
