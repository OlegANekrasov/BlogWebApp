using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Services
{
    public class TagService : ITagService
    {
        public TagService()
        {

        }
        
        public Task Add(AddTag model)
        {
            throw new NotImplementedException();
        }

        public Task Delete(DelTag model)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EditTag model)
        {
            throw new NotImplementedException();
        }

        public Task<Tag> Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tag> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
