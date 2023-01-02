using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Services
{
    public class CommentService : ICommentService
    {
        public CommentService()
        {

        }
        
        public Task Add(AddComment model)
        {
            throw new NotImplementedException();
        }

        public Task Delete(DelComment model)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EditComment model)
        {
            throw new NotImplementedException();
        }

        public Task<Comment> Get(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Comment> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
