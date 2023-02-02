using BlogWebApp.BLL.ViewModels.Tags;
using BlogWebApp.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.BlogArticles
{
    /// <summary>
    /// Data to pass to the Edit view
    /// </summary>
    public class EditBlogArticleViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Название", Prompt = "Введите название")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Описание", Prompt = "Введите описание")]
        public string Description { get; set; }

        public List<TagSelected> Tags { get; set; } = new List<TagSelected>();
        public List<BlogArticleImage> Images { get; set; }

        public string Id { get; set; }
        public string UserId { get; set; }

        public void SetTags(List<string> AllTags, List<string> tags)
        {
            foreach (var item in AllTags)
            {
                var tagSelected = new TagSelected() { Name = item };
                if(tags.FirstOrDefault(o => o == item) != null)
                {
                    tagSelected.IsTagSelected = true;
                }

                Tags.Add(tagSelected);
            }
        }
    }
}
