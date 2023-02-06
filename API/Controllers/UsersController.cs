using API.Models;
using BlogWebApp.BLL.Services;
using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public UsersController(IUserService userService, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _userService = userService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(Login login)
        {
            try
            {
                if (string.IsNullOrEmpty(login.UserName) || string.IsNullOrEmpty(login.Password))
                {
                    return BadRequest("Username and/or Password not specified");
                }

                var result = await _signInManager.PasswordSignInAsync(login.UserName, login.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, login.UserName),
                        new Claim(ClaimTypes.Name, login.Password)
                    };

                    var jwtSecurityToken = new JwtSecurityToken(
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                    return Ok(tokenString);
                }
            }
            catch
            {
                return BadRequest ("An error occurred in generating the token");
            }

            return Unauthorized();
        }

        [HttpPost, Route("register")]
        public async Task<ActionResult<User>> Register(Register register)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = register.Email,
                    Email = register.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "пользователь");

                    return Ok(user);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return BadRequest("An error occurred while registering the user");
        }

        [HttpGet("GetAll"), Authorize]
        public async Task<ActionResult<List<UserListModel>>> GetAll()
        {
            var user = await _userManager.FindByEmailAsync(User?.Identity?.Name);        
            if (user == null)
            {
                return BadRequest("Ошибка получения данных пользователя из БД.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if(userRoles == null)
            {
                return BadRequest("Ошибка получения данных пользователя из БД.");
            }

            if(userRoles.FirstOrDefault(o => o.Equals("Администратор")) != null)
            {
                return await _userService.CreateUserListModelAsync();
            }

            return BadRequest("У пользователя нет необходимой роли.");
        }
    }
}
