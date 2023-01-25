using BlogWebApp.BLL.ViewModels.Roles;
using BlogWebApp.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Users
{
    /// <summary>
    /// User data to pass to the view
    /// </summary>
    public class UserViewModel
    {
        [Required]
        [Display(Name = "Идентификатор пользователя")]
        public string UserId { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Имя", Prompt = "Введите имя")]
        public string? FirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Фамилия", Prompt = "Введите фамилию")]
        public string? LastName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Отчество", Prompt = "Введите отчество")]
        public string? MiddleName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email", Prompt = "Email@example.com")]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime BirthDate { get; set; }

        public string UserName => Email;

        [Phone]
        [Display(Name = "Phone number")]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Фото", Prompt = "Загрузите фото")]
        public byte[]? Image { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "О себе", Prompt = "Введите данные о себе")]
        public string? About { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Количество статей и комментариев")]
        public string? BlogArticles { get; set; }

        public List<UserRole> Roles { get; set; } = new List<UserRole>();
    }
}
