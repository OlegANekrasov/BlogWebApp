using BlogWebApp.BLL.Models;
using BlogWebApp.DAL.EF;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.DAL.Repository
{
    public class BlogArticleImageRepository : Repository<BlogArticleImage>
    {
        public BlogArticleImageRepository(ApplicationDbContext db) : base(db)
        {
            
        }

        /// <summary>
        /// Adding an entry to the database
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<BlogArticleImage> Add(AddBlogArticleImage model)
        {
            var item = new BlogArticleImage()
            {
                Id = Guid.NewGuid().ToString(),
                Image = model.Image,
                ImageName = model.ImageName,
                BlogArticleId = model.BlogArticleId
            };

            await CreateAsync(item);

            return item;
        }

        /// <summary>
        /// Deleting an entry
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Delete(DelBlogArticleImage model)
        {
            var blogArticleImage = GetById(model.Id);
            if (blogArticleImage != null)
            {
                await DeleteAsync(blogArticleImage);
            }
        }

        /// <summary>
        /// Selecting a record by ID from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BlogArticleImage GetById(string id)
        {
            return GetAll().FirstOrDefault(o => o.Id == id);
        }

    }
}
