using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;

namespace BlogWebApp.BLL.Services
{
    public class BlogArticleService : IBlogArticleService
    {
        BlogArticlesRepository blogArticlesRepository;

        public BlogArticleService(BlogArticlesRepository _blogArticlesRepository) 
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
