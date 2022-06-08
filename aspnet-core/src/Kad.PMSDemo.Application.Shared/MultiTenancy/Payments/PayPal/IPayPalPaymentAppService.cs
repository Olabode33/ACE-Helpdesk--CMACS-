using System.Threading.Tasks;
using Abp.Application.Services;
using Kad.PMSDemo.MultiTenancy.Payments.PayPal.Dto;

namespace Kad.PMSDemo.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}
