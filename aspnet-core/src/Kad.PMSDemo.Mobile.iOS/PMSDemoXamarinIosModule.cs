using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Kad.PMSDemo
{
    [DependsOn(typeof(PMSDemoXamarinSharedModule))]
    public class PMSDemoXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PMSDemoXamarinIosModule).GetAssembly());
        }
    }
}