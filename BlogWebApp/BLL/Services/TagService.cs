using AutoMapper;
using BlogWebApp.BLL.Models;
using BlogWebApp.BLL.ViewModels.Tags;
using BlogWebApp.DAL.Interfaces;
using BlogWebApp.DAL.Models;
using BlogWebApp.DAL.Repository;

namespace BlogWebApp.BLL.Services
{
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _tagsRepository;
        private readonly IRepository<BlogArticle> _blogArticlesRepository;
        private readonly IMapper _mapper;

        public TagService(IRepository<Tag> tagsRepository, IRepository<BlogArticle> blogArticlesRepository, IMapper mapper)
        {
            _tagsRepository = tagsRepository;
            _blogArticlesRepository = blogArticlesRepository;
            _mapper = mapper;
        }
        
        public async Task Add(AddTag model)
        {
            await((TagsRepository)_tagsRepository).Add(model);
        }

        public async Task Delete(DelTag model)
        {
            await ((TagsRepository)_tagsRepository).Delete(model);
        }

        public async Task Edit(EditTag model)
        {
            await ((TagsRepository)_tagsRepository).Edit(model);
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
