using System.Threading.Tasks;
using Abp.Application.Services;
using Kad.PMSDemo.Install.Dto;

namespace Kad.PMSDemo.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}