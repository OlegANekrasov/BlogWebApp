using BlogWebApp.BLL.ViewModels.Users;
using Microsoft.Extensions.Hosting.Internal;
using System.ComponentModel.DataAnnotations;

namespace BlogWebApp.BLL.Validation.Users
{
    /// <summary>
    /// Role validation attribute - unique name
    /// </summary>
    public class UserRoleAttribute : ValidationAttribute
    {
        public UserRoleAttribute() { }

        public override bool IsValid(object? value)
        {
            var userRoles = value as IEnumerable<UserRole>;
            return userRoles.Where(o => o.IsRoleAssigned).Any();
        }
    }
}
