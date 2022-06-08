using Test.Requests;
using Kad.PMSDemo.Authorization.Users;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Test.TechTeams.Exporting;
using Test.TechTeams.Dtos;
using Kad.PMSDemo.Dto;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Abp.Net.Mail;
using Kad.PMSDemo;
using Kad.PMSDemo.Net.Emailing;

namespace Test.TechTeams
{
	[AbpAuthorize]
    public class TechTeamsAppService : PMSDemoAppServiceBase, ITechTeamsAppService
    {
		 private readonly IRepository<TechTeam> _techTeamRepository;
		 private readonly ITechTeamsExcelExporter _techTeamsExcelExporter;
		 private readonly IRepository<Request,int> _requestRepository;
		 private readonly IRepository<User,long> _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateProvider _emailTemplateProvider;


        public TechTeamsAppService(IRepository<TechTeam> techTeamRepository, ITechTeamsExcelExporter techTeamsExcelExporter , IRepository<Request, int> requestRepository, IRepository<User, long> userRepository, IEmailSender emailSender, IEmailTemplateProvider emailTemplateProvider) 
		  {
			_techTeamRepository = techTeamRepository;
			_techTeamsExcelExporter = techTeamsExcelExporter;
			_requestRepository = requestRepository;
		    _userRepository = userRepository;
            _emailSender = emailSender;
            _emailTemplateProvider = emailTemplateProvider;
        }

		 public async Task<PagedResultDto<GetTechTeamForView>> GetAll(GetAllTechTeamsInput input)
         {
			var roleFilter = (StaffCategory) input.RoleFilter;


            var filtered = _techTeamRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinTimeChargeFilter != null, e => e.TimeCharge >= input.MinTimeChargeFilter)
                        .WhereIf(input.MaxTimeChargeFilter != null, e => e.TimeCharge <= input.MaxTimeChargeFilter)
                        .WhereIf(input.RoleFilter > -1, e => e.Role == roleFilter);

            var pagedAndFiltered = filtered
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var query = (from o in pagedAndFiltered
                         join o1 in _requestRepository.GetAll() on o.RequestId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _userRepository.GetAll() on o.CMACSUserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetTechTeamForView()
                         {
                             TechTeam = ObjectMapper.Map<TechTeamDto>(o),
                             RequestLocalChargeCode = s1 == null ? "" : s1.LocalChargeCode.ToString(),
                             UserName = s2 == null ? "" : s2.Name.ToString()

                         })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.RequestLocalChargeCodeFilter), e => e.RequestLocalChargeCode.ToLower() == input.RequestLocalChargeCodeFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserName.ToLower() == input.UserNameFilter.ToLower().Trim());

            var totalCount = await filtered.CountAsync();

            return new PagedResultDto<GetTechTeamForView>(
                totalCount,
                await query.ToListAsync()
            );
         }
		 
		 public async Task<GetTechTeamForEditOutput> GetTechTeamForEdit(EntityDto input)
         {
            var techTeam = await _techTeamRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTechTeamForEditOutput {TechTeam = ObjectMapper.Map<CreateOrEditTechTeamDto>(techTeam)};

		    if (output.TechTeam.RequestId != null)
            {
                var request = await _requestRepository.FirstOrDefaultAsync((int)output.TechTeam.RequestId);
                output.RequestLocalChargeCode = request.LocalChargeCode.ToString();
            }

		    if (output.TechTeam.CMACSUserId != null)
            {
                var user = await _userRepository.FirstOrDefaultAsync((long)output.TechTeam.CMACSUserId);
                output.UserName = user.Name.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTechTeamDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

        public async Task CreateOrEditTechTeam(List<CreateOrEditTechTeamDto> input)
        {
            int? requestid = 0;
            int counter = 0;
            foreach (var _input in input)
            {
                if (counter == 0)
                {
                    await _techTeamRepository.DeleteAsync(e => e.RequestId == _input.RequestId);
                    counter++;
                }
                

                await CreateOrEdit(_input);
                requestid = _input.RequestId;

                string _toEmail = _userRepository.Get((long)_input.CMACSUserId).EmailAddress;
                string _userName = _userRepository.Get((long)_input.CMACSUserId).FullName;
                string _emailTitle = L("RequestAssigned_Title");
                string _emailSubTitle = L("RequestAssigned_SubTitle");
                string _emailSubject = L("RequestAssigned_Email_Subject");

                var mailMessage = new StringBuilder();
                mailMessage.AppendLine("<br>" + L("RequestAssigned_Email_Body") + "<br />");

                try
                {
                    await SwapTemplateAndSendEmail(_toEmail, _emailTitle, _emailSubTitle, _emailSubject, mailMessage, _userName);
                }
                catch
                {

                }
            }

            //await _requestRepository.UpdateRequestStatus(requestid, RequestStatus.Assigned);

            var request = await _requestRepository.FirstOrDefaultAsync((int)requestid);
            request.RequestStatusId = request.RequestStatusId == RequestStatus.Requested ? (request.RequestTypeId == RequestType.Enquiry ? RequestStatus.WIP : RequestStatus.Assigned) : request.RequestStatusId;
            //request.CompletionDate = DateTime.Now;
            await _requestRepository.UpdateAsync(request);

        }


		 private async Task Create(CreateOrEditTechTeamDto input)
         {
            var techTeam = ObjectMapper.Map<TechTeam>(input);

			
			if (AbpSession.TenantId != null)
			{
				techTeam.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _techTeamRepository.InsertAsync(techTeam);
         }

		 private async Task Update(CreateOrEditTechTeamDto input)
         {
            var techTeam = await _techTeamRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, techTeam);
         }


         public async Task Delete(EntityDto input)
         {
            await _techTeamRepository.DeleteAsync(input.Id);
         }

		 public async Task<FileDto> GetTechTeamsToExcel(GetAllTechTeamsForExcelInput input)
         {
			var roleFilter = (StaffCategory) input.RoleFilter;

            var filtered = _techTeamRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinTimeChargeFilter != null, e => e.TimeCharge >= input.MinTimeChargeFilter)
                        .WhereIf(input.MaxTimeChargeFilter != null, e => e.TimeCharge <= input.MaxTimeChargeFilter)
                        .WhereIf(input.RoleFilter > -1, e => e.Role == roleFilter);

            var query = (from o in filtered
                         join o1 in _requestRepository.GetAll() on o.RequestId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _userRepository.GetAll() on o.CMACSUserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetTechTeamForView()
                         {
                             TechTeam = ObjectMapper.Map<TechTeamDto>(o),
                             RequestLocalChargeCode = s1 == null ? "" : s1.LocalChargeCode.ToString(),
                             UserName = s2 == null ? "" : s2.Name.ToString()

                         })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.RequestLocalChargeCodeFilter), e => e.RequestLocalChargeCode.ToLower() == input.RequestLocalChargeCodeFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserName.ToLower() == input.UserNameFilter.ToLower().Trim());


            var techTeamListDtos = await query.ToListAsync();

            return _techTeamsExcelExporter.ExportToFile(techTeamListDtos);
         }


         public async Task<PagedResultDto<RequestLookupTableDto>> GetAllRequestForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _requestRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.LocalChargeCode.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var requestList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<RequestLookupTableDto>();
			foreach(var request in requestList){
				lookupTableDtoList.Add(new RequestLookupTableDto
				{
					Id = request.Id,
					DisplayName = request.LocalChargeCode.ToString()
				});
			}

            return new PagedResultDto<RequestLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }


         public async Task<PagedResultDto<UserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name.ToString().Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new UserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name.ToString()
				});
			}

            return new PagedResultDto<UserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

        private async Task SwapTemplateAndSendEmail(string toEmail, string emailTitle, string emailSubTitle, string subject, StringBuilder mailMessage, string userName)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate(AbpSession.TenantId));
            emailTemplate.Replace("{EMAIL_TITLE}", emailTitle);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", emailSubTitle + " <br/>Dear " + userName + ",");
            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

            await _emailSender.SendAsync(toEmail, subject, emailTemplate.ToString());

        }
    }
}