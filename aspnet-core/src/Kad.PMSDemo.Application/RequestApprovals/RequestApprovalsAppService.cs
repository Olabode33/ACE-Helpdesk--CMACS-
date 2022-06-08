using Test.Requests;
using Kad.PMSDemo.Authorization.Users;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Test.RequestApprovals.Exporting;
using Test.RequestApprovals.Dtos;
using Kad.PMSDemo.Dto;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Kad.PMSDemo;

namespace Test.RequestApprovals
{
	[AbpAuthorize]
    public class RequestApprovalsAppService : PMSDemoAppServiceBase, IRequestApprovalsAppService
    {
		 private readonly IRepository<RequestApproval> _requestApprovalRepository;
		 private readonly IRequestApprovalsExcelExporter _requestApprovalsExcelExporter;
		 private readonly IRepository<Request,int> _requestRepository;
		 private readonly IRepository<User,long> _userRepository;
		 

		  public RequestApprovalsAppService(IRepository<RequestApproval> requestApprovalRepository, IRequestApprovalsExcelExporter requestApprovalsExcelExporter , IRepository<Request, int> requestRepository, IRepository<User, long> userRepository) 
		  {
			_requestApprovalRepository = requestApprovalRepository;
			_requestApprovalsExcelExporter = requestApprovalsExcelExporter;
			_requestRepository = requestRepository;
		_userRepository = userRepository;
		
		  }

		 public async Task<PagedResultDto<GetRequestApprovalForView>> GetAll(GetAllRequestApprovalsInput input)
         {

            var filtered = _requestApprovalRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinApprovalSentTimeFilter != null, e => e.ApprovalSentTime >= input.MinApprovalSentTimeFilter)
                        .WhereIf(input.MaxApprovalSentTimeFilter != null, e => e.ApprovalSentTime <= input.MaxApprovalSentTimeFilter)
                        .WhereIf(input.ApprovedFilter > -1, e => Convert.ToInt32(e.Approved) == input.ApprovedFilter)
                        .WhereIf(input.MinApprovedTimeFilter != null, e => e.ApprovedTime >= input.MinApprovedTimeFilter)
                        .WhereIf(input.MaxApprovedTimeFilter != null, e => e.ApprovedTime <= input.MaxApprovedTimeFilter);

            var pagedAndFiltered = filtered
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var query = (from o in pagedAndFiltered
                         join o1 in _requestRepository.GetAll() on o.RequestId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _userRepository.GetAll() on o.ApproverId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetRequestApprovalForView()
                         {
                             RequestApproval = ObjectMapper.Map<RequestApprovalDto>(o),
                             RequestRequestNo = s1 == null ? "" : s1.RequestNo.ToString(),
                             UserName = s2 == null ? "" : s2.Name.ToString()
                         })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.RequestRequestNoFilter), e => e.RequestRequestNo.ToLower() == input.RequestRequestNoFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserName.ToLower() == input.UserNameFilter.ToLower().Trim());

            var totalCount = await filtered.CountAsync();

            return new PagedResultDto<GetRequestApprovalForView>(
                totalCount,
                await query.ToListAsync()
            );
         }
		 

		 public async Task<GetRequestApprovalForEditOutput> GetRequestApprovalForEdit(EntityDto input)
         {
            var requestApproval = await _requestApprovalRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRequestApprovalForEditOutput {RequestApproval = ObjectMapper.Map<CreateOrEditRequestApprovalDto>(requestApproval)};

		    if (output.RequestApproval.RequestId != null)
            {
                var request = await _requestRepository.FirstOrDefaultAsync((int)output.RequestApproval.RequestId);
                output.RequestRequestNo = request.RequestNo.ToString();
            }

		    if (output.RequestApproval.ApproverId != null)
            {
                var user = await _userRepository.FirstOrDefaultAsync((long)output.RequestApproval.ApproverId);
                output.UserName = user.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRequestApprovalDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }


		 private async Task Create(CreateOrEditRequestApprovalDto input)
         {
            var requestApproval = ObjectMapper.Map<RequestApproval>(input);

			
			if (AbpSession.TenantId != null)
			{
				requestApproval.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _requestApprovalRepository.InsertAsync(requestApproval);
         }

		 private async Task Update(CreateOrEditRequestApprovalDto input)
         {
            var requestApproval = await _requestApprovalRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, requestApproval);
         }


         public async Task Delete(EntityDto input)
         {
            await _requestApprovalRepository.DeleteAsync(input.Id);
         }

		 public async Task<FileDto> GetRequestApprovalsToExcel(GetAllRequestApprovalsForExcelInput input)
         {
            var filtered = _requestApprovalRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinApprovalSentTimeFilter != null, e => e.ApprovalSentTime >= input.MinApprovalSentTimeFilter)
                        .WhereIf(input.MaxApprovalSentTimeFilter != null, e => e.ApprovalSentTime <= input.MaxApprovalSentTimeFilter)
                        .WhereIf(input.ApprovedFilter > -1, e => Convert.ToInt32(e.Approved) == input.ApprovedFilter)
                        .WhereIf(input.MinApprovedTimeFilter != null, e => e.ApprovedTime >= input.MinApprovedTimeFilter)
                        .WhereIf(input.MaxApprovedTimeFilter != null, e => e.ApprovedTime <= input.MaxApprovedTimeFilter);

            var query = (from o in filtered
                         join o1 in _requestRepository.GetAll() on o.RequestId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _userRepository.GetAll() on o.ApproverId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetRequestApprovalForView()
                         {
                             RequestApproval = ObjectMapper.Map<RequestApprovalDto>(o),
                             RequestRequestNo = s1 == null ? "" : s1.RequestNo.ToString(),
                             UserName = s2 == null ? "" : s2.Name.ToString()
                         })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.RequestRequestNoFilter), e => e.RequestRequestNo.ToLower() == input.RequestRequestNoFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserName.ToLower() == input.UserNameFilter.ToLower().Trim());


            var requestApprovalListDtos = await query.ToListAsync();

            return _requestApprovalsExcelExporter.ExportToFile(requestApprovalListDtos);
         }


         public async Task<PagedResultDto<RequestForApprovalLookupTableDto>> GetAllRequestForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _requestRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.RequestNo.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var requestList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<RequestForApprovalLookupTableDto>();
			foreach(var request in requestList){
				lookupTableDtoList.Add(new RequestForApprovalLookupTableDto
				{
					Id = request.Id,
					DisplayName = request.RequestNo.ToString()
				});
			}

            return new PagedResultDto<RequestForApprovalLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }		

         public async Task<PagedResultDto<UserForApprovalLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UserForApprovalLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new UserForApprovalLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name.ToString()
				});
			}

            return new PagedResultDto<UserForApprovalLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}