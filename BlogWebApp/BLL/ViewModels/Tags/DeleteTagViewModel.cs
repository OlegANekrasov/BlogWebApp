using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Tags
{
    public class DeleteTagViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        public string BlogArticleId { get; set; }
        public string Id { get; set; }

        public DeleteTagViewModel() { }
        public DeleteTagViewModel(string id, string name, string blogArticleId)
        {
            Id = id;
            Name = name;
            BlogArticleId = blogArticleId;
        }
    }
}
