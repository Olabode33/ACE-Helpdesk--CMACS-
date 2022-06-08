using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Test.TORApprovals.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.TORApprovals
{
    public interface ITORApprovalsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTORApprovalForView>> GetAll(GetAllTORApprovalsInput input);

		Task<GetTORApprovalForEditOutput> GetTORApprovalForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTORApprovalDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTORApprovalsToExcel(GetAllTORApprovalsForExcelInput input);

		
		Task<PagedResultDto<UserLookupTORTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<RequestLookupTORTableDto>> GetAllRequestForLookupTable(GetAllForLookupTableInput input);
		
    }
}