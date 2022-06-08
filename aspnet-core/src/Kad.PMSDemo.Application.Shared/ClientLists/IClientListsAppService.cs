using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Test.ClientLists.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.ClientLists
{
    public interface IClientListsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetClientListForView>> GetAll(GetAllClientListsInput input);

		Task<GetClientListForEditOutput> GetClientListForEdit(EntityDto input);

		Task<CreateOrEditClientListDto> CreateOrEdit(CreateOrEditClientListDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetClientListsToExcel(GetAllClientListsForExcelInput input);

		Task<PagedResultDto<IndustryLookupTableDto>> GetAllIndustryForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ReportingTerritoryLookupTableDto>> GetAllReportingTerritoryForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<StockExchangeLookupTableDto>> GetAllStockExchangeForLookupTable(GetAllForLookupTableInput input);
		
    }
}