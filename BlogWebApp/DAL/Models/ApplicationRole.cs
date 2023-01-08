using Microsoft.AspNetCore.Identity;

namespace BlogWebApp.DAL.Models
{
    public class ApplicationRole : IdentityRole
    {
        public string? Description { get; set; }

        public ApplicationRole() { }

        public ApplicationRole(string roleName, string description = null) : base (roleName) 
        {
            Description = description ?? string.Empty;
        }
    }
}
