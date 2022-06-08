using Abp.AspNetCore.Mvc.Views;

namespace Kad.PMSDemo.Web.Views
{
    public abstract class PMSDemoRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected PMSDemoRazorPage()
        {
            LocalizationSourceName = PMSDemoConsts.LocalizationSourceName;
        }
    }
}
