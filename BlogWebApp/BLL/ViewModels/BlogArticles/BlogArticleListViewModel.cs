using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.ViewModels.BlogArticles
{
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
