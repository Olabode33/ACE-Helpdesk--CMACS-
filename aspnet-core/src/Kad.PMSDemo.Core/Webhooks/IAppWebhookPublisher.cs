using System.Threading.Tasks;
using Kad.PMSDemo.Authorization.Users;

namespace Kad.PMSDemo.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
