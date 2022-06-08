using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Test.ReportingTerritories.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.ReportingTerritories
{
    public interface IReportingTerritoriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetReportingTerritoryForView>> GetAll(GetAllReportingTerritoriesInput input);

		Task<GetReportingTerritoryForEditOutput> GetReportingTerritoryForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditReportingTerritoryDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetReportingTerritoriesToExcel(GetAllReportingTerritoriesForExcelInput input);

		
    }
}