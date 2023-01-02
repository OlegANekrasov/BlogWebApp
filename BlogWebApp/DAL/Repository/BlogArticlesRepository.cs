using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.EF;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.DAL.Repository
{
    public class BlogArticlesRepository : Repository<BlogArticle>
    {
        public BlogArticlesRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task Add(AddBlogArticle model, User user)
        {
            var blogArticle = Set.AsEnumerable().FirstOrDefault(x => x.Title == model.Title);
            if(blogArticle == null)
            {
                var item = new BlogArticle()
                {
                    Title = model.Title,
                    Description = model.Description,
                    DateCreation = DateTime.Now,
                    DateChange = DateTime.Now,
                    User = user
                };

                await Create(item);
            }
        }

        public async Task Edit(EditBlogArticle model)
        {
            var blogArticle = await Get(model.Id);    
            if (blogArticle != null)
            {
                var edit = false;
                
                if(blogArticle.Title != model.Title)
                {
                    blogArticle.Title = model.Title;
                    edit = true;
                }

                if (blogArticle.Description != model.Description)
                {
                    blogArticle.Description = model.Description;
                    edit = true;
                }

                if(edit)
                {
                    blogArticle.DateChange = DateTime.Now;
                    await Update(blogArticle);
                }
            }
        }

        public async Task Delete(DelBlogArticle model)
        {
            var blogArticle = await Get(model.Id);
            if (blogArticle != null)
            {
                await Delete(blogArticle);
            }
        }

        public IEnumerable<BlogArticle> GetAll()
        {
            return GetAll();
        }

        public IEnumerable<BlogArticle> GetAllByUserId(string userId)
        {
            return GetAll().Where(o => o.UserId == userId);
        }

        public BlogArticle GetById(string id)
        {
            return GetAll().FirstOrDefault(o => o.Id == id);
        }
    }
}
