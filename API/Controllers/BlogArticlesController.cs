using AutoMapper;
using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.BLL.ViewModels.BlogArticles;
using BlogWebApp.BLL.ViewModels.Roles;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogArticlesController : Controller
    {
        private readonly IBlogArticleService _blogArticleService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public BlogArticlesController(IBlogArticleService blogArticleService, UserManager<User> userManager, IMapper mapper)
        {
            _blogArticleService = blogArticleService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("GetAll"), Authorize]
        public async Task<ActionResult<IEnumerable<BlogArticleViewModel>>> GetAll()
        {
            var user = await _userManager.FindByEmailAsync(User?.Identity?.Name);
            if (user == null)
            {
                return BadRequest("Ошибка получения данных пользователя из БД.");
            }

            var all_blogArticles = ((BlogArticleService)_blogArticleService).GetAllIncludeTags();
            BlogArticleListViewModel model = new BlogArticleListViewModel(all_blogArticles, user);

            return Ok(model._blogArticles);
        }

        [HttpGet("Get"), Authorize]
        public ActionResult<BlogArticle> Get(string id)
        {
            return Ok(((BlogArticleService)_blogArticleService).Get(id));
        }

        [HttpPost("Add"), Authorize]
        public async Task<IActionResult> Add(CreateBlogArticleViewModel incomingmModel)
        {
            var userId = incomingmModel.UserId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest($"Не удалось загрузить пользователя с ID '{userId}'.");
            }
            else
            {
                var model = _mapper.Map<AddBlogArticle>(incomingmModel);
                model.Tags = incomingmModel.Tags.Where(o => o.IsTagSelected).Select(o => o.Name).ToList();

                if (!await _blogArticleService.AddAsync(model, user))
                {
                    return BadRequest("Ошибка при добавлении статьи.");
                }
            }

            return Ok("Статья успешно добавлена.");
        }

        [HttpPatch("Update"), Authorize]
        public async Task<IActionResult> Update(EditBlogArticleViewModel incomingmModel)
        {
            var model = _mapper.Map<EditBlogArticle>(incomingmModel);
            model.SetTags(incomingmModel.Tags.Where(o => o.IsTagSelected).ToList());

            if (!await _blogArticleService.EditAsync(model))
            {
                return BadRequest("Ошибка при изменении статьи.");
            }

            return Ok("Статья успешно изменена.");
        }

        [HttpDelete("Delete"), Authorize]
        public async Task<IActionResult> Delete(DeleteBlogArticleViewModel incomingmModel)
        {
            var id = incomingmModel.Id;
            var blogArticle = _blogArticleService.Get(id);
            if (blogArticle == null)
            {
                return BadRequest($"Не найдена статья с ID '{id}'.");
             }

            var model = _mapper.Map<DelBlogArticle>(incomingmModel);
            if (!await _blogArticleService.DeleteAsync(model))
            {
                return BadRequest("Ошибка при удалении статьи.");
            }

            return Ok("Статья успешно удалена.");
        }
    }
}
