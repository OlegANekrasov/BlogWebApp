using AutoMapper;
using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.ViewModels.Comment;
using BlogWebApp.BLL.ViewModels.Tags;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebApp.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly IBlogArticleService _blogArticleService;
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public CommentController(IBlogArticleService blogArticleService, ICommentService commentService, UserManager<User> userManager, IMapper mapper)
        {
            _blogArticleService = blogArticleService;
            _commentService = commentService;
            _userManager = userManager;
            _mapper = mapper;
        }
        
        public async Task<IActionResult> Index(string id)
        {
            var blogArticle = _blogArticleService.Get(id);
            if (blogArticle == null)
            {
                return NotFound($"Не найдена статья с ID '{id}'.");
            }

            var userId = blogArticle.UserId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Не найден пользователь с ID '{id}'.");
            }

            BlogArticleViewModel model = new BlogArticleViewModel()
            {
                blogArticle = new BlogArticleModel()
                {
                    Id= id,
                    Title = blogArticle.Title,
                    Description = blogArticle.Description,
                    Tags = ((BlogArticleService)_blogArticleService).SetTagsInModel(blogArticle.Tags),
                    UserName = user.UserName,
                    DateCreation = blogArticle.DateCreation.ToString("dd.MM.yyyy")
                },

                ListComments = new List<CommentsViewModel>()
            };

            var listComments = ((CommentService)_commentService).GetAllByBlogArticleId(id);
            if(listComments != null)
            {
                foreach(var comment in listComments)
                {
                    model.ListComments.Add(new CommentsViewModel()
                    {
                        Id= comment.Id,
                        Content = comment.Content,
                        Author = comment.User?.Email,
                        DateChange = comment.DateChange.ToString("dd.MM.yyyy")
                    });
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string blogArticleId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            CreateCommentViewModel model = new CreateCommentViewModel("", blogArticleId, user.Id); 

            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentViewModel incomingmModel)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<AddComment>(incomingmModel);
                await _commentService.Add(model);
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }

            return RedirectToAction("Index", new { id = incomingmModel.BlogArticleId });
        }

    }
}
