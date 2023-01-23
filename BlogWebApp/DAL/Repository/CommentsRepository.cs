using BlogWebApp.BLL.Models;
using BlogWebApp.Controllers;
using BlogWebApp.DAL.EF;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BlogWebApp.DAL.Repository
{
    public class CommentsRepository : Repository<Comment>
    {
        private readonly UserManager<User> userManager;
        private readonly IRepository<BlogArticle> _blogArticlesRepository;

        public CommentsRepository(ApplicationDbContext db, UserManager<User> _userManager, IRepository<BlogArticle> blogArticlesRepository) : base(db)
        {
            userManager = _userManager;
            _blogArticlesRepository = blogArticlesRepository;
        }

        public async Task AddAsync(AddComment model)
        {
            var item = new Comment()
            {
                Id = Guid.NewGuid().ToString(),
                Content = model.Content,
                Image= model.Image,
                DateCreation = DateTime.Now,
                DateChange = DateTime.Now,
                BlogArticle = ((BlogArticlesRepository)_blogArticlesRepository).GetById(model.BlogArticleId),
                User = await userManager.FindByIdAsync(model.UserId)
            };

            await CreateAsync(item);
        }

        public async Task EditAsync(EditComment model)
        {
            var comment = await GetAsync(model.Id);
            if (comment != null)
            {
                var edit = false;

                if (comment.Content != model.Content)
                {
                    comment.Content = model.Content;
                    edit = true;
                }

                if (comment.Image != model.Image)
                {
                    comment.Image = model.Image;
                    edit = true;
                }

                if (edit)
                {
                    await UpdateAsync(comment);
                }
            }
        }

        public async Task DeleteAsync(DelComment model)
        {
            var comment = await GetAsync(model.Id);
            if (comment != null)
            {
                await DeleteAsync(comment);
            }
        }

        public IEnumerable<Comment> GetAll()
        {
            return base.GetAll();
        }

        public Comment GetById(string id)
        {
            return GetAll().FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<Comment> GetAllByUserId(string id)
        {
            return GetAll().Where(o => o.UserId == id);
        }

        public IEnumerable<Comment> GetAllByBlogArticleId(string blogArticleId)
        {
            BlogArticle blogArticle = ((BlogArticlesRepository)_blogArticlesRepository).GetIncludeCommentsById(blogArticleId);
            if(blogArticle != null)
            {
                return blogArticle.Comments;
            }

            return null;
        }

    }
}
