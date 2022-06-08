using System.Threading.Tasks;
using Abp.Webhooks;

namespace Kad.PMSDemo.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
