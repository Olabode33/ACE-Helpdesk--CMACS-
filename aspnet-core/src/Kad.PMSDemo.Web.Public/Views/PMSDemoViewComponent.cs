using Abp.AspNetCore.Mvc.ViewComponents;

namespace Kad.PMSDemo.Web.Public.Views
{
    public abstract class PMSDemoViewComponent : AbpViewComponent
    {
        protected PMSDemoViewComponent()
        {
            LocalizationSourceName = PMSDemoConsts.LocalizationSourceName;
        }
    }
}