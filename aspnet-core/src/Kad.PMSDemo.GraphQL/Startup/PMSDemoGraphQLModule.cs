using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Kad.PMSDemo.Startup
{
    [DependsOn(typeof(PMSDemoCoreModule))]
    public class PMSDemoGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PMSDemoGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}