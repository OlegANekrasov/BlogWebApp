using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.ViewModels;
using BlogWebApp.BLL.ViewModels.Comments;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using System.Drawing.Printing;

namespace BlogWebApp.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _commentsRepository;
        private readonly IBlogArticleService _blogArticleService;
        private readonly UserManager<User> _userManager;

        public CommentService(IRepository<Comment> commentsRepository, IBlogArticleService blogArticleService, UserManager<User> userManager)
        {
            _commentsRepository = commentsRepository;
            _blogArticleService = blogArticleService;
            _userManager = userManager;
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

        public async Task<BlogArticleViewModel> GetBlogArticleViewModel(string id, BlogArticle blogArticle, User author, int pageNumber, int pageSize)
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
                }
               
            };

            var listComments = new List<CommentsViewModel>();

            var listBlogArticleComments = GetAllByBlogArticleId(id);
            if (listBlogArticleComments != null)
            {
                foreach (var comment in listBlogArticleComments.OrderBy(o => o.DateCreation))
                {
                    listComments.Add(new CommentsViewModel()
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        Author = (await _userManager.FindByIdAsync(comment.UserId)).Email,
                        AuthorId = comment.UserId,
                        DateChange = comment.DateChange.ToString("dd.MM.yyyy HH:mm"),
                        blogArticleId = id,
                    });
                }
            }

            var userQueryable = listComments.AsQueryable();
            model.PaginatedListComments = PaginatedList<CommentsViewModel>.CreateAsync(userQueryable, pageNumber, pageSize);

            return model;
        }
    }
}
