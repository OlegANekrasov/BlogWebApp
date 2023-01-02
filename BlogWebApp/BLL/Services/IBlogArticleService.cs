using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Services
{
    public interface IBlogArticleService
    {
        IEnumerable<BlogArticle> GetAll();
        Task<BlogArticle> Get(string id);
        Task Add(AddBlogArticle model, User user);
        Task Edit(EditBlogArticle model);
        Task Delete(DelBlogArticle model);
    }
}
