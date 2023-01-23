using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Services.Interfaces
{
    public interface ICommentService
    {
        IEnumerable<Comment> GetAll();
        Comment Get(string id);
        Task<bool> AddAsync(AddComment model);
        Task<bool> EditAsync(EditComment model, User user);
        Task<bool> DeleteAsync(DelComment model, User user);
    }
}
