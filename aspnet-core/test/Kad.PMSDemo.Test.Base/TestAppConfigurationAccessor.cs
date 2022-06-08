using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using Kad.PMSDemo.Configuration;

namespace Kad.PMSDemo.Test.Base
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(PMSDemoTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}
