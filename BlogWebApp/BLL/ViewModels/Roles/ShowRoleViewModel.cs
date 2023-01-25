using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.ViewModels.Roles
{
    /// <summary>
    /// Role data to pass to the view
    /// </summary>
    public class ShowRoleViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Описание")]
        public string Description { get; set; }
    }
}
