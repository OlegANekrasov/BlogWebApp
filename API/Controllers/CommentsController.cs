using AutoMapper;
using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.BLL.ViewModels.Comments;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CommentsController(ICommentService commentService, IMapper mapper, UserManager<User> userManager)
        {
            _commentService = commentService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("Get"), Authorize]
        public ActionResult<EditCommentViewModel> Get(string id)
        {
            var model = _mapper.Map<EditCommentViewModel>(_commentService.Get(id));
            return Ok(model);
        }

        [HttpGet("GetAll"), Authorize]  
        public async Task<ActionResult<IEnumerable<CommentsViewModel>>> GetAll(string blogArticleId)
        {
            var model = await ((CommentService)_commentService).GetListCommentsViewModelAsync(blogArticleId);
            return Ok(model);
        }

        [HttpPost("Add"), Authorize]
        public async Task<IActionResult> Add(CreateCommentViewModel incomingmModel)
        {
            var model = _mapper.Map<AddComment>(incomingmModel);

            ModelState.Remove("uploadImage");
            if (ModelState.IsValid)
            {
                if (!await _commentService.AddAsync(model))
                {
                    return BadRequest("Ошибка при добавлении комментария.");
                }
            }
            else
            {
                return BadRequest("Некорректные данные");
            }

            return Ok("Комментарий успешно добавлен.");
        }

        [HttpPatch("Update"), Authorize]
        public async Task<IActionResult> Update(EditCommentViewModel incomingmModel)
        {
            ModelState.Remove("uploadImage");
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<EditComment>(incomingmModel);

                var user = await _userManager.FindByEmailAsync(User?.Identity?.Name);
                if (user == null)
                {
                    return BadRequest("Ошибка получения данных пользователя из БД.");
                }

                if (!await _commentService.EditAsync(model, user))
                {
                    return BadRequest("Ошибка при изменении комментария.");
                }
            }
            else
            {
                return BadRequest("Некорректные данные");
            }

            return Ok("Комментарий успешно изменен.");
        }

        [HttpDelete("Delete"), Authorize]
        public async Task<IActionResult> Delete(DeleteCommentViewModel incomingmModel)
        {
            var model = _mapper.Map<DelComment>(incomingmModel);

            var user = await _userManager.FindByEmailAsync(User?.Identity?.Name);
            if (user == null)
            {
                return BadRequest("Ошибка получения данных пользователя из БД.");
            }

            if (!await _commentService.DeleteAsync(model, user))
            {
                return BadRequest("Ошибка при удалении комментария.");
            }

            return Ok("Комментарий успешно удален.");
        }
    }
}
