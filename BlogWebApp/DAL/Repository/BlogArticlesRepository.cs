using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.EF;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.DAL.Repository
{
    public class BlogArticlesRepository : Repository<BlogArticle>
    {
        private readonly IRepository<Tag> _tagsRepository;

        public BlogArticlesRepository(ApplicationDbContext db, IRepository<Tag> tagsRepository) : base(db)
        {
            _tagsRepository = tagsRepository;
        }

        public async Task Add(AddBlogArticle model, User user)
        {
            var blogArticle = Set.AsEnumerable().FirstOrDefault(x => x.Title == model.Title);
            if(blogArticle == null)
            {
                var item = new BlogArticle()
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = model.Title,
                    Description = model.Description,
                    DateCreation = DateTime.Now,
                    DateChange = DateTime.Now,
                    User = user
                };

                await Create(item);

                string[] tags = model.Tags.Split(',');
                foreach(var tag in tags)
                {
                    var tagItem = new Tag()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = tag.Trim()
                    };

                    tagItem.BlogArticles.Add(item);

                    _tagsRepository.Create(tagItem);
                }
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
