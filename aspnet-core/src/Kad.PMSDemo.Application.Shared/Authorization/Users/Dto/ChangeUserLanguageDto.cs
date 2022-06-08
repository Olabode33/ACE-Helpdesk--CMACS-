using System.ComponentModel.DataAnnotations;

namespace Kad.PMSDemo.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
