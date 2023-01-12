using BlogWebApp.DAL.Models;

namespace BlogWebApp.BLL.ViewModels.Tags
{
    public class TagSelected
    {
        public string Name { get; set;}

        public bool IsTagSelected { get; set;} = false;
    }
}
