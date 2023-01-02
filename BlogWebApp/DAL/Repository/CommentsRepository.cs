using BlogWebApp.BLL.Models;
using BlogWebApp.Controllers;
using BlogWebApp.DAL.EF;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogWebApp.DAL.Repository
{
    public class CommentsRepository : Repository<Comment>
    {
        UserManager<User> userManager;

        public CommentsRepository(ApplicationDbContext db, UserManager<User> _userManager) : base(db)
        {
            userManager = _userManager;
        }

        public async Task Add(AddComment model, BlogArticlesRepository blogArticlesRepository)
        {
            var item = new Comment()
            {
                Content = model.Content,
                DateCreation = DateTime.Now,
                DateChange = DateTime.Now,
                BlogArticle = blogArticlesRepository.GetById(model.Id),
                User = await userManager.FindByIdAsync(model.UserId)
            };

            await Create(item);
        }

        public async Task Edit(EditComment model)
        {
            var comment = await Get(model.Id);
            if (comment != null)
            {
                var edit = false;

                if (comment.Content != model.Content)
                {
                    comment.Content = model.Content;
                    edit = true;
                }

                if (edit)
                {
                    await Update(comment);
                }
            }
        }

        public async Task Delete(DelComment model)
        {
            var comment = await Get(model.Id);
            if (comment != null)
            {
                await Delete(comment);
            }
        }

        public IEnumerable<Comment> GetAll()
        {
            return GetAll();
        }

        public IEnumerable<Comment> GetAllById(string id)
        {
            return GetAll().Where(o => o.Id == id);
        }
    }
}
