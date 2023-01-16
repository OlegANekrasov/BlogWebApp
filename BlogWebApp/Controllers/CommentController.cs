using AutoMapper;
using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.ViewModels.Comments;
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
                return NotFound($"Не найден пользователь с ID '{userId}'.");
            }

            await ((BlogArticleService)_blogArticleService).IncCountOfVisit(id);

            BlogArticleViewModel model = ((CommentService)_commentService).GetBlogArticleViewModel(id, blogArticle, user);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string blogArticleId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Не найден пользователь с ID '{_userManager.GetUserId(User)}'.");
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

        [HttpGet]
        public IActionResult Edit(string id, string blogArticleId)
        {
            var comment = _commentService.Get(id);
            if (comment == null)
            {
                return NotFound($"Не найден комментарий с ID '{id}'.");
            }

            EditCommentViewModel model = new EditCommentViewModel(comment.Content, id, blogArticleId);

            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCommentViewModel incomingmModel)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<EditComment>(incomingmModel);
                await _commentService.Edit(model);
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }

            return RedirectToAction("Index", new { id = incomingmModel.BlogArticleId });
        }

        [HttpGet]
        public IActionResult Delete(string id, string blogArticleId)
        {
            var comment = _commentService.Get(id);
            if (comment == null)
            {
                return NotFound($"Не найден комментарий с ID '{id}'.");
            }

            DeleteCommentViewModel model = new DeleteCommentViewModel(comment.Content, id, blogArticleId);

            return View("Delete", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCommentViewModel incomingmModel)
        {
            var model = _mapper.Map<DelComment>(incomingmModel);
            await _commentService.Delete(model);
            
            return RedirectToAction("Index", new { id = incomingmModel.BlogArticleId });
        }
    }
}
