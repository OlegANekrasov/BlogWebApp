using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;
using NuGet.Protocol.Core.Types;

namespace BlogWebApp.BLL.Services
{
    public class BlogArticleService : IBlogArticleService
    {
        IRepository<BlogArticle> blogArticlesRepository;

        public BlogArticleService(IRepository<BlogArticle> _blogArticlesRepository) 
        {
            blogArticlesRepository = _blogArticlesRepository;
        }

        public Task Add(AddBlogArticle model, User user)
        {
            throw new NotImplementedException();
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
            return blogArticlesRepository.GetAll();
        }
    }
}
