using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.EF;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.DAL.Repository
{
    public class TagsRepository : Repository<Tag>
    {
        public TagsRepository(ApplicationDbContext db) : base(db)
        {
        }

        public async Task Add(AddTag model)
        {
            var tag = Set.AsEnumerable().FirstOrDefault(x => x.Name == model.Name);
            if (tag == null)
            {
                var item = new Tag()
                {
                    Name = model.Name
                };

                await Create(item);
            }
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
            var tag = await Get(model.Id);
            if (tag != null)
            {
                await Delete(tag);
            }
        }

        public IEnumerable<Tag> GetAll()
        {
            return GetAll();
        }

        public Tag GetById(string id)
        {
            return GetAll().FirstOrDefault(o => o.Id == id);
        }
    }
}

