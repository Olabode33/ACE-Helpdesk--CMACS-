using System.ComponentModel.DataAnnotations;

namespace Kad.PMSDemo.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}