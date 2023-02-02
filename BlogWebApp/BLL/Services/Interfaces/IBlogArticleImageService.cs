using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Services.Interfaces
{
    /// <summary>
    /// Describes CRUD operations
    /// </summary>
    public interface IBlogArticleImageService
    {
        BlogArticleImage Get(string id);
        Task<bool> AddAsync(AddBlogArticleImage model);
        Task<bool> DeleteAsync(DelBlogArticleImage model);
    }
}
