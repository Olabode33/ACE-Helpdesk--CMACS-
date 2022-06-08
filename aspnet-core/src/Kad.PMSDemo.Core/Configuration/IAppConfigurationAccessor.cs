using Microsoft.Extensions.Configuration;

namespace Kad.PMSDemo.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
