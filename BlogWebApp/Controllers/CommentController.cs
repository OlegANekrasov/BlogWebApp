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
        private readonly ILogger<TagController> _logger;

        public CommentController(IBlogArticleService blogArticleService, 
                                 ICommentService commentService, 
                                 UserManager<User> userManager, 
                                 IMapper mapper, 
                                 ILogger<TagController> logger)
        {
            _blogArticleService = blogArticleService;
            _commentService = commentService;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }
        
        public async Task<IActionResult> Index(string id, int? pageNumber)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось загрузить пользователя с ID '{_userManager.GetUserId(User)}'." });
            }

            var blogArticle = _blogArticleService.Get(id);
            if (blogArticle == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найдена статья с ID '{id}'." });
            }

            var userId = blogArticle.UserId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось загрузить пользователя с ID '{userId}'." });
            }

            if (pageNumber == null)
            {
                try
                {
                    await ((BlogArticleService)_blogArticleService).IncCountOfVisit(id);
                    _logger.LogInformation($"Пользователь '{currentUser.Email}' просматривает статью '{blogArticle.Title}'.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Ошибка при просмотре статьи '{blogArticle.Title}' пользователем '{currentUser.Email}'.");
                }
            }

            var pageSize = 5;
            BlogArticleViewModel model = await ((CommentService)_commentService).GetBlogArticleViewModel(id, blogArticle, user, pageNumber, pageSize);

            ViewBag.UserEmail = currentUser.Email;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create(string blogArticleId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось загрузить пользователя с ID '{_userManager.GetUserId(User)}'." });
            }

            CreateCommentViewModel model = new CreateCommentViewModel("", blogArticleId, user.Id); 

            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCommentViewModel incomingmModel, IFormFile uploadImage)
        {
            ModelState.Remove("uploadImage");  
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<AddComment>(incomingmModel);

                if (uploadImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await uploadImage.CopyToAsync(memoryStream);
                        model.Image = memoryStream.ToArray();
                    }
                }

                try
                {
                    await _commentService.Add(model);
                    var user = await _userManager.FindByIdAsync(model.UserId);
                    var blogArticle = _blogArticleService.Get(model.BlogArticleId);
                    _logger.LogInformation($"Пользователь '{user.Email}' добавил комментарий к статье '{blogArticle.Title}'.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при добавлении комментария.");
                }
                
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View(incomingmModel);
            }

            return RedirectToAction("Index", new { id = incomingmModel.BlogArticleId });
        }

        [HttpGet]
        public IActionResult Edit(string id, string blogArticleId)
        {
            var comment = _commentService.Get(id);
            if (comment == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найден комментарий с ID '{id}'." });
            }

            EditCommentViewModel model = new EditCommentViewModel(comment.Content, id, blogArticleId, comment.Image);

            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCommentViewModel incomingmModel, IFormFile uploadImage)
        {
            ModelState.Remove("uploadImage");
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<EditComment>(incomingmModel);

                if (uploadImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await uploadImage.CopyToAsync(memoryStream);
                        model.Image = memoryStream.ToArray();
                    }
                }

                try
                {
                    await _commentService.Edit(model);
                    var user = await _userManager.GetUserAsync(User);
                    var blogArticle = _blogArticleService.Get(incomingmModel.BlogArticleId);
                    _logger.LogInformation($"Пользователь '{user.Email}' изменил комментарий к статье '{blogArticle.Title}'.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при добавлении комментария.");
                }
                
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View(incomingmModel);
            }

            return RedirectToAction("Index", new { id = incomingmModel.BlogArticleId });
        }

        [HttpGet]
        public IActionResult Delete(string id, string blogArticleId)
        {
            var comment = _commentService.Get(id);
            if (comment == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найден комментарий с ID '{id}'." });
            }

            DeleteCommentViewModel model = new DeleteCommentViewModel(comment.Content, id, blogArticleId, comment.Image);

            return View("Delete", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteCommentViewModel incomingmModel)
        {
            var model = _mapper.Map<DelComment>(incomingmModel);

            try
            {
                await _commentService.Delete(model);
                var user = await _userManager.GetUserAsync(User);
                var blogArticle = _blogArticleService.Get(incomingmModel.BlogArticleId);
                _logger.LogInformation($"Пользователь '{user.Email}' удалил комментарий к статье '{blogArticle.Title}'.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении комментария.");
            }
                 
            return RedirectToAction("Index", new { id = incomingmModel.BlogArticleId });
        }
    }
}
