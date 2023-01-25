using BlogWebApp.BLL.Services;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BlogWebApp.BLL.Validation.Tags
{
    /// <summary>
    /// Tag validation attribute - one word
    /// </summary>
    public class TagNameAttribute : ValidationAttribute
    {
        public TagNameAttribute() { }

        public override bool IsValid(object? value)
        {
            var str = value as string;
            return str != null && !str.Contains(' ');
        }
    }
}
