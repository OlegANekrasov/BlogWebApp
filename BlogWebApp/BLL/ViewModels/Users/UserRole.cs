using System.Data;

namespace BlogWebApp.BLL.ViewModels.Users
{
    public class UserRole
    {
        public string Name { get; set; }

        public bool IsRoleAssigned { get; set; } = false;
    }
}
