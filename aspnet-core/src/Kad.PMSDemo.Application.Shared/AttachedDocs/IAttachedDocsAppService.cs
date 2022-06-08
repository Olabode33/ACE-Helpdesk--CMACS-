using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Dto;
using Test.AttachedDocs.Dtos;

namespace Test.AttachedDocs
{
    public interface IAttachedDocsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetAttachedDocForView>> GetAll(GetAllAttachedDocsInput input);

		Task<GetAttachedDocForEditOutput> GetAttachedDocForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditAttachedDocDto input);

        Task CreateOrEditMultipleDoc(List<CreateOrEditAttachedDocDto> input);


        Task Delete(EntityDto input);

		Task<FileDto> GetAttachedDocsToExcel(GetAllAttachedDocsForExcelInput input);

		
		Task<PagedResultDto<RequestIdLookupTableDto>> GetAllRequestForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<UserIdLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<BinaryObjectLookupTableDto>> GetAllBinaryObjectForLookupTable(GetAllForLookupTableInput input);
		
    }
}