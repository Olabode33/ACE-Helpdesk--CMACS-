using Kad.PMSDemo.Editions.Dto;

namespace Kad.PMSDemo.MultiTenancy.Payments.Dto
{
    public class PaymentInfoDto
    {
        public EditionSelectDto Edition { get; set; }

        public decimal AdditionalPrice { get; set; }

        public bool IsLessThanMinimumUpgradePaymentAmount()
        {
            return AdditionalPrice < PMSDemoConsts.MinimumUpgradePaymentAmount;
        }
    }
}
