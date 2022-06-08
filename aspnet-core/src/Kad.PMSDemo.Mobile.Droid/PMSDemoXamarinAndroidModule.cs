using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Kad.PMSDemo
{
    [DependsOn(typeof(PMSDemoXamarinSharedModule))]
    public class PMSDemoXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PMSDemoXamarinAndroidModule).GetAssembly());
        }
    }
}