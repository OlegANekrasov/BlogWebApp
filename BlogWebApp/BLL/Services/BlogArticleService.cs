using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.ViewModels.BlogArticles;
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

        public async Task Delete(DelBlogArticle model)
        {
            await((BlogArticlesRepository)_blogArticlesRepository).Delete(model);
        }

        public async Task Edit(EditBlogArticle model)
        {
            await ((BlogArticlesRepository)_blogArticlesRepository).Edit(model);
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
    }
}
