using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Test.TechTeams.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.TechTeams
{
    public interface ITechTeamsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTechTeamForView>> GetAll(GetAllTechTeamsInput input);

		Task<GetTechTeamForEditOutput> GetTechTeamForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTechTeamDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTechTeamsToExcel(GetAllTechTeamsForExcelInput input);

		
		Task<PagedResultDto<RequestLookupTableDto>> GetAllRequestForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<UserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}