using System.Threading.Tasks;
using Abp.Application.Services;
using Kad.PMSDemo.Editions.Dto;
using Kad.PMSDemo.MultiTenancy.Dto;

namespace Kad.PMSDemo.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}