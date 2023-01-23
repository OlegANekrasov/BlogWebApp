using AutoMapper;
using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.Controllers;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BlogWebApp.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IRepository<BlogArticle> _blogArticlesRepository;
        private readonly IRepository<Comment> _commentsRepository;
        private readonly ILogger<UserService> _logger;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserService(UserManager<User> userManager, 
                           IMapper mapper, 
                           IRepository<BlogArticle> blogArticlesRepository, 
                           IRepository<Comment> commentsRepository,
                           ILogger<UserService> logger,
                           RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _blogArticlesRepository = blogArticlesRepository;
            _commentsRepository = commentsRepository;
            _logger = logger;
            _roleManager = roleManager;
        }

        public async Task<User> FindByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                _logger.LogError($"Не найден пользователь с ID '{id}'.");
            }

            return user;
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal claimsPrincipalUser)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipalUser);
            if (user == null)
            {
                _logger.LogError($"Не найден пользователь с ID '{_userManager.GetUserId(claimsPrincipalUser)}'.");
            }

            return user;
        }

        public async Task<bool> EditUserRolesAsync(ChangeUserRoleViewModel model)
        {
            var userId = model.Id;
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Any())
                {
                    var result = await _userManager.RemoveFromRolesAsync(user, userRoles);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation($"Роли пользователя '{user.Email}' удалены.");
                    }
                    else
                    {
                        _logger.LogError($"Ошибка при удалении ролей пользователя '{user.Email}'.");
                        return false;
                    }
                }

                foreach (var item in model.Roles)
                {
                    if (item.IsRoleAssigned)
                    {
                        var result = await _userManager.AddToRoleAsync(user, item.Name);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation($"Роль '{item.Name}' добавлена пользователю '{user.Email}'.");
                        }
                        else
                        {
                            _logger.LogError($"Ошибка при добавлении роли '{item.Name}' пользователю '{user.Email}'.");
                            return false;
                        }
                    }
                }
            }
            else
            {
                _logger.LogError($"Не найден пользователь с ID '{userId}'.");
                return false;
            }

            return true;
        }

        public async Task<UserViewModel> GetUserViewModelAsync(User user)
        {
            var model = _mapper.Map<UserViewModel>(user);

            var blogArticlesCount = ((BlogArticlesRepository)_blogArticlesRepository).GetAllByUserId(user.Id).Count().ToString();
            var commentsCount = ((CommentsRepository)_commentsRepository).GetAllByUserId(user.Id).Count().ToString();

            model.BlogArticles = $"Статей ({blogArticlesCount})   Комментариев ({commentsCount})";
            
            List<UserRole> listRoles = new List<UserRole>();
            
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var item in roles.OrderBy(o => o))
            {
                listRoles.Add(new UserRole() { Name = item, IsRoleAssigned = false });
            }

            model.Roles = listRoles;

            return model;
        }

        public async Task<List<UserListModel>> CreateUserListModelAsync()
        {
            var users = _userManager.Users;
            var userList = new List<UserListModel>();
            foreach (var user in users)
            {
                string Id = user.Id;
                string? userName = user.FirstName + " " + user.MiddleName + " " + user.LastName;
                string? email = user.Email;
                byte[]? image = user.Image;

                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles.Any())
                {
                    string? roleName = "";
                    bool first = true;
                    foreach (var role in userRoles.OrderBy(o => o))
                    {
                        if (first)
                        {
                            roleName = role;
                            first = false;
                        }
                        else
                        {
                            roleName += (", " + role);
                        }
                    }

                    userList.Add(new UserListModel(Id, userName, email, roleName, image));
                }
            }

            return userList;
        }

        public List<UserListModel> SortOrder(List<UserListModel> userList, string sortOrder)
        {
            switch (sortOrder)
            {
                case "Username":
                    userList = userList.OrderBy(s => s.UserName).ToList();
                    break;
                case "Email":
                    userList = userList.OrderBy(s => s.Email).ToList();
                    break;
                case "RoleName":
                    userList = userList.OrderBy(s => s.RoleName).ToList();
                    break;
                default:
                    userList = userList.OrderBy(s => s.UserName).ToList();
                    break;
            }
            return userList;
        }

        public async Task<bool> DeleteAsync(User user, UserDeleteViewModel model)
        {
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Пользователь '{model.Email}' удален.");
                return true;
            }
            else
            {
                _logger.LogError($"Ошибка удаления пользователя с ID '{model.Id}'.");
                return false;
            }
        }

        public async Task<bool> UpdateAsync(User user, string Email)
        {
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Данные пользователя '{Email}' обновлены.");
                return true;
            }
            else
            {
                _logger.LogError($"Ошибка обновления данных пользователя '{Email}'.");
                return false;
            }
        }


        public async Task<ChangeUserRoleViewModel> CreateChangeUserRoleViewModelAsync(User user, string id)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles;

            var allRoles = new List<string>();
            foreach (var role in roles)
            {
                allRoles.Add(role.Name);
            }

            var useName = user.FirstName + " " + user.MiddleName + " " + user.LastName;
            var model = new ChangeUserRoleViewModel(id, useName, user.Email, userRoles, allRoles);

            return model;
        }
    }
}
