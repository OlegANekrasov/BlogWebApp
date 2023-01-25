using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.EF;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.DAL.Repository
{
    /// <summary>
    /// Implements operations on the Tag model
    /// </summary>
    public class TagsRepository : Repository<Tag>
    {
        public TagsRepository(ApplicationDbContext db) : base(db)
        {
        }

        /// <summary>
        /// Adding an entry to the database
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<Tag> Add(AddTag model)
        {
            var tag = Set.AsEnumerable().FirstOrDefault(x => x.Name == model.Name);
            if (tag == null)
            {
                var item = new Tag()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name
                };

                await CreateAsync(item);

                return item;
            }

            return tag;
        }

        /// <summary>
        /// Editing existing entries
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Edit(EditTag model)
        {
            var tag = await GetAsync(model.Id);
            if (tag != null)
            {
                var edit = false;

                if (tag.Name != model.Name)
                {
                    tag.Name = model.Name;
                    edit = true;
                }

                if (edit)
                {
                    await UpdateAsync(tag);
                }
            }
        }

        /// <summary>
        /// Deleting an entry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Delete(DelTag model)
        {
            var tag = GetById(model.Id);
            if (tag != null)
            {
                await DeleteAsync(tag);
            }
        }

        /// <summary>
        /// Selecting a record by ID from the database include BlogArticles
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tag GetByIdIncludeBlogArticles(string id)
        {
            IEnumerable<Tag> allTags = Set.Include(c => c.BlogArticles);
            if (allTags.Any())
            {
                return allTags.FirstOrDefault(o => o.Id == id);
            }
            return null;
        }

        /// <summary>
        /// Selecting all records from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tag> GetAll()
        {
            return base.GetAll();
        }

        /// <summary>
        /// Selecting all records from the database include BlogArticles
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tag> GetAllIncludeBlogArticles()
        {
            return Set.Include(c => c.BlogArticles);
        }

        /// <summary>
        /// Selecting a record by ID from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tag GetById(string id)
        {
            return GetAll().FirstOrDefault(o => o.Id == id);
        }

        /// <summary>
        /// Selecting a record by Name from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tag GetByName(string name)
        {
            IEnumerable<Tag> allTags = Set.Include(c => c.BlogArticles);
            if (allTags.Any())
            {
                return allTags.FirstOrDefault(o => o.Name == name);
            }

            return null;
        }
    }
}

