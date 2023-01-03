using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Core.Types;

namespace BlogWebApp.BLL.Services
{
    public class BlogArticleService : IBlogArticleService
    {
        private readonly IRepository<BlogArticle> _blogArticlesRepository;
        private readonly UserManager<User> _userManager;

        public BlogArticleService(UserManager<User> userManager, IRepository<BlogArticle> blogArticlesRepository) 
        {
            _userManager = userManager;
            _blogArticlesRepository = blogArticlesRepository;
        }

        public async Task Add(AddBlogArticle model, User user)
        {
            await ((BlogArticlesRepository)_blogArticlesRepository).Add(model, user);
        }

        public Task Delete(DelBlogArticle model)
        {
            throw new NotImplementedException();
        }

        public Task Edit(EditBlogArticle model)
        {
            throw new NotImplementedException();
        }

        public Task<BlogArticle> Get(string id)
        {
            throw new NotImplementedException();
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

        private async Task<User> FindByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
    }
}
