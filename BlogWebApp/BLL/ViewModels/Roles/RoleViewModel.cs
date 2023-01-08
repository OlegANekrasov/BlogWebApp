namespace BlogWebApp.BLL.ViewModels.Roles
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public bool IsProgramRole { get; set; } = false;

        public RoleViewModel() { }
        public RoleViewModel(string id, string name, string description = null) 
        {
            Id = id;
            Name = name;
            Description = description;  

            if(name == "Администратор" || name == "Модератор" || name == "Пользователь")
            {
                IsProgramRole = true;
            }
        }
    }
}
