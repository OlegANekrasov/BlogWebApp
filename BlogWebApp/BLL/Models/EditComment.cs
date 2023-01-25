namespace BlogWebApp.BLL.Models
{
    /// <summary>
    /// Comment data for editing
    /// </summary>
    public class EditComment
    {
        public string Id { get; set; }

        public string BlogArticleId { get; set; }

        public string Content { get; set; }

        public byte[]? Image { get; set; }
    }
}
