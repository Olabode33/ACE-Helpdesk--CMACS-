using Test.Requests;
using Kad.PMSDemo.Authorization.Users;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Test.RequestThreads.Exporting;
using Test.RequestThreads.Dtos;
using Kad.PMSDemo.Dto;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Kad.PMSDemo;

namespace Test.RequestThreads
{
	[AbpAuthorize]
    public class RequestThreadsAppService : PMSDemoAppServiceBase, IRequestThreadsAppService
    {
		 private readonly IRepository<RequestThread> _requestThreadRepository;
		 private readonly IRequestThreadsExcelExporter _requestThreadsExcelExporter;
		 private readonly IRepository<Request,int> _requestRepository;
		 private readonly IRepository<User,long> _userRepository;
		 

		  public RequestThreadsAppService(IRepository<RequestThread> requestThreadRepository, IRequestThreadsExcelExporter requestThreadsExcelExporter , IRepository<Request, int> requestRepository, IRepository<User, long> userRepository) 
		  {
			_requestThreadRepository = requestThreadRepository;
			_requestThreadsExcelExporter = requestThreadsExcelExporter;
			_requestRepository = requestRepository;
		_userRepository = userRepository;
		
		  }

		 public async Task<PagedResultDto<GetRequestThreadForView>> GetAll(GetAllRequestThreadsInput input)
         {

            var filtered = _requestThreadRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Comment.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CommentFilter), e => e.Comment.ToLower() == input.CommentFilter.ToLower().Trim())
                        .WhereIf(input.MinCommentDateFilter != null, e => e.CommentDate >= input.MinCommentDateFilter)
                        .WhereIf(input.MaxCommentDateFilter != null, e => e.CommentDate <= input.MaxCommentDateFilter);

            var pagedAndFiltered = filtered
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var query = (from o in pagedAndFiltered
                         join o1 in _requestRepository.GetAll() on o.RequestId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _userRepository.GetAll() on o.CommentById equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetRequestThreadForView()
                         {
                             RequestThread = ObjectMapper.Map<RequestThreadDto>(o),
                             RequestLocalChargeCode = s1 == null ? "" : s1.LocalChargeCode.ToString(),
                             UserName = s2 == null ? "" : s2.Name.ToString()

                         })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.RequestLocalChargeCodeFilter), e => e.RequestLocalChargeCode.ToLower() == input.RequestLocalChargeCodeFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserName.ToLower() == input.UserNameFilter.ToLower().Trim());

            var totalCount = await filtered.CountAsync();

            return new PagedResultDto<GetRequestThreadForView>(
                totalCount,
                await query.ToListAsync()
            );

         }
		 

		 public async Task<GetRequestThreadForEditOutput> GetRequestThreadForEdit(EntityDto input)
         {
            var requestThread = await _requestThreadRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetRequestThreadForEditOutput {RequestThread = ObjectMapper.Map<CreateOrEditRequestThreadDto>(requestThread)};

		    if (output.RequestThread.RequestId != null)
            {
                var request = await _requestRepository.FirstOrDefaultAsync((int)output.RequestThread.RequestId);
                output.RequestLocalChargeCode = request.LocalChargeCode.ToString();
            }

		    if (output.RequestThread.CommentById != null)
            {
                var user = await _userRepository.FirstOrDefaultAsync((long)output.RequestThread.CommentById);
                output.UserName = user.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditRequestThreadDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }


		 private async Task Create(CreateOrEditRequestThreadDto input)
         {
            var requestThread = ObjectMapper.Map<RequestThread>(input);

			
			if (AbpSession.TenantId != null)
			{
				requestThread.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _requestThreadRepository.InsertAsync(requestThread);
         }

		 private async Task Update(CreateOrEditRequestThreadDto input)
         {
            var requestThread = await _requestThreadRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, requestThread);
         }


         public async Task Delete(EntityDto input)
         {
            await _requestThreadRepository.DeleteAsync(input.Id);
         }

		 public async Task<FileDto> GetRequestThreadsToExcel(GetAllRequestThreadsForExcelInput input)
         {

            var filtered = _requestThreadRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Comment.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CommentFilter), e => e.Comment.ToLower() == input.CommentFilter.ToLower().Trim())
                        .WhereIf(input.MinCommentDateFilter != null, e => e.CommentDate >= input.MinCommentDateFilter)
                        .WhereIf(input.MaxCommentDateFilter != null, e => e.CommentDate <= input.MaxCommentDateFilter);

            var query = (from o in filtered
                         join o1 in _requestRepository.GetAll() on o.RequestId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _userRepository.GetAll() on o.CommentById equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetRequestThreadForView()
                         {
                             RequestThread = ObjectMapper.Map<RequestThreadDto>(o),
                             RequestLocalChargeCode = s1 == null ? "" : s1.LocalChargeCode.ToString(),
                             UserName = s2 == null ? "" : s2.Name.ToString()

                         })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.RequestLocalChargeCodeFilter), e => e.RequestLocalChargeCode.ToLower() == input.RequestLocalChargeCodeFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserName.ToLower() == input.UserNameFilter.ToLower().Trim());


            var requestThreadListDtos = await query.ToListAsync();

            return _requestThreadsExcelExporter.ExportToFile(requestThreadListDtos);
         }


         public async Task<PagedResultDto<RequestLookupTableDto_4Threads>> GetAllRequestForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _requestRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.LocalChargeCode.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var requestList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<RequestLookupTableDto_4Threads>();
			foreach(var request in requestList){
				lookupTableDtoList.Add(new RequestLookupTableDto_4Threads
				{
					Id = request.Id,
					DisplayName = request.LocalChargeCode.ToString()
				});
			}

            return new PagedResultDto<RequestLookupTableDto_4Threads>(
                totalCount,
                lookupTableDtoList
            );
         }		 

         public async Task<PagedResultDto<UserLookupTableDto_4Threads>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UserLookupTableDto_4Threads>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new UserLookupTableDto_4Threads
				{
					Id = user.Id,
					DisplayName = user.Name.ToString()
				});
			}

            return new PagedResultDto<UserLookupTableDto_4Threads>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}