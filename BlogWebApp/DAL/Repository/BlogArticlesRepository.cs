using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.EF;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.DAL.Repository
{
    /// <summary>
    /// Implements operations on the BlogArticle model
    /// </summary>
    public class BlogArticlesRepository : Repository<BlogArticle>
    {
        private readonly IRepository<Tag> _tagsRepository;

        public BlogArticlesRepository(ApplicationDbContext db, IRepository<Tag> tagsRepository) : base(db)
        {
            _tagsRepository = tagsRepository;
        }

        /// <summary>
        /// Adding an entry to the database
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
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

                await CreateAsync(item);
                await AddTags(model.Tags, item);
            }
        }

        /// <summary>
        /// Editing existing entries
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

                if (model.Tags != null && model.Tags.Any())
                {
                    List<string> listNewTags = new List<string>();
                    string[] tags = model.Tags.Split(',');
                    foreach (var tag in tags)
                    {
                        listNewTags.Add(tag.Trim());
                    }

                    List<string> listTags = blogArticle.Tags.Select(o => o.Name).ToList();

                    var addTags = listNewTags.Except(listTags).ToList();
                    var delTags = listTags.Except(listNewTags).ToList();

                    if (addTags.Any() || delTags.Any())
                    {
                        edit = true;
                    }

                    await AddTags(addTags, blogArticle);
                    await DelTags(delTags, blogArticle);
                }

                if (edit)
                {
                    blogArticle.DateChange = DateTime.Now;
                    await UpdateAsync(blogArticle);
                }
            }
        }

        /// <summary>
        /// Adding tags to an article
        /// </summary>
        /// <param name="listTags"></param>
        /// <param name="item"></param>
        /// <returns></returns>
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
                    await _tagsRepository.CreateAsync(tagItem);
                else
                    await _tagsRepository.UpdateAsync(tagItem);
            }
        }

        /// <summary>
        /// Removing article tags
        /// </summary>
        /// <param name="listTags"></param>
        /// <param name="item"></param>
        /// <returns></returns>
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
                        await _tagsRepository.DeleteAsync(tagItem);
                    }

                    update = true;
                }
            }

            if(update)
            {
                await UpdateAsync(item);
            }
        }

        /// <summary>
        /// Deleting an entry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Delete(DelBlogArticle model)
        {
            var blogArticle = Set.Include(c => c.Tags).FirstOrDefault(o => o.Id == model.Id);
            if (blogArticle != null)
            {
                List<string> listTags = blogArticle.Tags.Select(o => o.Name).ToList();
                
                await DelTags(listTags, blogArticle);
                await DeleteAsync(blogArticle);
            }
        }

        /// <summary>
        /// Saving the number of views of an article
        /// </summary>
        /// <param name="blogArticle"></param>
        /// <returns></returns>
        public async Task IncCountOfVisit(BlogArticle blogArticle)
        {
            if (blogArticle != null)
            {
                await UpdateAsync(blogArticle);
            }
        }

        /// <summary>
        /// Selecting all records from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BlogArticle> GetAll()
        {
            return base.GetAll();
        }

        /// <summary>
        /// Selecting all records from the database include Tags
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BlogArticle> GetAllIncludeTags()
        {
            IEnumerable<BlogArticle> allBlogArticle = Set.Include(c => c.Tags);
            return allBlogArticle;
        }

        /// <summary>
        /// Selecting a record by ID from the database include Comments
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BlogArticle GetIncludeCommentsById(string id)
        {
            IEnumerable<BlogArticle> blogArticles = Set.Include(c => c.Comments);
            return blogArticles.FirstOrDefault(o => o.Id == id);
        }

        /// <summary>
        /// Selecting all records of a specific user from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<BlogArticle> GetAllByUserId(string userId)
        {
            return GetAll().Where(o => o.UserId == userId);
        }

        /// <summary>
        /// Selecting a record by ID from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BlogArticle GetById(string id)
        {
            IEnumerable<BlogArticle> allBlogArticle = Set.Include(c => c.Tags).Include(c => c.Images);
            if(allBlogArticle.Any())
            {
                return allBlogArticle.FirstOrDefault(o => o.Id == id);
            }
            return null;
        }
    }
}
