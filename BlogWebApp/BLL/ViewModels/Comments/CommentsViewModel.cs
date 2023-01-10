using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.ViewModels.Comments
{
    public class CommentsViewModel
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public string DateChange { get; set; }

        public string Author { get; set; }

        public string AuthorId { get; set; }
    }
}
