using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Services.Interfaces
{
    public interface IBlogArticleService
    {
        IEnumerable<BlogArticle> GetAll();
        BlogArticle Get(string id);
        Task<bool> AddAsync(AddBlogArticle model, User user);
        Task<bool> EditAsync(EditBlogArticle model);
        Task<bool> DeleteAsync(DelBlogArticle model);
    }
}
