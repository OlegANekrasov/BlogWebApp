namespace BlogWebApp.BLL.ViewModels.Users
{
    public class UserListModel
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? RoleName { get; set; }

        public UserListModel() { }
        public UserListModel(string id, string? username, string? email, string? roleName) 
        {
            Id = id;
            UserName = username;
            Email = email;
            RoleName = roleName;
        }
    }
}
