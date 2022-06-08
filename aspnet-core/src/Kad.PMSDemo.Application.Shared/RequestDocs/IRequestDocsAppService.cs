using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Test.RequestDocs.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.RequestDocs
{
    public interface IRequestDocsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRequestDocForView>> GetAll(GetAllRequestDocsInput input);

		Task<GetRequestDocForEditOutput> GetRequestDocForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditRequestDocDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetRequestDocsToExcel(GetAllRequestDocsForExcelInput input);

		
		Task<PagedResultDto<RequestForDocLookupTableDto>> GetAllRequestForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<UserForDocLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}