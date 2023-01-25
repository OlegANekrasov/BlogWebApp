using BlogWebApp.BLL.ViewModels.BlogArticles;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.ViewModels.Tags
{
    /// <summary>
    /// Passing Data to the List View
    /// </summary>
    public class ListTagsViewModel
    {
        public List<TagViewModel> _tags;
        public string _blogArticleId;

        public ListTagsViewModel() { }
        public ListTagsViewModel(List<Tag> tag, string blogArticleId = null, User user = null)
        {
            _blogArticleId = blogArticleId;

            _tags = new List<TagViewModel>();
            foreach (var item in tag) 
            {
                TagViewModel tagViewModel;
                if (user != null)
                {
                    var blogArticlesUserCount = item.BlogArticles.Where(o => o.UserId == user.Id).Count();
                    var blogArticlesUser = blogArticlesUserCount.ToString();

                    bool isUserUsingTag = false;
                    if(blogArticlesUserCount == item.BlogArticles.Count())
                    {
                        isUserUsingTag = true;
                    }

                    tagViewModel = new TagViewModel(item.Id, item.Name, item.BlogArticles.Count().ToString(), blogArticlesUser, isUserUsingTag);
                }
                else
                {
                    tagViewModel = new TagViewModel(item.Id, item.Name, item.BlogArticles.Count().ToString());
                }

                _tags.Add(tagViewModel);
            }
        }
    }
}
