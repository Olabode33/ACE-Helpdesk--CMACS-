using Test.Requests;
using Test.RequestAreas;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Test.Requests.Dtos;
using Kad.PMSDemo.Dto;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Kad.PMSDemo;

namespace Test.Requests
{
	[AbpAuthorize]
    public class RequestSubAreaMappingsAppService : PMSDemoAppServiceBase, IRequestSubAreaMappingsAppService
    {
		 private readonly IRepository<RequestSubAreaMapping> _requestSubAreaMappingRepository;
		 private readonly IRepository<Request,int> _lookup_requestRepository;
		 private readonly IRepository<RequestSubArea,int> _lookup_requestSubAreaRepository;
		 

		  public RequestSubAreaMappingsAppService(IRepository<RequestSubAreaMapping> requestSubAreaMappingRepository , IRepository<Request, int> lookup_requestRepository, IRepository<RequestSubArea, int> lookup_requestSubAreaRepository) 
		  {
			_requestSubAreaMappingRepository = requestSubAreaMappingRepository;
			_lookup_requestRepository = lookup_requestRepository;
		_lookup_requestSubAreaRepository = lookup_requestSubAreaRepository;
		
		  }

		 public async Task<PagedResultDto<GetRequestSubAreaMappingForViewDto>> GetAll(GetAllRequestSubAreaMappingsInput input)
         {
			
			var filteredRequestSubAreaMappings = _requestSubAreaMappingRepository.GetAll()
						.Include( e => e.RequestFk)
						.Include( e => e.RequestSubAreaFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.RequestRequestNoFilter), e => e.RequestFk != null && e.RequestFk.RequestNo == input.RequestRequestNoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.RequestSubAreaNameFilter), e => e.RequestSubAreaFk != null && e.RequestSubAreaFk.Name == input.RequestSubAreaNameFilter);

			var pagedAndFilteredRequestSubAreaMappings = filteredRequestSubAreaMappings
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var requestSubAreaMappings = from o in pagedAndFilteredRequestSubAreaMappings
                         join o1 in _lookup_requestRepository.GetAll() on o.RequestId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_requestSubAreaRepository.GetAll() on o.RequestSubAreaId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetRequestSubAreaMappingForViewDto() {
							RequestSubAreaMapping = new RequestSubAreaMappingDto
							{
                                Id = o.Id
							},
                         	RequestRequestNo = s1 == null ? "" : s1.RequestNo.ToString(),
                         	RequestSubAreaName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredRequestSubAreaMappings.CountAsync();

            return new PagedResultDto<GetRequestSubAreaMappingForViewDto>(
                totalCount,
                await requestSubAreaMappings.ToListAsync()
            );
         }
		 

		 public async Task<GetRequestSubAreaMappingForEditOutput> GetRequestSubAreaMappingForEdit(EntityDto input)
         {
            var requestSubAreaMapping = await _requestSubAreaMappingRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRequestSubAreaMappingForEditOutput {RequestSubAreaMapping = ObjectMapper.Map<CreateOrEditRequestSubAreaMappingDto>(requestSubAreaMapping)};

		    if (output.RequestSubAreaMapping.RequestId != null)
            {
                var _lookupRequest = await _lookup_requestRepository.FirstOrDefaultAsync((int)output.RequestSubAreaMapping.RequestId);
                output.RequestRequestNo = _lookupRequest.RequestNo.ToString();
            }

		    if (output.RequestSubAreaMapping.RequestSubAreaId != null)
            {
                var _lookupRequestSubArea = await _lookup_requestSubAreaRepository.FirstOrDefaultAsync((int)output.RequestSubAreaMapping.RequestSubAreaId);
                output.RequestSubAreaName = _lookupRequestSubArea.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRequestSubAreaMappingDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }


		 protected virtual async Task Create(CreateOrEditRequestSubAreaMappingDto input)
         {
            var requestSubAreaMapping = ObjectMapper.Map<RequestSubAreaMapping>(input);

			

            await _requestSubAreaMappingRepository.InsertAsync(requestSubAreaMapping);
         }


		 protected virtual async Task Update(CreateOrEditRequestSubAreaMappingDto input)
         {
            var requestSubAreaMapping = await _requestSubAreaMappingRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, requestSubAreaMapping);
         }

         public async Task Delete(EntityDto input)
         {
            await _requestSubAreaMappingRepository.DeleteAsync(input.Id);
         } 


         public async Task<PagedResultDto<RequestSubAreaMappingRequestLookupTableDto>> GetAllRequestForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_requestRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.RequestNo.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var requestList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<RequestSubAreaMappingRequestLookupTableDto>();
			foreach(var request in requestList){
				lookupTableDtoList.Add(new RequestSubAreaMappingRequestLookupTableDto
				{
					Id = request.Id,
					DisplayName = request.RequestNo?.ToString()
				});
			}

            return new PagedResultDto<RequestSubAreaMappingRequestLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }


         public async Task<PagedResultDto<RequestSubAreaMappingRequestSubAreaLookupTableDto>> GetAllRequestSubAreaForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_requestSubAreaRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var requestSubAreaList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<RequestSubAreaMappingRequestSubAreaLookupTableDto>();
			foreach(var requestSubArea in requestSubAreaList){
				lookupTableDtoList.Add(new RequestSubAreaMappingRequestSubAreaLookupTableDto
				{
					Id = requestSubArea.Id,
					DisplayName = requestSubArea.Name?.ToString()
				});
			}

            return new PagedResultDto<RequestSubAreaMappingRequestSubAreaLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}