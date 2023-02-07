using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.BLL.ViewModels;
using BlogWebApp.BLL.ViewModels.Comments;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.Controllers;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using System.Drawing.Printing;

namespace BlogWebApp.BLL.Services
{
    /// <summary>
    /// Describes CRUD operations
    /// </summary>
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _commentsRepository;
        private readonly IBlogArticleService _blogArticleService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CommentService> _logger;

        public CommentService(IRepository<Comment> commentsRepository, 
                              IBlogArticleService blogArticleService, 
                              UserManager<User> userManager, 
                              ILogger<CommentService> logger)
        {
            _commentsRepository = commentsRepository;
            _blogArticleService = blogArticleService;
            _userManager = userManager;
            _logger = logger;
        }
        
        public async Task<bool> AddAsync(AddComment model)
        {
            try
            {
                await ((CommentsRepository)_commentsRepository).AddAsync(model);
                
                var user = await _userManager.FindByIdAsync(model.UserId);
                var blogArticle = _blogArticleService.Get(model.BlogArticleId);

                _logger.LogInformation($"Пользователь '{user.Email}' добавил комментарий к статье '{blogArticle.Title}'.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении комментария.");
                return false;
            }
        }

        public async Task<bool> EditAsync(EditComment model, User user)
        {
            try
            {
                await ((CommentsRepository)_commentsRepository).EditAsync(model);

                var blogArticle = _blogArticleService.Get(model.BlogArticleId);

                _logger.LogInformation($"Пользователь '{user.Email}' изменил комментарий к статье '{blogArticle.Title}'.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при изменении комментария.");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(DelComment model, User user)
        {
            try
            {
                await ((CommentsRepository)_commentsRepository).DeleteAsync(model);

                var blogArticle = _blogArticleService.Get(model.BlogArticleId);

                _logger.LogInformation($"Пользователь '{user.Email}' удалил комментарий к статье '{blogArticle.Title}'.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении комментария.");
                return false;
            }
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

        public async Task<List<CommentsViewModel>> GetListCommentsViewModelAsync(string id)
        {
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
                        Image = comment.Image,
                        Author = (await _userManager.FindByIdAsync(comment.UserId)).Email,
                        AuthorId = comment.UserId,
                        DateChange = comment.DateChange.ToString("dd.MM.yyyy HH:mm"),
                        blogArticleId = id,
                    });
                }
            }

            return listComments;
        }

        public async Task<BlogArticleCommentsViewModel> GetBlogArticleViewModel(string id, BlogArticle blogArticle, User author, int? pageNumber, int pageSize)
        {
            BlogArticleCommentsViewModel model = new BlogArticleCommentsViewModel()
            {
                blogArticle = new BlogArticleModel()
                {
                    Id = id,
                    Title = blogArticle.Title,
                    Description = blogArticle.Description,
                    Images = blogArticle.Images,
                    Tags = ((BlogArticleService)_blogArticleService).SetTagsInModel(blogArticle.Tags),
                    UserName = author.UserName,
                    UserId = author.Id,
                    DateCreation = blogArticle.DateCreation.ToString("dd.MM.yyyy")
                }
               
            };

            var listComments = await GetListCommentsViewModelAsync(id); 

            var userQueryable = listComments.AsQueryable();
            if(pageNumber == null)
            {
                var count = userQueryable.Count();
                pageNumber = (int)Math.Ceiling(count / (double)pageSize);
            }

            model.PaginatedListComments = PaginatedList<CommentsViewModel>.CreateAsync(userQueryable, (int)pageNumber, pageSize);

            return model;
        }

        public async Task<SendHubModel> GetSendHubModelAsync(AddComment comment)
        {
            var model = new SendHubModel()
            {
                Content = comment.Content,
                AuthorId = comment.UserId,
                Author = (await _userManager.FindByIdAsync(comment.UserId)).Email,
                blogArticleId = comment.BlogArticleId,
                DateCreate = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
            };

            if(comment.Image != null && comment.Image.Length > 0)
            {
                model.Image = Convert.ToBase64String(comment.Image);
            }

            return model;
        }
    }
}
