namespace BlogWebApp.DAL.Models
{
    public class Tag
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string BlogArticleId { get; set; }
        public BlogArticle? BlogArticle { get; set; }
    }
}
