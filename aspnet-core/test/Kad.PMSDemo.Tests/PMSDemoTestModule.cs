using Abp.Modules;
using Kad.PMSDemo.Test.Base;

namespace Kad.PMSDemo.Tests
{
    [DependsOn(typeof(PMSDemoTestBaseModule))]
    public class PMSDemoTestModule : AbpModule
    {
       
    }
}
