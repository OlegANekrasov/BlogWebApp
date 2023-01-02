using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWebApp.DAL.Models
{
    [Table("Tags")]
    public class Tag
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<BlogArticle> BlogArticles { get; set; }
    }
}
