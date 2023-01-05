using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;

namespace BlogWebApp.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _commentsRepository;

        public CommentService(IRepository<Comment> commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }
        
        public async Task Add(AddComment model)
        {
            await ((CommentsRepository)_commentsRepository).Add(model);
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
            return ((CommentsRepository)_commentsRepository).GetAll();
        }

        public IEnumerable<Comment> GetAllByBlogArticleId(string blogArticleId)
        {
            var comments = ((CommentsRepository)_commentsRepository).GetAllByBlogArticleId(blogArticleId);
            if(comments != null)
            {
                return comments;
            }

            return null;
        }
    }
}
