using BlogWebApp.BLL.ViewModels.Tags;
using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.Models
{
    /// <summary>
    /// Article data for editing
    /// </summary>
    public class EditBlogArticle
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Tags { get; set; }

        public string UserId { get; set; }

        public EditBlogArticle() { }

        public void SetTags(List<TagSelected> tags) 
        {
            bool first = true;
            foreach (var tag in tags)
            {
                if (first)
                {
                    Tags += tag.Name;
                    first = false;
                }
                else
                {
                    Tags += (", " + tag.Name);
                }
            }
        }
    }
}
