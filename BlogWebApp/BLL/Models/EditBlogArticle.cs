namespace BlogWebApp.BLL.Models
{
    public class EditBlogArticle
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }

        public string UserId { get; set; }

        public DateTime? DateChange { get; set; }
    }
}
