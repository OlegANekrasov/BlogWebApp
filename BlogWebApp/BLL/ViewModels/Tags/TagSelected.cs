using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.ViewModels.Tags
{
    /// <summary>
    /// Data to pass to the Tag Selector view
    /// </summary>
    public class TagSelected
    {
        public string Name { get; set;}

        public bool IsTagSelected { get; set;} = false;
    }
}
