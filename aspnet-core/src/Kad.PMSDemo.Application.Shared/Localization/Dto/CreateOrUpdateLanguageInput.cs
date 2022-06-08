using System.ComponentModel.DataAnnotations;

namespace Kad.PMSDemo.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}