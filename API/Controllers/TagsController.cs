using AutoMapper;
using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.BLL.ViewModels.Tags;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.EF;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagsController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public TagsController(ITagService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }

        [HttpGet("GetAll"), Authorize]
        public ActionResult<IEnumerable<TagViewModel>> GetAll()
        {
            return Ok(new ObjectResult(_tagService.GetListTagsViewModel()._tags));
        }

        [HttpGet("Get"), Authorize]
        public ActionResult<IEnumerable<Tag>> Get(string id)
        {
            var tag = _tagService.Get(id);
            if (tag == null)
            {
                return BadRequest($"Не найден тег с ID '{id}'.");
            }

            return Ok(tag);
        }

        [HttpPost("Add"), Authorize]
        public async Task<IActionResult> Add(CreateTagViewModel incomingmModel)
        {
            if (!ModelState.IsValid)
            {
                return View(incomingmModel);
            }

            var tags = _tagService.GetAll().ToList();
            if (tags.FirstOrDefault(o => o.Name.ToUpper() == incomingmModel.Name.ToUpper()) != null)
            {
                return BadRequest("Такой тег уже есть.");
            }

            var model = _mapper.Map<AddTag>(incomingmModel);
            if (!await _tagService.AddAsync(model))
            {
                return BadRequest("Ошибка при добавлении тега.");
            }

            return Ok("Тег успешно добавлен.");
        }

        [HttpPatch("Update"), Authorize]
        public async Task<IActionResult> Update(EditTagViewModel incomingmModel)
        {
            if (!ModelState.IsValid)
            {
                return View(incomingmModel);
            }

            var tags = _tagService.GetAll().ToList();
            if (tags.FirstOrDefault(o => o.Name.ToUpper() == incomingmModel.Name.ToUpper() && o.Id != incomingmModel.Id) != null)
            {
                return BadRequest("Такой тег уже есть.");
            }

            var model = _mapper.Map<EditTag>(incomingmModel);
            if (!await _tagService.EditAsync(model))
            {
                return BadRequest("Ошибка при изменении тега.");
            }

            return Ok("Тег успешно изменен.");
        }

        [HttpDelete("Delete"), Authorize]
        public async Task<IActionResult> Delete(DeleteTagViewModel incomingmModel)
        {
            var id = incomingmModel.Id;
            var tag = _tagService.Get(id);
            if (tag == null)
            {
                return BadRequest($"Не найден тег с ID '{id}'.");
            }

            var model = _mapper.Map<DelTag>(incomingmModel);
            if (!await _tagService.DeleteAsync(model))
            {
                return BadRequest("Ошибка при удалении тега.");
            }

            return Ok("Тег успешно удален.");
        }
    }
}
