using System.Threading.Tasks;
using Abp.Application.Services;
using Kad.PMSDemo.Configuration.Host.Dto;

namespace Kad.PMSDemo.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
