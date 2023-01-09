namespace BlogWebApp.BLL.ViewModels.Users
{
    public class UserListModel
    {
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? RoleName { get; set; }
        public byte[]? Image { get; set; }

        public UserListModel() { }
        public UserListModel(string id, string? username, string? email, string? roleName, byte[]? image) 
        {
            Id = id;
            UserName = username;
            Email = email;
            RoleName = roleName;
            Image = image;
        }
    }
}
