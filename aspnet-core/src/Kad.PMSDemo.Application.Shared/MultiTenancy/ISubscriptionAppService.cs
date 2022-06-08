using System.Threading.Tasks;
using Abp.Application.Services;

namespace Kad.PMSDemo.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
