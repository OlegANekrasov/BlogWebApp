using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Services.Interfaces
{
    public interface ICommentService
    {
        IEnumerable<Comment> GetAll();
        Comment Get(string id);
        Task Add(AddComment model);
        Task Edit(EditComment model);
        Task Delete(DelComment model);
    }
}
