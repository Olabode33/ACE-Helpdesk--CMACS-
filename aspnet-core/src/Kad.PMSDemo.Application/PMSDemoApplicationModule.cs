using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Kad.PMSDemo.Authorization;

namespace Kad.PMSDemo
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(PMSDemoApplicationSharedModule),
        typeof(PMSDemoCoreModule)
        )]
    public class PMSDemoApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PMSDemoApplicationModule).GetAssembly());
        }
    }
}