using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Test.Requests.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.Requests
{
    public interface IRequestSubAreaMappingsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRequestSubAreaMappingForViewDto>> GetAll(GetAllRequestSubAreaMappingsInput input);

		Task<GetRequestSubAreaMappingForEditOutput> GetRequestSubAreaMappingForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditRequestSubAreaMappingDto input);

		Task Delete(EntityDto input);

		
		Task<PagedResultDto<RequestSubAreaMappingRequestLookupTableDto>> GetAllRequestForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<RequestSubAreaMappingRequestSubAreaLookupTableDto>> GetAllRequestSubAreaForLookupTable(GetAllForLookupTableInput input);
		
    }
}