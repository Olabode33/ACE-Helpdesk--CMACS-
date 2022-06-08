
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Test.RequestDomains.Exporting;
using Test.RequestDomains.Dtos;
using Kad.PMSDemo.Dto;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Kad.PMSDemo;

namespace Test.RequestDomains
{
	[AbpAuthorize]
    public class RequestDomainsAppService : PMSDemoAppServiceBase, IRequestDomainsAppService
    {
		 private readonly IRepository<RequestDomain> _requestDomainRepository;
		 private readonly IRequestDomainsExcelExporter _requestDomainsExcelExporter;
		 

		  public RequestDomainsAppService(IRepository<RequestDomain> requestDomainRepository, IRequestDomainsExcelExporter requestDomainsExcelExporter ) 
		  {
			_requestDomainRepository = requestDomainRepository;
			_requestDomainsExcelExporter = requestDomainsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetRequestDomainForView>> GetAll(GetAllRequestDomainsInput input)
         {

            var filtered = _requestDomainRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DomainName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DomainNameFilter), e => e.DomainName.ToLower() == input.DomainNameFilter.ToLower().Trim());

            var pagedAndFiltered = filtered
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var query = (from o in pagedAndFiltered

                         select new GetRequestDomainForView()
                         {
                             RequestDomain = ObjectMapper.Map<RequestDomainDto>(o)
                         });

            var totalCount = await filtered.CountAsync();

            return new PagedResultDto<GetRequestDomainForView>(
                totalCount,
                await query.ToListAsync()
            );
         }
		 
		 public async Task<GetRequestDomainForEditOutput> GetRequestDomainForEdit(EntityDto input)
         {
            var requestDomain = await _requestDomainRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRequestDomainForEditOutput {RequestDomain = ObjectMapper.Map<CreateOrEditRequestDomainDto>(requestDomain)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRequestDomainDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
		 private async Task Create(CreateOrEditRequestDomainDto input)
         {
            var requestDomain = ObjectMapper.Map<RequestDomain>(input);

			
			if (AbpSession.TenantId != null)
			{
				requestDomain.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _requestDomainRepository.InsertAsync(requestDomain);
         }

		 [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
		 private async Task Update(CreateOrEditRequestDomainDto input)
         {
            var requestDomain = await _requestDomainRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, requestDomain);
         }

		 [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
         public async Task Delete(EntityDto input)
         {
            await _requestDomainRepository.DeleteAsync(input.Id);
         }

		 public async Task<FileDto> GetRequestDomainsToExcel(GetAllRequestDomainsForExcelInput input)
         {
            var filtered = _requestDomainRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DomainName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DomainNameFilter), e => e.DomainName.ToLower() == input.DomainNameFilter.ToLower().Trim());

            var query = (from o in filtered

                         select new GetRequestDomainForView()
                         {
                             RequestDomain = ObjectMapper.Map<RequestDomainDto>(o)
                         });

            var requestDomainListDtos = await query.ToListAsync();

            return _requestDomainsExcelExporter.ExportToFile(requestDomainListDtos);
         }


    }
}