using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.ViewModels.BlogArticles
{
    /// <summary>
    /// Passing Data to the List View
    /// </summary>
    public class BlogArticleListViewModel
    {
        public List<BlogArticleViewModel> _blogArticles;

        public BlogArticleListViewModel(IEnumerable<BlogArticle> blogArticles, User user)
        {
            _blogArticles = new List<BlogArticleViewModel>();
            foreach(var blogArticle in blogArticles)
            {
                _blogArticles.Add(new BlogArticleViewModel(blogArticle, user));
            }
        }
    }
}
