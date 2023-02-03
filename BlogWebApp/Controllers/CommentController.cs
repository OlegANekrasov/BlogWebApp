using AutoMapper;
using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.BLL.ViewModels.Comments;
using BlogWebApp.BLL.ViewModels.Tags;
using BlogWebApp.DAL.Models;
using BlogWebApp.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;

namespace BlogWebApp.Controllers
{
    /// <summary>
    /// Handling incoming requests from the Comments page
    /// </summary>
    [Authorize]
    public class CommentController : Controller
    {
        private readonly IBlogArticleService _blogArticleService;
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IHubContext<BlogHub> _hubContext;

        public CommentController(IBlogArticleService blogArticleService, 
                                 ICommentService commentService, 
                                 UserManager<User> userManager, 
                                 IMapper mapper,
                                 IHubContext<BlogHub> hubContext)
        {
            _blogArticleService = blogArticleService;
            _commentService = commentService;
            _userManager = userManager;
            _mapper = mapper;
            _hubContext = hubContext;
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
                if(!await ((BlogArticleService)_blogArticleService).IncCountOfVisitAsync(id, currentUser.Email))
                {
                    return RedirectToAction("SomethingWentWrong", "Home", 
                            new { str = $"Ошибка при просмотре статьи '{blogArticle.Title}' пользователем '{currentUser.Email}'." });
                }
            }

            var pageSize = 5;
            BlogArticleCommentsViewModel model = await ((CommentService)_commentService).GetBlogArticleViewModel(id, blogArticle, user, pageNumber, pageSize);

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
            var model = _mapper.Map<AddComment>(incomingmModel);

            ModelState.Remove("uploadImage");  
            if (ModelState.IsValid)
            {
                if (uploadImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await uploadImage.CopyToAsync(memoryStream);
                        model.Image = memoryStream.ToArray();
                    }
                }

                if(!await _commentService.AddAsync(model))
                {
                    return RedirectToAction("SomethingWentWrong", "Home", new { str = "Ошибка при добавлении комментария." });
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View(incomingmModel);
            }

            var hubModel = await ((CommentService)_commentService).GetSendHubModelAsync(model);
            await _hubContext.Clients.All.SendAsync("NewMessage", hubModel.Content, hubModel.AuthorId, 
                                                                  hubModel.blogArticleId, hubModel.Author, hubModel.DateCreate, hubModel.Image);

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
                
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось загрузить пользователя с ID '{_userManager.GetUserId(User)}'." });
                }

                if (!await _commentService.EditAsync(model, user))
                {
                    return RedirectToAction("SomethingWentWrong", "Home", new { str = "Ошибка при изменении комментария." });
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

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не удалось загрузить пользователя с ID '{_userManager.GetUserId(User)}'." });
            }

            if (!await _commentService.DeleteAsync(model, user))
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = "Ошибка при удалении комментария." });
            }
                 
            return RedirectToAction("Index", new { id = incomingmModel.BlogArticleId });
        }
    }
}
