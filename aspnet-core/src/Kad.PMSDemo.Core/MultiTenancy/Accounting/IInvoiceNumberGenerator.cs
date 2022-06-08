using System.Threading.Tasks;
using Abp.Dependency;

namespace Kad.PMSDemo.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}