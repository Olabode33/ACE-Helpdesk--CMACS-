using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.MultiTenancy.Accounting.Dto;

namespace Kad.PMSDemo.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
