using System.ComponentModel.DataAnnotations.Schema;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BlogWebApp.DAL.Models
{
    /// <summary>
    /// Article properties
    /// </summary>
    [Table("BlogArticles")]
    public class BlogArticle
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime? DateChange { get; set; }

        public int? CountOfVisit { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public List<Tag> Tags { get; set; } = new List<Tag>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<BlogArticleImage> Images { get; set; } = new List<BlogArticleImage>();
        public List<BlogArticleVideo> Video { get; set; } = new List<BlogArticleVideo>();
    }
}
