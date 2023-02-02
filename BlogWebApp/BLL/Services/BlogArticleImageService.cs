using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;

namespace BlogWebApp.BLL.Services
{
    /// <summary>
    /// Describes CRUD operations
    /// </summary>
    public class BlogArticleImageService : IBlogArticleImageService
    {
        private readonly IRepository<BlogArticleImage> _blogArticleImageRepository;
        private readonly ILogger<BlogArticleImageService> _logger;

        public BlogArticleImageService(IRepository<BlogArticleImage> blogArticleImageRepository, ILogger<BlogArticleImageService> logger)
        {
            _blogArticleImageRepository = blogArticleImageRepository;
            _logger = logger;
        }

        public async Task<bool> AddAsync(AddBlogArticleImage model)
        {
            try
            {
                await((BlogArticleImageRepository)_blogArticleImageRepository).Add(model);

                _logger.LogInformation($"Фото '{model.ImageName}' добавлено.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении фото.");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(DelBlogArticleImage model)
        {
            try
            {
                await((BlogArticleImageRepository)_blogArticleImageRepository).Delete(model);

                _logger.LogInformation($"Фото '{model.ImageName}' удалено.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении фото.");
                return false;
            }
        }

        public BlogArticleImage Get(string id)
        {
            return ((BlogArticleImageRepository)_blogArticleImageRepository).GetById(id);
        }
    }
}
