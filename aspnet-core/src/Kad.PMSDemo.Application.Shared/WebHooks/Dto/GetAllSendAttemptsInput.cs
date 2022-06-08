using Kad.PMSDemo.Dto;

namespace Kad.PMSDemo.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
