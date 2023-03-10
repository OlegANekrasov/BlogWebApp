using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWebApp.DAL.Models
{
    /// <summary>
    /// Tag properties
    /// </summary>
    [Table("Tags")]
    public class Tag
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public List<BlogArticle> BlogArticles { get; set; } = new List<BlogArticle>();
    }
}
