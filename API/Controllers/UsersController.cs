using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService; 
        }
        
        [HttpGet]
        public async Task<ActionResult<List<UserListModel>>> Get()
        {
            return await _userService.CreateUserListModelAsync();
        }
    }
}
