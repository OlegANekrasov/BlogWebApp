using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Comments
{
    /// <summary>
    /// Data to pass to the Edit view
    /// </summary>
    public class EditCommentViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Комментарий", Prompt = "Введите комментарий")]
        public string Content { get; set; }

        public byte[]? Image { get; set; }

        public string Id { get; set; }
        public string BlogArticleId { get; set; }

        public EditCommentViewModel() { }
        public EditCommentViewModel(string name, string id, string blogArticleId, byte[]? image)
        {
            Content = name;
            Id = id;
            BlogArticleId = blogArticleId;
            
            if(image != null && image.Any())
            { 
                Image = image;
            }
        }
    }
}
