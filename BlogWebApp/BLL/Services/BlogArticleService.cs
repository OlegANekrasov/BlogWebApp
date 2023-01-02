using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Services
{
    public class BlogArticleService : IBlogArticleService
    {

        public BlogArticleService() 
        { 
        
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
            throw new NotImplementedException();
        }
    }
}
