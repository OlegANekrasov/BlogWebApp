namespace BlogWebApp.BLL.Models
{
    public class AddComment
    {
        public string Content { get; set; }

        public byte[]? Image { get; set; }

        public string UserId { get; set; }

        public string BlogArticleId { get; set; }
    }
}
