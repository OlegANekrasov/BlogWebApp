using AutoMapper;
using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.ViewModels.BlogArticles;
using BlogWebApp.BLL.ViewModels.Tags;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogWebApp.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        private readonly IBlogArticleService _blogArticleService;
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public TagController(ITagService tagService, IBlogArticleService blogArticleService, IMapper mapper)
        {
            _tagService = tagService;
            _blogArticleService = blogArticleService;
            _mapper = mapper;
        }

        public IActionResult Index(string id)
        {
            var blogArticle = _blogArticleService.Get(id);
            if (blogArticle == null)
            {
                return NotFound($"Не найдена статья с ID '{id}'.");
            }

            var tags = blogArticle.Tags.ToList();
            var model = new ListTagsViewModel(tags , id);

            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Create(string blogArticleId)
        {
            CreateTagViewModel model = new CreateTagViewModel("", blogArticleId);
            
            return View("Create", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateTagViewModel incomingmModel)
        {
            if (ModelState.IsValid)
            {
                var id = incomingmModel.BlogArticleId;
                var blogArticle = _blogArticleService.Get(id);
                if (blogArticle == null)
                {
                    return NotFound($"Не найдена статья с ID '{id}'.");
                }

                var tags = blogArticle.Tags.ToList();
                if(tags.FirstOrDefault(o => o.Name == incomingmModel.Name) != null)
                {
                    return Problem("Такой тег у статьи уже есть.");
                }

                var model = _mapper.Map<AddTag>(incomingmModel);
                await _tagService.Add(model);
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
            var tag = _tagService.Get(id);
            if (tag == null)
            {
                return NotFound($"Не найден тег с ID '{id}'.");
            }

            EditTagViewModel model = new EditTagViewModel(id, tag.Name, blogArticleId);

            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagViewModel incomingmModel)
        {
            if (ModelState.IsValid)
            {
                var id = incomingmModel.BlogArticleId;
                var blogArticle = _blogArticleService.Get(id);
                if (blogArticle == null)
                {
                    return NotFound($"Не найдена статья с ID '{id}'.");
                }

                var tags = blogArticle.Tags.ToList();
                if (tags.FirstOrDefault(o => o.Name == incomingmModel.Name) != null)
                {
                    return Problem("Такой тег у статьи уже есть.");
                }

                var model = _mapper.Map<EditTag>(incomingmModel);
                await _tagService.Edit(model);
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
            var tag = _tagService.Get(id);
            if (tag == null)
            {
                return NotFound($"Не найден тег с ID '{id}'.");
            }

            DeleteTagViewModel model = new DeleteTagViewModel(id, tag.Name, blogArticleId);

            return View("Delete", model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeleteTagViewModel incomingmModel)
        {
            var id = incomingmModel.Id;
            var tag = _tagService.Get(id);
            if (tag == null)
            {
                return NotFound($"Не найден тег с ID '{id}'.");
            }

            var model = _mapper.Map<DelTag>(incomingmModel);
            await _tagService.Delete(model);

            return RedirectToAction("Index", new { id = incomingmModel.BlogArticleId });
        }
    }
}
