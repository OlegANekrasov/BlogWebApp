namespace BlogWebApp.BLL.Models
{
    /// <summary>
    /// Tag data for editing
    /// </summary>
    public class EditTag
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string OldName { get; set; }

        public string BlogArticleId { get; set; }
    }
}
