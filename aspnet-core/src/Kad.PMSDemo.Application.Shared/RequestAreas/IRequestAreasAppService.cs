using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Test.RequestAreas.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.RequestAreas
{
    public interface IRequestAreasAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRequestAreaForView>> GetAll(GetAllRequestAreasInput input);

		Task<GetRequestAreaForEditOutput> GetRequestAreaForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditRequestAreaDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetRequestAreasToExcel(GetAllRequestAreasForExcelInput input);

		
    }
}