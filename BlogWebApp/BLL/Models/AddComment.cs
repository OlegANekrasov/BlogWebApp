namespace BlogWebApp.BLL.Models
{

    /// <summary>
    /// Comment data to add
    /// </summary>
    public class AddComment
    {
        public string Content { get; set; }

        public byte[]? Image { get; set; }

        public string UserId { get; set; }

        public string BlogArticleId { get; set; }
    }
}
