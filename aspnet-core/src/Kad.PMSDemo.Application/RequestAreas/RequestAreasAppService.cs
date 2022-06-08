
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Test.RequestAreas.Exporting;
using Test.RequestAreas.Dtos;
using Kad.PMSDemo.Dto;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Kad.PMSDemo;

namespace Test.RequestAreas
{
	[AbpAuthorize]
    public class RequestAreasAppService : PMSDemoAppServiceBase, IRequestAreasAppService
    {
		 private readonly IRepository<RequestArea> _requestAreaRepository;
		 private readonly IRequestAreasExcelExporter _requestAreasExcelExporter;
		 

		  public RequestAreasAppService(IRepository<RequestArea> requestAreaRepository, IRequestAreasExcelExporter requestAreasExcelExporter ) 
		  {
			_requestAreaRepository = requestAreaRepository;
			_requestAreasExcelExporter = requestAreasExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetRequestAreaForView>> GetAll(GetAllRequestAreasInput input)
         {
            var filtered = _requestAreaRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.RequestAreaName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RequestAreaNameFilter), e => e.RequestAreaName.ToLower() == input.RequestAreaNameFilter.ToLower().Trim());


            var pagedAndFiltered = filtered
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var query = (from o in pagedAndFiltered

                         select new GetRequestAreaForView()
                         {
                             RequestArea = ObjectMapper.Map<RequestAreaDto>(o)
                         });

            var totalCount = await filtered.CountAsync();

            return new PagedResultDto<GetRequestAreaForView>(
                totalCount,
                await query.ToListAsync()
            );
         }
		 

		 public async Task<GetRequestAreaForEditOutput> GetRequestAreaForEdit(EntityDto input)
         {
            var requestArea = await _requestAreaRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRequestAreaForEditOutput {RequestArea = ObjectMapper.Map<CreateOrEditRequestAreaDto>(requestArea)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRequestAreaDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
		 private async Task Create(CreateOrEditRequestAreaDto input)
         {
            var requestArea = ObjectMapper.Map<RequestArea>(input);

			
			if (AbpSession.TenantId != null)
			{
				requestArea.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _requestAreaRepository.InsertAsync(requestArea);
         }

		 [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
		 private async Task Update(CreateOrEditRequestAreaDto input)
         {
            var requestArea = await _requestAreaRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, requestArea);
         }

		 [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
         public async Task Delete(EntityDto input)
         {
            await _requestAreaRepository.DeleteAsync(input.Id);
         }

		 public async Task<FileDto> GetRequestAreasToExcel(GetAllRequestAreasForExcelInput input)
         {
            var filtered = _requestAreaRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.RequestAreaName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RequestAreaNameFilter), e => e.RequestAreaName.ToLower() == input.RequestAreaNameFilter.ToLower().Trim());

            var query = (from o in filtered

                         select new GetRequestAreaForView()
                         {
                             RequestArea = ObjectMapper.Map<RequestAreaDto>(o)
                         });


            var requestAreaListDtos = await query.ToListAsync();

            return _requestAreasExcelExporter.ExportToFile(requestAreaListDtos);
         }


    }
}