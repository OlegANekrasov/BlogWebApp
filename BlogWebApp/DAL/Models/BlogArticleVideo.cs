using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWebApp.DAL.Models
{

    /// <summary>
    /// Blog article video
    /// </summary>
    [Table("BlogArticleVideo")]
    public class BlogArticleVideo
    {
        public string Id { get; set; }

        public byte[]? VideoData { get; set; }
        public string? VideoName { get; set; }

        public string? BlogArticleId { get; set; }
        public BlogArticle? BlogArticle { get; set; }
    }
}
