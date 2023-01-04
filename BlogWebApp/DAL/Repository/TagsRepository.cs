using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.EF;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApp.DAL.Repository
{
    public class TagsRepository : Repository<Tag>
    {
        public TagsRepository(ApplicationDbContext db) : base(db)
        {
        }

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

                await Create(item);

                return item;
            }

            return tag;
        }

        public async Task Edit(EditTag model)
        {
            var tag = await Get(model.Id);
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
                    await Update(tag);
                }
            }
        }

        public async Task Delete(DelTag model)
        {
            var tag = GetByIdIncludeBlogArticles(model.Id);
            if (tag != null)
            {
                if(tag.BlogArticles.Count() == 0)
                {
                    await Delete(tag);
                }
            }
        }

        public Tag GetByIdIncludeBlogArticles(string id)
        {
            IEnumerable<Tag> allTags = Set.Include(c => c.BlogArticles);
            if (allTags.Any())
            {
                return allTags.FirstOrDefault(o => o.Id == id);
            }
            return null;
        }

        public IEnumerable<Tag> GetAll()
        {
            return base.GetAll();
        }

        public Tag GetById(string id)
        {
            return GetAll().FirstOrDefault(o => o.Id == id);
        }

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

