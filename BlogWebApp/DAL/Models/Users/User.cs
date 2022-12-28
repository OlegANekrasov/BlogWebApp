using Microsoft.AspNetCore.Identity;

namespace BlogWebApp.DAL.Models.Users
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? MiddleName { get; set; }

        public DateTime? BirthDate { get; set; }

        public byte[]? Image { get; set; }

        public string? About { get; set; }

        public string GetFullName()
        {
            return FirstName + " " + MiddleName + " " + LastName;
        }

        public User()
        {
            About = "Информация обо мне.";
        }

    }
}
