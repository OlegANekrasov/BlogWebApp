using AutoMapper;
using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.Services.Interfaces;
using BlogWebApp.BLL.ViewModels.Tags;
using BlogWebApp.Controllers;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;

namespace BlogWebApp.BLL.Services
{
    /// <summary>
    /// Describes CRUD operations
    /// </summary>
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _tagsRepository;
        private readonly IRepository<BlogArticle> _blogArticlesRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TagService> _logger;

        public TagService(IRepository<Tag> tagsRepository, IRepository<BlogArticle> blogArticlesRepository, IMapper mapper, ILogger<TagService> logger)
        {
            _tagsRepository = tagsRepository;
            _blogArticlesRepository = blogArticlesRepository;
            _mapper = mapper;
            _logger = logger;
        }
        
        public async Task<bool> AddAsync(AddTag model)
        {
            try
            {
                await ((TagsRepository)_tagsRepository).Add(model);

                _logger.LogInformation($"Тег '{model.Name}' добавлен.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении тега.");
                return false;
            }
        }

        public async Task<bool> EditAsync(EditTag model)
        {
            try
            {
                await ((TagsRepository)_tagsRepository).Edit(model);

                _logger.LogInformation($"Тег '{model.OldName}' изменен на '{model.Name}'.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при изменении тега.");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(DelTag model)
        {
            try
            {
                await ((TagsRepository)_tagsRepository).Delete(model);

                _logger.LogInformation($"Тег '{model.Name}' удален.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении тега.");
                return false;
            }            
        }

        public Tag Get(string id)
        {
            return ((TagsRepository)_tagsRepository).GetById(id);
        }

        public IEnumerable<Tag> GetAll()
        {
            return ((TagsRepository)_tagsRepository).GetAll();
        }

        public IEnumerable<Tag> GetAllIncludeBlogArticles()
        {
            return ((TagsRepository)_tagsRepository).GetAllIncludeBlogArticles();
        }

        public ListTagsViewModel GetListTagsViewModel()
        {
            var tags = GetAllIncludeBlogArticles().OrderBy(o => o.Name).ToList();

            var model = new ListTagsViewModel(tags);
            return model;
        }

        public ListTagsViewModel GetListTagsViewModel(User user) 
        {
            var tags = GetAllIncludeBlogArticles().OrderBy(o => o.Name).ToList();
            
            var model = new ListTagsViewModel(tags, user: user);
            return model;
        }
    }
}
