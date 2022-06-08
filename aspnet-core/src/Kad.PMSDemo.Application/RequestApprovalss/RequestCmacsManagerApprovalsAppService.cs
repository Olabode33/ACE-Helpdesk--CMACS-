using Test.Requests;
using Kad.PMSDemo.Authorization.Users;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Kad.PMSDemo.Dto;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Test.RequestApprovals.Dtos;
using Kad.PMSDemo;

namespace Test.RequestApprovals
{
	[AbpAuthorize]
    public class RequestCmacsManagerApprovalsAppService : PMSDemoAppServiceBase, IRequestCmacsManagerApprovalsAppService
    {
		 private readonly IRepository<RequestCmacsManagerApproval> _requestCmacsManagerApprovalRepository;
		 private readonly IRepository<Request,int> _lookup_requestRepository;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public RequestCmacsManagerApprovalsAppService(IRepository<RequestCmacsManagerApproval> requestCmacsManagerApprovalRepository , IRepository<Request, int> lookup_requestRepository, IRepository<User, long> lookup_userRepository) 
		  {
			_requestCmacsManagerApprovalRepository = requestCmacsManagerApprovalRepository;
			_lookup_requestRepository = lookup_requestRepository;
		_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetRequestCmacsManagerApprovalForViewDto>> GetAll(GetAllRequestCmacsManagerApprovalsInput input)
         {
			
			var filteredRequestCmacsManagerApprovals = _requestCmacsManagerApprovalRepository.GetAll()
						.Include( e => e.RequestFk)
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.RequestRequestNoFilter), e => e.RequestFk != null && e.RequestFk.RequestNo.ToLower() == input.RequestRequestNoFilter.ToLower().Trim())
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name.ToLower() == input.UserNameFilter.ToLower().Trim());

			var pagedAndFilteredRequestCmacsManagerApprovals = filteredRequestCmacsManagerApprovals
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var requestCmacsManagerApprovals = from o in pagedAndFilteredRequestCmacsManagerApprovals
                         join o1 in _lookup_requestRepository.GetAll() on o.RequestId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetRequestCmacsManagerApprovalForViewDto() {
							RequestCmacsManagerApproval = new RequestCmacsManagerApprovalDto
							{
                                Approved = o.Approved,
                                ApprovedTime = o.ApprovedTime,
                                Id = o.Id
							},
                         	RequestRequestNo = s1 == null ? "" : s1.RequestNo.ToString(),
                         	UserName = s2 == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredRequestCmacsManagerApprovals.CountAsync();

            return new PagedResultDto<GetRequestCmacsManagerApprovalForViewDto>(
                totalCount,
                await requestCmacsManagerApprovals.ToListAsync()
            );
         }
		 

		 public async Task<GetRequestCmacsManagerApprovalForEditOutput> GetRequestCmacsManagerApprovalForEdit(EntityDto input)
         {
            var requestCmacsManagerApproval = await _requestCmacsManagerApprovalRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRequestCmacsManagerApprovalForEditOutput {RequestCmacsManagerApproval = ObjectMapper.Map<CreateOrEditRequestCmacsManagerApprovalDto>(requestCmacsManagerApproval)};

		    if (output.RequestCmacsManagerApproval.RequestId != null)
            {
                var _lookupRequest = await _lookup_requestRepository.FirstOrDefaultAsync((int)output.RequestCmacsManagerApproval.RequestId);
                output.RequestRequestNo = _lookupRequest.RequestNo.ToString();
            }

		    if (output.RequestCmacsManagerApproval.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.RequestCmacsManagerApproval.UserId);
                output.UserName = _lookupUser.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRequestCmacsManagerApprovalDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 
		 protected virtual async Task Create(CreateOrEditRequestCmacsManagerApprovalDto input)
         {
            var requestCmacsManagerApproval = ObjectMapper.Map<RequestCmacsManagerApproval>(input);

			

            await _requestCmacsManagerApprovalRepository.InsertAsync(requestCmacsManagerApproval);
         }

		 
		 protected virtual async Task Update(CreateOrEditRequestCmacsManagerApprovalDto input)
         {
            var requestCmacsManagerApproval = await _requestCmacsManagerApprovalRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, requestCmacsManagerApproval);
         }

		 
         public async Task Delete(EntityDto input)
         {
            await _requestCmacsManagerApprovalRepository.DeleteAsync(input.Id);
         } 

		
         public async Task<PagedResultDto<RequestCmacsManagerApprovalRequestLookupTableDto>> GetAllRequestForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_requestRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.RequestNo.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var requestList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<RequestCmacsManagerApprovalRequestLookupTableDto>();
			foreach(var request in requestList){
				lookupTableDtoList.Add(new RequestCmacsManagerApprovalRequestLookupTableDto
				{
					Id = request.Id,
					DisplayName = request.RequestNo?.ToString()
				});
			}

            return new PagedResultDto<RequestCmacsManagerApprovalRequestLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }


         public async Task<PagedResultDto<RequestCmacsManagerApprovalUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<RequestCmacsManagerApprovalUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new RequestCmacsManagerApprovalUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<RequestCmacsManagerApprovalUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}