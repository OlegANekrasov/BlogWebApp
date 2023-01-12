using BlogWebApp.BLL.ViewModels.Tags;
using BlogWebApp.BLL.ViewModels.Users;
using BlogWebApp.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.BlogArticles
{
    public class CreateBlogArticleViewModel
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

        public string UserId { get; set; }

        public CreateBlogArticleViewModel() { }
        public CreateBlogArticleViewModel(List<Tag> tags)
        {
            foreach (var item in tags)
            {
                Tags.Add(new TagSelected() { Name = item.Name });
            }
        }
    }
}
