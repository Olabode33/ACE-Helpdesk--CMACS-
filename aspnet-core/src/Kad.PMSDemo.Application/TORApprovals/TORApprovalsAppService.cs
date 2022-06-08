using Kad.PMSDemo.Authorization.Users;
using Test.Requests;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Test.TORApprovals.Exporting;
using Test.TORApprovals.Dtos;
using Kad.PMSDemo.Dto;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Kad.PMSDemo;

namespace Test.TORApprovals
{
    [AbpAuthorize]
    public class TORApprovalsAppService : PMSDemoAppServiceBase, ITORApprovalsAppService
    {
		 private readonly IRepository<TORApproval> _torApprovalRepository;
		 private readonly ITORApprovalsExcelExporter _torApprovalsExcelExporter;
		 private readonly IRepository<User,long> _userRepository;
		 private readonly IRepository<Request,int> _requestRepository;
		 

		  public TORApprovalsAppService(IRepository<TORApproval> torApprovalRepository, ITORApprovalsExcelExporter torApprovalsExcelExporter , IRepository<User, long> userRepository, IRepository<Request, int> requestRepository) 
		  {
			_torApprovalRepository = torApprovalRepository;
			_torApprovalsExcelExporter = torApprovalsExcelExporter;
			_userRepository = userRepository;
		_requestRepository = requestRepository;
		
		  }

		 public async Task<PagedResultDto<GetTORApprovalForView>> GetAll(GetAllTORApprovalsInput input)
         {


            var filtered = _torApprovalRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinTORTimeSentFilter != null, e => e.TORTimeSent >= input.MinTORTimeSentFilter)
                        .WhereIf(input.MaxTORTimeSentFilter != null, e => e.TORTimeSent <= input.MaxTORTimeSentFilter)
                        .WhereIf(input.ApprovedFilter > -1, e => Convert.ToInt32(e.Approved) == input.ApprovedFilter)
                        .WhereIf(input.MinApprovedTimeFilter != null, e => e.ApprovedTime >= input.MinApprovedTimeFilter)
                        .WhereIf(input.MaxApprovedTimeFilter != null, e => e.ApprovedTime <= input.MaxApprovedTimeFilter);

            var pagedAndFiltered = filtered
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var query = (from o in pagedAndFiltered
                         join o1 in _userRepository.GetAll() on o.ApproverId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _requestRepository.GetAll() on o.RequestId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetTORApprovalForView()
                         {
                             TORApproval = ObjectMapper.Map<TORApprovalDto>(o)
                         ,
                             UserName = s1 == null ? "" : s1.Name.ToString(),
                             RequestRequestNo = s2 == null ? "" : s2.RequestNo.ToString()
                         })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserName.ToLower() == input.UserNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RequestRequestNoFilter), e => e.RequestRequestNo.ToLower() == input.RequestRequestNoFilter.ToLower().Trim());

            var totalCount = await filtered.CountAsync();

            return new PagedResultDto<GetTORApprovalForView>(
                totalCount,
                await query.ToListAsync()
            );
         }
		 
		 public async Task<GetTORApprovalForEditOutput> GetTORApprovalForEdit(EntityDto input)
         {
            var torApproval = await _torApprovalRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTORApprovalForEditOutput {TORApproval = ObjectMapper.Map<CreateOrEditTORApprovalDto>(torApproval)};

		    if (output.TORApproval.ApproverId != null)
            {
                var user = await _userRepository.FirstOrDefaultAsync((long)output.TORApproval.ApproverId);
                output.UserName = user.Name.ToString();
            }

		    if (output.TORApproval.RequestId != null)
            {
                var request = await _requestRepository.FirstOrDefaultAsync((int)output.TORApproval.RequestId);
                output.RequestRequestNo = request.RequestNo.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTORApprovalDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 private async Task Create(CreateOrEditTORApprovalDto input)
         {
            var torApproval = ObjectMapper.Map<TORApproval>(input);

			
			if (AbpSession.TenantId != null)
			{
				torApproval.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _torApprovalRepository.InsertAsync(torApproval);
         }

		 private async Task Update(CreateOrEditTORApprovalDto input)
         {
            var torApproval = await _torApprovalRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, torApproval);
         }

         public async Task Delete(EntityDto input)
         {
            await _torApprovalRepository.DeleteAsync(input.Id);
         }

		 public async Task<FileDto> GetTORApprovalsToExcel(GetAllTORApprovalsForExcelInput input)
         {
            var filtered = _torApprovalRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinTORTimeSentFilter != null, e => e.TORTimeSent >= input.MinTORTimeSentFilter)
                        .WhereIf(input.MaxTORTimeSentFilter != null, e => e.TORTimeSent <= input.MaxTORTimeSentFilter)
                        .WhereIf(input.ApprovedFilter > -1, e => Convert.ToInt32(e.Approved) == input.ApprovedFilter)
                        .WhereIf(input.MinApprovedTimeFilter != null, e => e.ApprovedTime >= input.MinApprovedTimeFilter)
                        .WhereIf(input.MaxApprovedTimeFilter != null, e => e.ApprovedTime <= input.MaxApprovedTimeFilter);

            var query = (from o in filtered
                         join o1 in _userRepository.GetAll() on o.ApproverId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _requestRepository.GetAll() on o.RequestId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetTORApprovalForView()
                         {
                             TORApproval = ObjectMapper.Map<TORApprovalDto>(o)
                         ,
                             UserName = s1 == null ? "" : s1.Name.ToString(),
                             RequestRequestNo = s2 == null ? "" : s2.RequestNo.ToString()
                         })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserName.ToLower() == input.UserNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RequestRequestNoFilter), e => e.RequestRequestNo.ToLower() == input.RequestRequestNoFilter.ToLower().Trim());


            var torApprovalListDtos = await query.ToListAsync();

            return _torApprovalsExcelExporter.ExportToFile(torApprovalListDtos);
         }

         public async Task<PagedResultDto<UserLookupTORTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UserLookupTORTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new UserLookupTORTableDto
				{
					Id = user.Id,
					DisplayName = user.Name.ToString()
				});
			}

            return new PagedResultDto<UserLookupTORTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

         public async Task<PagedResultDto<RequestLookupTORTableDto>> GetAllRequestForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _requestRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.RequestNo.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var requestList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<RequestLookupTORTableDto>();
			foreach(var request in requestList){
				lookupTableDtoList.Add(new RequestLookupTORTableDto
				{
					Id = request.Id,
					DisplayName = request.RequestNo.ToString()
				});
			}

            return new PagedResultDto<RequestLookupTORTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}