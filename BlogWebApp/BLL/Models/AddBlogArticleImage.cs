namespace BlogWebApp.BLL.Models
{
    /// <summary>
    /// Blog Article Image data to add
    /// </summary>
    public class AddBlogArticleImage
    {
        public byte[]? Image { get; set; }

        public string? ImageName { get; set; }

        public string? BlogArticleId { get; set; }
    }
}
