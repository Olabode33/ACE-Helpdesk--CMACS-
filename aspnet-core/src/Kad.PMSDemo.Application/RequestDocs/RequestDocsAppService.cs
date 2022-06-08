using Test.Requests;
using Kad.PMSDemo.Authorization.Users;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Test.RequestDocs.Exporting;
using Test.RequestDocs.Dtos;
using Kad.PMSDemo.Dto;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Kad.PMSDemo;

namespace Test.RequestDocs
{
	[AbpAuthorize]
    public class RequestDocsAppService : PMSDemoAppServiceBase, IRequestDocsAppService
    {
		 private readonly IRepository<RequestDoc> _requestDocRepository;
		 private readonly IRequestDocsExcelExporter _requestDocsExcelExporter;
		 private readonly IRepository<Request,int> _requestRepository;
		 private readonly IRepository<User,long> _userRepository;
		 

		  public RequestDocsAppService(IRepository<RequestDoc> requestDocRepository, IRequestDocsExcelExporter requestDocsExcelExporter , IRepository<Request, int> requestRepository, IRepository<User, long> userRepository) 
		  {
			_requestDocRepository = requestDocRepository;
			_requestDocsExcelExporter = requestDocsExcelExporter;
			_requestRepository = requestRepository;
		_userRepository = userRepository;
		
		  }

		 public async Task<PagedResultDto<GetRequestDocForView>> GetAll(GetAllRequestDocsInput input)
         {
			var preparerTypeIdFilter = (StaffEntityType) input.PreparerTypeIdFilter;

            var filtered = _requestDocRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentName.Contains(input.Filter) || e.DocumentLocation.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentNameFilter), e => e.DocumentName.ToLower() == input.DocumentNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentLocationFilter), e => e.DocumentLocation.ToLower() == input.DocumentLocationFilter.ToLower().Trim())
                        .WhereIf(input.PreparerTypeIdFilter > -1, e => e.PreparerTypeId == preparerTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentGUIDFilter.ToString()), e => e.DocumentGUID.ToString() == input.DocumentGUIDFilter.ToString().Trim());


            var pagedAndFiltered = filtered
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var query = (from o in pagedAndFiltered
                         join o1 in _requestRepository.GetAll() on o.RequestId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _userRepository.GetAll() on o.PreparerId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetRequestDocForView()
                         {
                             RequestDoc = ObjectMapper.Map<RequestDocDto>(o),
                             RequestLocalChargeCode = s1 == null ? "" : s1.LocalChargeCode.ToString(),
                             UserName = s2 == null ? "" : s2.Name.ToString()
                         })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.RequestLocalChargeCodeFilter), e => e.RequestLocalChargeCode.ToLower() == input.RequestLocalChargeCodeFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserName.ToLower() == input.UserNameFilter.ToLower().Trim());


            var totalCount = await filtered.CountAsync();

            return new PagedResultDto<GetRequestDocForView>(
                totalCount,
                await query.ToListAsync()
            );
         }
		 

		 public async Task<GetRequestDocForEditOutput> GetRequestDocForEdit(EntityDto input)
         {
            var requestDoc = await _requestDocRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRequestDocForEditOutput {RequestDoc = ObjectMapper.Map<CreateOrEditRequestDocDto>(requestDoc)};

		    if (output.RequestDoc.RequestId != null)
            {
                var request = await _requestRepository.FirstOrDefaultAsync((int)output.RequestDoc.RequestId);
                output.RequestLocalChargeCode = request.LocalChargeCode.ToString();
            }

		    if (output.RequestDoc.PreparerId != null)
            {
                var user = await _userRepository.FirstOrDefaultAsync((long)output.RequestDoc.PreparerId);
                output.UserName = user.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRequestDocDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }


		 private async Task Create(CreateOrEditRequestDocDto input)
         {
            var requestDoc = ObjectMapper.Map<RequestDoc>(input);

			
			if (AbpSession.TenantId != null)
			{
				requestDoc.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _requestDocRepository.InsertAsync(requestDoc);
         }


		 private async Task Update(CreateOrEditRequestDocDto input)
         {
            var requestDoc = await _requestDocRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, requestDoc);
         }


         public async Task Delete(EntityDto input)
         {
            await _requestDocRepository.DeleteAsync(input.Id);
         }

		 public async Task<FileDto> GetRequestDocsToExcel(GetAllRequestDocsForExcelInput input)
         {
			var preparerTypeIdFilter = (StaffEntityType) input.PreparerTypeIdFilter;

            var filtered = _requestDocRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.DocumentName.Contains(input.Filter) || e.DocumentLocation.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentNameFilter), e => e.DocumentName.ToLower() == input.DocumentNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentLocationFilter), e => e.DocumentLocation.ToLower() == input.DocumentLocationFilter.ToLower().Trim())
                        .WhereIf(input.PreparerTypeIdFilter > -1, e => e.PreparerTypeId == preparerTypeIdFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DocumentGUIDFilter.ToString()), e => e.DocumentGUID.ToString() == input.DocumentGUIDFilter.ToString().Trim());

            var query = (from o in filtered
                         join o1 in _requestRepository.GetAll() on o.RequestId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _userRepository.GetAll() on o.PreparerId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetRequestDocForView()
                         {
                             RequestDoc = ObjectMapper.Map<RequestDocDto>(o),
                             RequestLocalChargeCode = s1 == null ? "" : s1.LocalChargeCode.ToString(),
                             UserName = s2 == null ? "" : s2.Name.ToString()
                         })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.RequestLocalChargeCodeFilter), e => e.RequestLocalChargeCode.ToLower() == input.RequestLocalChargeCodeFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserName.ToLower() == input.UserNameFilter.ToLower().Trim());

            var requestDocListDtos = await query.ToListAsync();

            return _requestDocsExcelExporter.ExportToFile(requestDocListDtos);
         }


         public async Task<PagedResultDto<RequestForDocLookupTableDto>> GetAllRequestForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _requestRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.LocalChargeCode.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var requestList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<RequestForDocLookupTableDto>();
			foreach(var request in requestList){
				lookupTableDtoList.Add(new RequestForDocLookupTableDto
				{
					Id = request.Id,
					DisplayName = request.LocalChargeCode.ToString()
				});
			}

            return new PagedResultDto<RequestForDocLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }		

         public async Task<PagedResultDto<UserForDocLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UserForDocLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new UserForDocLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name.ToString()
				});
			}

            return new PagedResultDto<UserForDocLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}