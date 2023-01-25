namespace BlogWebApp.BLL.Models
{
    /// <summary>
    /// Article data to add
    /// </summary>
    public class AddBlogArticle
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
    }
}
