namespace BlogWebApp.BLL.ViewModels.Tags
{
    public class TagViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public TagViewModel(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
