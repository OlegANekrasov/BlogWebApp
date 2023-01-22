using AutoMapper;
using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.BLL.ViewModels.BlogArticles;
using BlogWebApp.BLL.ViewModels.Tags;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BlogWebApp.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<TagController> _logger;

        public TagController(ITagService tagService, UserManager<User> userManager, IMapper mapper, ILogger<TagController> logger)
        {
            _tagService = tagService;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                var errorStr = $"Не удалось загрузить пользователя с ID '{_userManager.GetUserId(User)}'.";
                return RedirectToAction("SomethingWentWrong", "Home", new { str = errorStr });
            }

            var model = _tagService.GetListTagsViewModel(user); 

            return View("Index", model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            CreateTagViewModel model = new CreateTagViewModel();
            
            return View("Create", model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateTagViewModel incomingmModel)
        {
            if (!ModelState.IsValid)
            {
                return View(incomingmModel);
            }

            var tags = _tagService.GetAll().ToList();
            if (tags.FirstOrDefault(o => o.Name.ToUpper() == incomingmModel.Name.ToUpper()) != null)
            {
                ModelState.AddModelError(string.Empty, "Такой тег уже есть.");
                return View(incomingmModel);
            }

            var model = _mapper.Map<AddTag>(incomingmModel);
            try
            {
                await _tagService.Add(model);
                _logger.LogInformation($"Тег '{incomingmModel.Name}' добавлен.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении тега.");
            }           
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var tag = _tagService.Get(id);
            if (tag == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найден тег с ID '{id}'." });
            }

            EditTagViewModel model = new EditTagViewModel(id, tag.Name);

            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagViewModel incomingmModel)
        {
            if (!ModelState.IsValid)
            {
                return View(incomingmModel);
            }

            var tags = _tagService.GetAll().ToList();
            if (tags.FirstOrDefault(o => o.Name.ToUpper() == incomingmModel.Name.ToUpper() && o.Id != incomingmModel.Id) != null)
            {
                ModelState.AddModelError(string.Empty, "Такой тег уже есть.");
                return View(incomingmModel);
            }

            var model = _mapper.Map<EditTag>(incomingmModel);
            try
            {
                await _tagService.Edit(model);
                _logger.LogInformation($"Тег '{incomingmModel.OldName}' изменен на '{incomingmModel.Name}'.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при изменении тега.");
            }
            

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(string id, string blogArticleId = null)
        {
            var tag = _tagService.Get(id);
            if (tag == null)
            {
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найден тег с ID '{id}'." });
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
                return RedirectToAction("SomethingWentWrong", "Home", new { str = $"Не найден тег с ID '{id}'." });
            }

            var model = _mapper.Map<DelTag>(incomingmModel);
            try
            {
                await _tagService.Delete(model);
                _logger.LogInformation($"Тег '{incomingmModel.Name}' удален.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении тега.");
            }

            return RedirectToAction("Index", new { id = incomingmModel.BlogArticleId });
        }
    }
}
