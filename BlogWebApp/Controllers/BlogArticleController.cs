using AutoMapper;
using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.ViewModels.BlogArticles;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BlogWebApp.Controllers
{
    [Authorize]
    public class BlogArticleController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IBlogArticleService _blogArticleService;
        private readonly IMapper _mapper;

        public BlogArticleController(UserManager<User> userManager, IBlogArticleService blogArticleService, IMapper mapper)
        {
            _userManager = userManager;
            _blogArticleService = blogArticleService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var all_blogArticles = _blogArticleService.GetAll();
            BlogArticleListViewModel model = new BlogArticleListViewModel(all_blogArticles);

            return View("Index", model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            CreateBlogArticleViewModel model = new CreateBlogArticleViewModel() { UserId = user.Id };

            return View("Create", model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogArticleViewModel incomingmModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(incomingmModel.UserId);

                if (user == null)
                {
                    ModelState.AddModelError("", "Некорректные данные");
                }
                else
                {
                    var model = _mapper.Map<AddBlogArticle>(incomingmModel);
                    await _blogArticleService.Add(model, user);
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var blogArticle = _blogArticleService.Get(id);
            if (blogArticle == null)
            {
                return NotFound($"Не найдена статья с ID '{id}'.");
            }

            var model = _mapper.Map<EditBlogArticleViewModel>(blogArticle);
            ((BlogArticleService)_blogArticleService).SetTagsInModel(model, blogArticle);

            model.UserId = user.Id;
            
            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogArticleViewModel incomingmModel)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<EditBlogArticle>(incomingmModel);
                await _blogArticleService.Edit(model);
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
            }

            return RedirectToAction("Index");
        }
    }
}
