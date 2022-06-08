using Test.RequestAreas;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Test.RequestAreas.Dtos;
using Kad.PMSDemo.Dto;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Kad.PMSDemo;

namespace Test.RequestAreas
{
	[AbpAuthorize]
    public class RequestSubAreasAppService : PMSDemoAppServiceBase, IRequestSubAreasAppService
    {
		 private readonly IRepository<RequestSubArea> _requestSubAreaRepository;
		 private readonly IRepository<RequestArea,int> _lookup_requestAreaRepository;
		 

		  public RequestSubAreasAppService(IRepository<RequestSubArea> requestSubAreaRepository , IRepository<RequestArea, int> lookup_requestAreaRepository) 
		  {
			_requestSubAreaRepository = requestSubAreaRepository;
			_lookup_requestAreaRepository = lookup_requestAreaRepository;
		
		  }

		 public async Task<PagedResultDto<GetRequestSubAreaForViewDto>> GetAll(GetAllRequestSubAreasInput input)
         {
			
			var filteredRequestSubAreas = _requestSubAreaRepository.GetAll()
						.Include( e => e.RequestAreaFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RequestAreaRequestAreaNameFilter), e => e.RequestAreaFk != null && e.RequestAreaFk.RequestAreaName == input.RequestAreaRequestAreaNameFilter);

			var pagedAndFilteredRequestSubAreas = filteredRequestSubAreas
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var requestSubAreas = from o in pagedAndFilteredRequestSubAreas
                         join o1 in _lookup_requestAreaRepository.GetAll() on o.RequestAreaId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetRequestSubAreaForViewDto() {
							RequestSubArea = new RequestSubAreaDto
							{
                                Name = o.Name,
                                Id = o.Id
							},
                         	RequestAreaRequestAreaName = s1 == null ? "" : s1.RequestAreaName.ToString()
						};

            var totalCount = await filteredRequestSubAreas.CountAsync();

            return new PagedResultDto<GetRequestSubAreaForViewDto>(
                totalCount,
                await requestSubAreas.ToListAsync()
            );
         }
		 
		 public async Task<GetRequestSubAreaForEditOutput> GetRequestSubAreaForEdit(EntityDto input)
         {
            var requestSubArea = await _requestSubAreaRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRequestSubAreaForEditOutput {RequestSubArea = ObjectMapper.Map<CreateOrEditRequestSubAreaDto>(requestSubArea)};

		    if (output.RequestSubArea.RequestAreaId != null)
            {
                var _lookupRequestArea = await _lookup_requestAreaRepository.FirstOrDefaultAsync((int)output.RequestSubArea.RequestAreaId);
                output.RequestAreaRequestAreaName = _lookupRequestArea.RequestAreaName.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRequestSubAreaDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
		 protected virtual async Task Create(CreateOrEditRequestSubAreaDto input)
         {
            var requestSubArea = ObjectMapper.Map<RequestSubArea>(input);

			

            await _requestSubAreaRepository.InsertAsync(requestSubArea);
         }

		 [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
		 protected virtual async Task Update(CreateOrEditRequestSubAreaDto input)
         {
            var requestSubArea = await _requestSubAreaRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, requestSubArea);
         }

		 [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
         public async Task Delete(EntityDto input)
         {
            await _requestSubAreaRepository.DeleteAsync(input.Id);
         } 

         public async Task<PagedResultDto<RequestSubAreaRequestAreaLookupTableDto>> GetAllRequestAreaForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_requestAreaRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.RequestAreaName.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var requestAreaList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<RequestSubAreaRequestAreaLookupTableDto>();
			foreach(var requestArea in requestAreaList){
				lookupTableDtoList.Add(new RequestSubAreaRequestAreaLookupTableDto
				{
					Id = requestArea.Id,
					DisplayName = requestArea.RequestAreaName?.ToString()
				});
			}

            return new PagedResultDto<RequestSubAreaRequestAreaLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}