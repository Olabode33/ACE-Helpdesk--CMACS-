using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization.Permissions.Dto;

namespace Kad.PMSDemo.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
