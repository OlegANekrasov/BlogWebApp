namespace BlogWebApp.BLL.ViewModels.BlogArticles
{
    public class DeletePhotoBlogArticleViewModel
    {
        public string Id { get; set; }

        public string BlogArticleId { get; set; }

        public byte[]? Image { get; set; }

        public string? ImageName { get; set; }
    }
}
