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
    /// <summary>
    /// Implements operations on the Comment model
    /// </summary>
    public class CommentsRepository : Repository<Comment>
    {
        private readonly UserManager<User> userManager;
        private readonly IRepository<BlogArticle> _blogArticlesRepository;

        public CommentsRepository(ApplicationDbContext db, UserManager<User> _userManager, IRepository<BlogArticle> blogArticlesRepository) : base(db)
        {
            userManager = _userManager;
            _blogArticlesRepository = blogArticlesRepository;
        }

        /// <summary>
        /// Adding an entry to the database
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Editing existing entries
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Deleting an entry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task DeleteAsync(DelComment model)
        {
            var comment = await GetAsync(model.Id);
            if (comment != null)
            {
                await DeleteAsync(comment);
            }
        }

        /// <summary>
        /// Selecting all records from the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Comment> GetAll()
        {
            return base.GetAll();
        }

        /// <summary>
        /// Selecting a record by ID from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Comment GetById(string id)
        {
            return GetAll().FirstOrDefault(o => o.Id == id);
        }

        /// <summary>
        /// Selecting all records of a specific user from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetAllByUserId(string id)
        {
            return GetAll().Where(o => o.UserId == id);
        }

        /// <summary>
        /// Selecting all records of a particular article from the database
        /// </summary>
        /// <param name="blogArticleId"></param>
        /// <returns></returns>
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
