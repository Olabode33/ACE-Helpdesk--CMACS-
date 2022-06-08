using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Test.RequestThreads.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.RequestThreads
{
    public interface IRequestThreadsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRequestThreadForView>> GetAll(GetAllRequestThreadsInput input);

		Task<GetRequestThreadForEditOutput> GetRequestThreadForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditRequestThreadDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetRequestThreadsToExcel(GetAllRequestThreadsForExcelInput input);

		
		Task<PagedResultDto<RequestLookupTableDto_4Threads>> GetAllRequestForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<UserLookupTableDto_4Threads>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}