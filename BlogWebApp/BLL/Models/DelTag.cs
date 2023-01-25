namespace BlogWebApp.BLL.Models
{
    /// <summary>
    /// Tag data to delete
    /// </summary>
    public class DelTag
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string BlogArticleId { get; set; }
    }
}
