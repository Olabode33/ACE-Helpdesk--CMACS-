using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Kad.PMSDemo
{
    public class PMSDemoClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PMSDemoClientModule).GetAssembly());
        }
    }
}
