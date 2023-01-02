using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.ViewModels.BlogArticles
{
    public class BlogArticleListViewModel
    {
        public List<BlogArticleViewModel> blogArticles;

        public BlogArticleListViewModel(IEnumerable<BlogArticle> _blogArticles)
        {
            blogArticles = new List<BlogArticleViewModel>();
            foreach(var blogArticle in _blogArticles)
            {
                blogArticles.Add(new BlogArticleViewModel(blogArticle));
            }
        }
    }
}
