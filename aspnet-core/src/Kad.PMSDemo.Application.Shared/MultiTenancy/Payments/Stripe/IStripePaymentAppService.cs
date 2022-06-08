using System.Threading.Tasks;
using Abp.Application.Services;
using Kad.PMSDemo.MultiTenancy.Payments.Dto;
using Kad.PMSDemo.MultiTenancy.Payments.Stripe.Dto;

namespace Kad.PMSDemo.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}