using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Test.RequestDomains.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.RequestDomains
{
    public interface IRequestDomainsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetRequestDomainForView>> GetAll(GetAllRequestDomainsInput input);

		Task<GetRequestDomainForEditOutput> GetRequestDomainForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditRequestDomainDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetRequestDomainsToExcel(GetAllRequestDomainsForExcelInput input);

		
    }
}