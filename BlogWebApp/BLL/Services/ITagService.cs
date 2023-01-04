using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Services
{
    public interface ITagService
    {
        IEnumerable<Tag> GetAll();
        Tag Get(string id);
        Task Add(AddTag model);
        Task Edit(EditTag model);
        Task Delete(DelTag model);
    }
}
