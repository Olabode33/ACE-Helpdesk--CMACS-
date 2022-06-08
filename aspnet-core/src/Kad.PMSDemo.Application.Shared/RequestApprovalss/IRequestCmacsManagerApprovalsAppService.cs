using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Dto;
using Test.RequestApprovals.Dtos;

namespace Test.RequestApprovals
{
    public interface IRequestCmacsManagerApprovalsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRequestCmacsManagerApprovalForViewDto>> GetAll(GetAllRequestCmacsManagerApprovalsInput input);

		Task<GetRequestCmacsManagerApprovalForEditOutput> GetRequestCmacsManagerApprovalForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditRequestCmacsManagerApprovalDto input);

		Task Delete(EntityDto input);

		
		Task<PagedResultDto<RequestCmacsManagerApprovalRequestLookupTableDto>> GetAllRequestForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<RequestCmacsManagerApprovalUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}