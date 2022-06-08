using Abp.Application.Services;
using Kad.PMSDemo.Dto;
using Kad.PMSDemo.Logging.Dto;

namespace Kad.PMSDemo.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
