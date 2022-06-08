using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Kad.PMSDemo
{
    [DependsOn(typeof(PMSDemoCoreSharedModule))]
    public class PMSDemoApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PMSDemoApplicationSharedModule).GetAssembly());
        }
    }
}