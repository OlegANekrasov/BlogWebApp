using System.ComponentModel.DataAnnotations.Schema;

namespace BlogWebApp.DAL.Models
{
    /// <summary>
    /// Comment properties
    /// </summary>
    [Table("Comments")]
    public class Comment
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateChange { get; set; }

        public byte[]? Image { get; set; }

        public string UserId { get; set; }
        public User? User { get; set; }

        public string BlogArticleId { get; set; }
        public BlogArticle? BlogArticle { get; set; }
    }
}
