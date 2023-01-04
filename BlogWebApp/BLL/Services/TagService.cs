using AutoMapper;
using BlogWebApp.BLL.Models;
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
            Tag tag = await((TagsRepository)_tagsRepository).Add(model);
            
            List<string> listTags = new List<string> { model.Name };

            BlogArticle blogArticle = await _blogArticlesRepository.Get(model.BlogArticleId);
            ((BlogArticlesRepository)_blogArticlesRepository).AddTags(listTags, blogArticle);
        }

        public async Task Delete(DelTag model)
        {
            var tag = Get(model.Id);
            var blogArticle = await _blogArticlesRepository.Get(model.BlogArticleId);
            if (blogArticle != null && tag != null)
            {
                List<string> listTags = new List<string> { tag.Name };
                ((BlogArticlesRepository)_blogArticlesRepository).DelTags(listTags, blogArticle);
            }

            await ((TagsRepository)_tagsRepository).Delete(model);
        }

        public async Task Edit(EditTag model)
        {
            var tag = Get(model.Id);
            if(tag.BlogArticles.Count() == 1)
            {
                await ((TagsRepository)_tagsRepository).Edit(model);
            }
            else
            {
                var modelDel = _mapper.Map<DelTag>(model);
                await Delete(modelDel);

                var modelAdd = _mapper.Map<AddTag>(model);
                await Add(modelAdd);
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
    }
}
