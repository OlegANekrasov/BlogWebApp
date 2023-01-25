namespace BlogWebApp.BLL.Models
{
    /// <summary>
    /// Comment data to delete
    /// </summary>
    public class DelComment
    {
        public string Id { get; set; }

        public string BlogArticleId { get; set; }

        public string Content { get; set; }
    }
}
