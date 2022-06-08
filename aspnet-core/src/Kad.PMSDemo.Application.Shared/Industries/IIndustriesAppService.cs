using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Test.Industries.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.Industries
{
    public interface IIndustriesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetIndustryForView>> GetAll(GetAllIndustriesInput input);

		Task<GetIndustryForEditOutput> GetIndustryForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditIndustryDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetIndustriesToExcel(GetAllIndustriesForExcelInput input);

		
    }
}