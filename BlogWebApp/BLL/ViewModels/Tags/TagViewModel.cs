using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Tags
{
    /// <summary>
    /// Tag data to pass to the view
    /// </summary>
    public class TagViewModel
    {
        [Required]
        [Display(Name = "Идентификатор тега")]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        public string? BlogArticles { get; set; }

        [DataType(DataType.Text)]
        public string? BlogArticlesUser { get; set; }

        public bool IsUserUsingTag { get; set; } = false;

        public TagViewModel(string id, string name, string blogArticles = null, string blogArticlesUser = null, bool isUserUsingTag = false)
        {
            Id = id;
            Name = name;
            BlogArticles = "Статей по тегу: (" + blogArticles + ")";
            BlogArticlesUser = "Ваших статей по тегу: (" + (blogArticlesUser ?? "0") + ")";
            IsUserUsingTag = isUserUsingTag;
        }
    }
}
