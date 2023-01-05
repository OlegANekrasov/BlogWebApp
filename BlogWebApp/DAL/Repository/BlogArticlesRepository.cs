using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.EF;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

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

                List<string> listNewTags = new List<string>();
                string[] tags = model.Tags.Split(',');
                foreach (var tag in tags)
                {
                    listNewTags.Add(tag.Trim());
                }

                await AddTags(listNewTags, item);
            }
        }

        public async Task Edit(EditBlogArticle model)
        {
            var blogArticle = Set.Include(c => c.Tags).FirstOrDefault(o => o.Id == model.Id);    
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

                List<string> listNewTags = new List<string>();
                string[] tags = model.Tags.Split(',');
                foreach (var tag in tags)
                {
                    listNewTags.Add(tag.Trim());  
                }

                List<string> listTags = blogArticle.Tags.Select(o => o.Name).ToList();

                var addTags = listNewTags.Except(listTags).ToList();
                var delTags = listTags.Except(listNewTags).ToList();

                if(addTags.Any() || delTags.Any())
                {
                    edit = true;
                }

                await AddTags(addTags, blogArticle);
                await DelTags(delTags, blogArticle);

                if (edit)
                {
                    blogArticle.DateChange = DateTime.Now;
                    await Update(blogArticle);
                }
            }
        }

        public async Task AddTags(List<string> listTags, BlogArticle item)
        {
            foreach (var tag in listTags)
            {
                bool tagItemNew = false;

                Tag tagItem = ((TagsRepository)_tagsRepository).GetByName(tag);
                if (tagItem == null)
                {
                    tagItemNew = true;
                    tagItem = new Tag()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = tag
                    };
                }

                tagItem.BlogArticles.Add(item);

                if (tagItemNew)
                    await _tagsRepository.Create(tagItem);
                else
                    await _tagsRepository.Update(tagItem);
            }
        }

        public async Task DelTags(List<string> listTags, BlogArticle item)
        {
            bool update = false;
            foreach (var tag in listTags)
            {
                Tag tagItem = ((TagsRepository)_tagsRepository).GetByName(tag);
                if (tagItem != null)
                {
                    item.Tags.Remove(tagItem);

                    if(tagItem.BlogArticles.Count() == 1)
                    {
                        await _tagsRepository.Delete(tagItem);
                    }

                    update = true;
                }
            }

            if(update)
            {
                await Update(item);
            }
        }

        public async Task Delete(DelBlogArticle model)
        {
            var blogArticle = Set.Include(c => c.Tags).FirstOrDefault(o => o.Id == model.Id);
            if (blogArticle != null)
            {
                List<string> listTags = blogArticle.Tags.Select(o => o.Name).ToList();
                
                await DelTags(listTags, blogArticle);
                await Delete(blogArticle);
            }
        }

        public IEnumerable<BlogArticle> GetAll()
        {
            return base.GetAll();
        }

        public IEnumerable<BlogArticle> GetAllIncludeTags()
        {
            IEnumerable<BlogArticle> allBlogArticle = Set.Include(c => c.Tags);
            return allBlogArticle;
        }

        public BlogArticle GetIncludeCommentsById(string id)
        {
            IEnumerable<BlogArticle> blogArticles = Set.Include(c => c.Comments);
            return blogArticles.FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<BlogArticle> GetAllByUserId(string userId)
        {
            return GetAll().Where(o => o.UserId == userId);
        }

        public BlogArticle GetById(string id)
        {
            IEnumerable<BlogArticle> allBlogArticle = Set.Include(c => c.Tags);
            if(allBlogArticle.Any())
            {
                return allBlogArticle.FirstOrDefault(o => o.Id == id);
            }
            return null;
        }
    }
}
