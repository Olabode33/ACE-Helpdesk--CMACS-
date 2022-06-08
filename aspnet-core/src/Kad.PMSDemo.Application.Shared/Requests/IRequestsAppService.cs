using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Test.Requests.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.Requests
{
    public interface IRequestsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRequestForView>> GetAll(GetAllRequestsInput input);

		Task<GetRequestForEditOutput> GetRequestForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditRequestDto input);

		Task Delete(EntityDto input);

        //Task UpdateRequestStatus(int requestid, RequestStatus status);

        Task<FileDto> GetRequestsToExcel(GetAllRequestsForExcelInput input);
		
		Task<PagedResultDto<RequestAreaLookupTableDto>> GetAllRequestAreaForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<RequestDomainLookupTableDto>> GetAllRequestDomainForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<UserLookupTableDto_4Tech>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ClientListLookupTableDto>> GetAllClientListForLookupTable(GetAllForLookupTableInput input);
		
    }
}