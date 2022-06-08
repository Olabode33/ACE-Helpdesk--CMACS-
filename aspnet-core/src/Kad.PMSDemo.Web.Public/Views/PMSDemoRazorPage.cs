using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace Kad.PMSDemo.Web.Public.Views
{
    public abstract class PMSDemoRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected PMSDemoRazorPage()
        {
            LocalizationSourceName = PMSDemoConsts.LocalizationSourceName;
        }
    }
}
