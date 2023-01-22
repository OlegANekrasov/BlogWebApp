using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.ViewModels.Tags;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Services.Interfaces
{
    public interface ITagService
    {
        IEnumerable<Tag> GetAll();
        Tag Get(string id);
        Task Add(AddTag model);
        Task Edit(EditTag model);
        Task Delete(DelTag model);

        ListTagsViewModel GetListTagsViewModel();
        ListTagsViewModel GetListTagsViewModel(User user);
    }
}
