﻿using BlogWebApp.BLL.Services;
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

        public TagsController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet, Authorize]
        public ActionResult<IEnumerable<TagViewModel>> Get()
        {
            return new ObjectResult(_tagService.GetListTagsViewModel()._tags);
        }

        [HttpGet("GetTags")]
        public ActionResult<IEnumerable<TagViewModel>> GetTags()
        {
            return new ObjectResult(_tagService.GetListTagsViewModel()._tags);
        }
    }
}
