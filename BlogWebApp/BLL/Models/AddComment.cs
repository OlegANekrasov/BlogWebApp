namespace BlogWebApp.BLL.Models
{
    public class AddComment
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }

        public string BlogArticleId { get; set; }
    }
}
