using System.Data;

namespace BlogWebApp.BLL.ViewModels.Users
{
    /// <summary>
    /// User data to pass to the Role view
    /// </summary>
    public class UserRole
    {
        public string Name { get; set; }

        public bool IsRoleAssigned { get; set; } = false;
    }
}
