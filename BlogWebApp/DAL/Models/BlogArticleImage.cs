using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWebApp.DAL.Models
{
    /// <summary>
    /// Blog article photos
    /// </summary>
    [Table("BlogArticleImages")]
    public class BlogArticleImage
    {
        public string Id { get; set; }

        public byte[]? Image { get; set; }
        public string? ImageName { get; set; }

        public string? BlogArticleId { get; set; }
        public BlogArticle? BlogArticle { get; set; }
    }
}
