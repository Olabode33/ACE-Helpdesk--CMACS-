using Abp;

namespace Kad.PMSDemo
{
    /// <summary>
    /// This class can be used as a base class for services in this application.
    /// It has some useful objects property-injected and has some basic methods most of services may need to.
    /// It's suitable for non domain nor application service classes.
    /// For domain services inherit <see cref="PMSDemoDomainServiceBase"/>.
    /// For application services inherit PMSDemoAppServiceBase.
    /// </summary>
    public abstract class PMSDemoServiceBase : AbpServiceBase
    {
        protected PMSDemoServiceBase()
        {
            LocalizationSourceName = PMSDemoConsts.LocalizationSourceName;
        }
    }
}