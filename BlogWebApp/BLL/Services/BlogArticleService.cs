using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.BLL.ViewModels.BlogArticles;
using BlogWebApp.Controllers;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using NuGet.Protocol.Core.Types;

namespace BlogWebApp.BLL.Services
{
    /// <summary>
    /// Describes CRUD operations
    /// </summary>
    public class BlogArticleService : IBlogArticleService
    {
        private readonly IRepository<BlogArticle> _blogArticlesRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<BlogArticleService> _logger;

        public BlogArticleService(UserManager<User> userManager, IRepository<BlogArticle> blogArticlesRepository, ILogger<BlogArticleService> logger) 
        {
            _userManager = userManager;
            _blogArticlesRepository = blogArticlesRepository;
            _logger = logger;
        }

        public async Task<bool> AddAsync(AddBlogArticle model, User user)
        {
            try
            {
                await ((BlogArticlesRepository)_blogArticlesRepository).Add(model, user);
                
                _logger.LogInformation($"Статья '{model.Title}' добавлена.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении статьи.");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(DelBlogArticle model)
        {
            try
            {
                await ((BlogArticlesRepository)_blogArticlesRepository).Delete(model);
                
                _logger.LogInformation($"Статья '{model.Title}' удалена.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении статьи.");
                return false;
            }
        }

        public async Task<bool> EditAsync(EditBlogArticle model)
        {
            try
            {
                await ((BlogArticlesRepository)_blogArticlesRepository).Edit(model);
                
                _logger.LogInformation($"Статья '{model.Title}' изменена.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при изменении статьи.");
                return false;
            }           
        }

        public BlogArticle Get(string id)
        {
            return ((BlogArticlesRepository)_blogArticlesRepository).GetById(id);
        }

        public IEnumerable<BlogArticle> GetAll()
        {
            var all_blogArticles =_blogArticlesRepository.GetAll();
            foreach (var blogArticle in all_blogArticles)
            {
                blogArticle.User = FindByIdAsync(blogArticle.UserId).Result;
            }

            return all_blogArticles;
        }

        public IEnumerable<BlogArticle> GetAllIncludeTags()
        {
            var all_blogArticles = ((BlogArticlesRepository)_blogArticlesRepository).GetAllIncludeTags();
            foreach (var blogArticle in all_blogArticles)
            {
                blogArticle.User = FindByIdAsync(blogArticle.UserId).Result;
            }

            return all_blogArticles;
        }

        private async Task<User> FindByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public string SetTagsInModel(List<Tag> tags)
        {
            bool first = true;
            string tagStr = "";
            foreach (var tag in tags)
            {
                if (first)
                {
                    tagStr += tag.Name;
                    first = false;
                }
                else
                {
                    tagStr += (", " + tag.Name);
                }
            }

            return tagStr;
        }

        public async Task<bool> IncCountOfVisitAsync(string id, string Email)
        {
            var blogArticle = ((BlogArticlesRepository)_blogArticlesRepository).GetById(id);

            if (blogArticle != null)
            {
                var countOfVisit = blogArticle.CountOfVisit ?? 0;
                blogArticle.CountOfVisit = ++countOfVisit;

                try
                {
                    await ((BlogArticlesRepository)_blogArticlesRepository).IncCountOfVisit(blogArticle);
                    
                    _logger.LogInformation($"Пользователь '{Email}' просматривает статью '{blogArticle.Title}'.");
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Ошибка при просмотре статьи '{blogArticle.Title}' пользователем '{Email}'.");
                    return false;
                }               
            }
            return false;
        }

        public IEnumerable<BlogArticle> SortOrder(IEnumerable<BlogArticle> blogArticle, string sortOrder)
        {
            switch (sortOrder)
            {
                case "Title":
                    blogArticle = blogArticle.OrderBy(s => s.Title).ToList();
                    break;
                case "Author":
                    blogArticle = blogArticle.OrderBy(s => s.User?.Email).ToList();
                    break;
                case "DateCreation":
                    blogArticle = blogArticle.OrderByDescending(s => s.DateCreation).ToList();
                    break;
                default:
                    blogArticle = blogArticle.OrderByDescending(s => s.DateCreation).ToList();
                    break;
            }
            return blogArticle;
        }
    }
}
