using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.ViewModels.Tags;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Services.Interfaces
{
    /// <summary>
    /// Describes CRUD operations
    /// </summary>
    public interface ITagService
    {
        IEnumerable<Tag> GetAll();
        Tag Get(string id);
        Task<bool> AddAsync(AddTag model);
        Task<bool> EditAsync(EditTag model);
        Task<bool> DeleteAsync(DelTag model);

        ListTagsViewModel GetListTagsViewModel();
        ListTagsViewModel GetListTagsViewModel(User user);
    }
}
