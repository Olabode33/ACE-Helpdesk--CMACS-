using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Test.RequestApprovals.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.RequestApprovals
{
    public interface IRequestApprovalsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRequestApprovalForView>> GetAll(GetAllRequestApprovalsInput input);

		Task<GetRequestApprovalForEditOutput> GetRequestApprovalForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditRequestApprovalDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetRequestApprovalsToExcel(GetAllRequestApprovalsForExcelInput input);

		
		Task<PagedResultDto<RequestForApprovalLookupTableDto>> GetAllRequestForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<UserForApprovalLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}