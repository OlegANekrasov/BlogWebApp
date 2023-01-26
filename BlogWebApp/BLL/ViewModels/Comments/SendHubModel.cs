namespace BlogWebApp.BLL.ViewModels.Comments
{
    public class SendHubModel
    {
        public string blogArticleId { get; set; }

        public string Content { get; set; }

        public string? Image { get; set; }

        public string DateCreate { get; set; }

        public string Author { get; set; }

        public string AuthorId { get; set; }
    }
}
