using Abp.Domain.Services;

namespace Kad.PMSDemo
{
    public abstract class PMSDemoDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected PMSDemoDomainServiceBase()
        {
            LocalizationSourceName = PMSDemoConsts.LocalizationSourceName;
        }
    }
}
