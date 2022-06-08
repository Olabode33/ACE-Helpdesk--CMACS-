using System;
using Kad.PMSDemo.Core;
using Kad.PMSDemo.Core.Dependency;
using Kad.PMSDemo.Services.Permission;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Kad.PMSDemo.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class HasPermissionExtension : IMarkupExtension
    {
        public string Text { get; set; }
        
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ApplicationBootstrapper.AbpBootstrapper == null || Text == null)
            {
                return false;
            }

            var permissionService = DependencyResolver.Resolve<IPermissionService>();
            return permissionService.HasPermission(Text);
        }
    }
}