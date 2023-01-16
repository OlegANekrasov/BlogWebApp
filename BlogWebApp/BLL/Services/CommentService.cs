using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.ViewModels.Comments;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;
using Microsoft.AspNetCore.Identity;

namespace BlogWebApp.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _commentsRepository;
        private readonly IBlogArticleService _blogArticleService;

        public CommentService(IRepository<Comment> commentsRepository, IBlogArticleService blogArticleService)
        {
            _commentsRepository = commentsRepository;
            _blogArticleService = blogArticleService;
        }
        
        public async Task Add(AddComment model)
        {
            await ((CommentsRepository)_commentsRepository).Add(model);
        }

        public async Task Delete(DelComment model)
        {
            await((CommentsRepository)_commentsRepository).Delete(model);
        }

        public async Task Edit(EditComment model)
        {
            await ((CommentsRepository)_commentsRepository).Edit(model);
        }

        public Comment Get(string id)
        {
            return ((CommentsRepository)_commentsRepository).GetById(id);
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

        public BlogArticleViewModel GetBlogArticleViewModel(string id, BlogArticle blogArticle, User author)
        {
            BlogArticleViewModel model = new BlogArticleViewModel()
            {
                blogArticle = new BlogArticleModel()
                {
                    Id = id,
                    Title = blogArticle.Title,
                    Description = blogArticle.Description,
                    Tags = ((BlogArticleService)_blogArticleService).SetTagsInModel(blogArticle.Tags),
                    UserName = author.UserName,
                    UserId = author.Id,
                    DateCreation = blogArticle.DateCreation.ToString("dd.MM.yyyy")
                },

                ListComments = new List<CommentsViewModel>()
            };

            var listComments = GetAllByBlogArticleId(id);
            if (listComments != null)
            {
                foreach (var comment in listComments)
                {
                    model.ListComments.Add(new CommentsViewModel()
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        Author = author.Email,
                        AuthorId = comment.UserId,
                        DateChange = comment.DateChange.ToString("dd.MM.yyyy HH:mm")
                    });
                }
            }

            return model;
        }
    }
}
