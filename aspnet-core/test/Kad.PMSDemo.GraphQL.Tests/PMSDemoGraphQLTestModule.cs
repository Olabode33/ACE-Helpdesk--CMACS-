using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Kad.PMSDemo.Configure;
using Kad.PMSDemo.Startup;
using Kad.PMSDemo.Test.Base;

namespace Kad.PMSDemo.GraphQL.Tests
{
    [DependsOn(
        typeof(PMSDemoGraphQLModule),
        typeof(PMSDemoTestBaseModule))]
    public class PMSDemoGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(PMSDemoGraphQLTestModule).GetAssembly());
        }
    }
}