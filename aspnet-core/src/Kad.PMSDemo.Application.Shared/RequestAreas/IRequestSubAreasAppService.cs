using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Test.RequestAreas.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.RequestAreas
{
    public interface IRequestSubAreasAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRequestSubAreaForViewDto>> GetAll(GetAllRequestSubAreasInput input);

		Task<GetRequestSubAreaForEditOutput> GetRequestSubAreaForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditRequestSubAreaDto input);

		Task Delete(EntityDto input);

		
		Task<PagedResultDto<RequestSubAreaRequestAreaLookupTableDto>> GetAllRequestAreaForLookupTable(GetAllForLookupTableInput input);
		
    }
}