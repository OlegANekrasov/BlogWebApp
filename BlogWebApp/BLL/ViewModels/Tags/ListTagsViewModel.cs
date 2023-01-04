using BlogWebApp.BLL.ViewModels.BlogArticles;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.ViewModels.Tags
{
    public class ListTagsViewModel
    {
        public List<TagViewModel> _tags;
        public string _blogArticleId;

        public ListTagsViewModel(List<Tag> tag, string blogArticleId)
        {
            _blogArticleId = blogArticleId;

            _tags = new List<TagViewModel>();
            foreach (var item in tag) 
            {
                _tags.Add(new TagViewModel(item.Id, item.Name));
            }
        }
    }
}
