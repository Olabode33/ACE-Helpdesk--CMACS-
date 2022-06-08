using Test.RequestAreas;
using Test.RequestDomains;
using Kad.PMSDemo.Authorization.Users;
using Test.ClientLists;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Test.Requests.Exporting;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Test.TechTeams;
using Abp.Net.Mail;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Test.TORApprovals;
using Test.RequestApprovals;
using Test.Requests.Dtos;
using Kad.PMSDemo.Dto;
using Abp.UI;
using Abp.Collections.Extensions;
using Test.AttachedDocs;
using Abp.Authorization.Users;
using Abp.Authorization.Roles;
using Microsoft.AspNetCore.Identity;
using Kad.PMSDemo.Net.Emailing;
using Kad.PMSDemo.Authorization.Roles;
using Kad.PMSDemo;

namespace Test.Requests
{
    [AbpAuthorize(AppPermissions.Pages_Requests)]
    public class RequestsAppService : PMSDemoAppServiceBase, IRequestsAppService
    {
        private readonly IRepository<Request> _requestRepository;
        private readonly IRequestsExcelExporter _requestsExcelExporter;
        private readonly IRepository<RequestArea, int> _requestAreaRepository;
        private readonly IRepository<RequestDomain, int> _requestDomainRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IRepository<ClientList, int> _clientListRepository;
        private readonly IRepository<TechTeam> _techTeamRepository;
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IRepository<TORApproval> _torApprovalRepository;
        private readonly IRepository<TORApproval> _torApprovalTmpRepository;
        private readonly IRepository<RequestApproval> _requestApprovalRepository;
        private readonly IRepository<AttachedDoc> _attachedDocRepository;
        private readonly IRepository<RequestCmacsManagerApproval> _cmacsManagerApprovalsRepository;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IAttachedDocsAppService _attachedDocsAppService;
        private readonly IRequestSubAreaMappingsAppService _requestSubAreaMappingsAppService;
        private readonly IRepository<RequestSubAreaMapping> _requestSubAreaMappingRepository;

        public RequestsAppService(IRepository<Request> requestRepository, IRequestsExcelExporter requestsExcelExporter, IRepository<RequestArea, int> requestAreaRepository,
                            IRepository<RequestDomain, int> requestDomainRepository, IRepository<User, long> userRepository, IRepository<UserRole, long> userRoleRepository,
                            IRepository<ClientList, int> clientListRepository, IRepository<TechTeam> techTeamRepository,
                            IEmailSender emailSender, IEmailTemplateProvider emailTemplateProvider, IWebHostEnvironment hostingEnvironment,
                            IRepository<TORApproval> torApprovalRepository, IRepository<RequestApproval> requestApprovalRepository, IRepository<AttachedDoc> attachedDocRepository,
                            UserManager userManager, RoleManager roleManager, IAttachedDocsAppService attachedDocsAppService, IRepository<RequestCmacsManagerApproval> cmacsManagerApprovalRepository,
                            IRequestSubAreaMappingsAppService requestSubAreaMappingsAppService, IRepository<RequestSubAreaMapping> requestSubAreaMappingRepository)
        {
            _requestRepository = requestRepository;
            _requestsExcelExporter = requestsExcelExporter;
            _requestAreaRepository = requestAreaRepository;
            _requestDomainRepository = requestDomainRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _clientListRepository = clientListRepository;
            _techTeamRepository = techTeamRepository;
            _emailSender = emailSender;
            _emailTemplateProvider = emailTemplateProvider;
            _hostingEnvironment = hostingEnvironment;
            _torApprovalRepository = torApprovalRepository;
            _torApprovalTmpRepository = torApprovalRepository;
            _requestApprovalRepository = requestApprovalRepository;
            _attachedDocRepository = attachedDocRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _attachedDocsAppService = attachedDocsAppService;
            _cmacsManagerApprovalsRepository = cmacsManagerApprovalRepository;
            _requestSubAreaMappingsAppService = requestSubAreaMappingsAppService;
            _requestSubAreaMappingRepository = requestSubAreaMappingRepository;
        }

        public async Task<PagedResultDto<GetRequestForView>> GetAll(GetAllRequestsInput input)
        {
            string filterText = input.Filter?.ToLower().ToString();
            var requestStatusIdFilter = (RequestStatus)input.RequestStatusIdFilter;

            var filteredRequests = _requestRepository.GetAll();
            //var torApprovals = _torApprovalRepository.GetAll().ToList();
            //var requestApprovals = _requestApprovalRepository.GetAll().ToList();

            if (await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.RequestingTeam)) //  PermissionChecker.IsGranted(AppPermissions.Pages_Requests_RequestingTeam))
            {
                //Filter to just request that has been assigned to user as the manager or Partner (for Requesting Team)
                var filteredRequests_1 = _requestRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocalSubCode.Contains(input.Filter) || e.LocalChargeCode.Contains(input.Filter) || e.ReasonResponseDate.Contains(input.Filter) || e.IssueDiscussedWith.Contains(input.Filter) || e.OOTReviewer.Contains(input.Filter) || e.ConsultationIssue.Contains(input.Filter) || e.Background.Contains(input.Filter) || e.TechReference.Contains(input.Filter) || e.AgreedGuidance.Contains(input.Filter) || e.TechGrpResponse.Contains(input.Filter))
                        .WhereIf(input.RequestStatusIdFilter > -1, e => e.RequestStatusId == requestStatusIdFilter)
                        .Where(e => e.RequestorId == AbpSession.UserId || e.RequestorPartnerId == AbpSession.UserId || e.RequestorManagerId == AbpSession.UserId);

                filteredRequests = filteredRequests_1;
            }
            else if (await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.TechnicalTeam) &&
                !(await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.Admin) || await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.IFRS_Helpdesk) ||
                await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.CMACsManager) || await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.FinalApprover)))
            // (PermissionChecker.IsGranted(AppPermissions.Pages_Requests_TechnicalTeam) &&
            // (PermissionChecker.IsGranted(AppPermissions.Pages_Requests_CmacsManager) || PermissionChecker.IsGranted(AppPermissions.Pages_Requests_MarkAsCompleted)))
            {
                //Filter to just request that has been assigned to tech team members
                var techTeamMembers = _techTeamRepository.GetAll().Where(x => x.CMACSUserId == AbpSession.UserId); //.ToListAsync();

                var filteredRequests_All = _requestRepository.GetAll()
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocalSubCode.Contains(input.Filter) || e.LocalChargeCode.Contains(input.Filter) || e.ReasonResponseDate.Contains(input.Filter) || e.IssueDiscussedWith.Contains(input.Filter) || e.OOTReviewer.Contains(input.Filter) || e.ConsultationIssue.Contains(input.Filter) || e.Background.Contains(input.Filter) || e.TechReference.Contains(input.Filter) || e.AgreedGuidance.Contains(input.Filter) || e.TechGrpResponse.Contains(input.Filter))
                            .WhereIf(input.RequestStatusIdFilter > -1, e => e.RequestStatusId == requestStatusIdFilter)
                            .Where(e => techTeamMembers.Any(x => x.RequestId == e.Id));

                filteredRequests = filteredRequests_All;
            }
            else
            {
                //Return all request for all others (Helpdesk, CMACS Manager, CMACS Partner)
                var filteredRequests_All = _requestRepository.GetAll()
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocalSubCode.Contains(input.Filter) || e.LocalChargeCode.Contains(input.Filter) || e.ReasonResponseDate.Contains(input.Filter) || e.IssueDiscussedWith.Contains(input.Filter) || e.OOTReviewer.Contains(input.Filter) || e.ConsultationIssue.Contains(input.Filter) || e.Background.Contains(input.Filter) || e.TechReference.Contains(input.Filter) || e.AgreedGuidance.Contains(input.Filter) || e.TechGrpResponse.Contains(input.Filter))
                            .WhereIf(input.RequestStatusIdFilter > -1, e => e.RequestStatusId == requestStatusIdFilter);

                filteredRequests = filteredRequests_All;
            }

            var pagedAndFiltered = filteredRequests
                .OrderBy(input.Sorting ?? "creationTime desc")
                .PageBy(input);

            var query = (from o in pagedAndFiltered
                         join o1 in _requestAreaRepository.GetAll() on o.RequestAreaId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _requestDomainRepository.GetAll() on o.RequestDomainId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         join o3 in _userRepository.GetAll() on o.RequestorId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         join o4 in _userRepository.GetAll() on o.RequestorPartnerId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()
                         join o5 in _userRepository.GetAll() on o.RequestorManagerId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()
                         join o6 in _clientListRepository.GetAll() on o.ClientListId equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()
                         join o7 in _userRepository.GetAll() on o.AssigneeId equals o7.Id into j7
                         from s7 in j7.DefaultIfEmpty()

                         select new GetRequestForView()
                         {
                             Request = ObjectMapper.Map<RequestDto>(o),
                             RequestAreaRequestAreaName = s1 == null ? "" : s1.RequestAreaName.ToString(),
                             RequestDomainDomainName = s2 == null ? "" : s2.DomainName.ToString(),
                             RequestorName = s3 == null ? "" : s3.Name.ToString() + " " + s3.Surname.ToString(),
                             PartnerName = s4 == null ? "" : s4.Name.ToString() + " " + s4.Surname.ToString(),
                             ManagerName = s5 == null ? "" : s5.Name.ToString() + " " + s5.Surname.ToString(),
                             ClientListClientName = s6 == null ? "" : s6.ClientName.ToString(),
                             AssigneeName = s7 == null ? "" : s7.Name.ToString() + " " + s7.Surname.ToString(),
                             //NextAction = GetRequestNextAction(o, torApprovals, requestApprovals),
                             //PercentageComplete = GetRequestPercentageCompletion(o, torApprovals, requestApprovals)
                         });

            var totalCount = await query.CountAsync();
            var requests = await query.ToListAsync();

            return new PagedResultDto<GetRequestForView>(
                totalCount,
                requests
            );
        }

        public async Task<PagedResultDto<GetRequestForView>> GetAllForWorkspaceNew(GetAllRequestsInput input)
        {
            string filterText = input.Filter?.ToLower().ToString();
            var requestStatusIdFilter = (RequestStatus)input.RequestStatusIdFilter;

            var filteredRequests = _requestRepository.GetAll();
            var torApprovals = _torApprovalRepository.GetAll().ToList();
            var requestApprovals = _requestApprovalRepository.GetAll().ToList();

            if (await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.RequestingTeam)) //  PermissionChecker.IsGranted(AppPermissions.Pages_Requests_RequestingTeam))
            {
                //Filter to just request that has been assigned to user as the manager or Partner (for Requesting Team)
                var filteredRequests_1 = _requestRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocalSubCode.Contains(input.Filter) || e.LocalChargeCode.Contains(input.Filter) || e.ReasonResponseDate.Contains(input.Filter) || e.IssueDiscussedWith.Contains(input.Filter) || e.OOTReviewer.Contains(input.Filter) || e.ConsultationIssue.Contains(input.Filter) || e.Background.Contains(input.Filter) || e.TechReference.Contains(input.Filter) || e.AgreedGuidance.Contains(input.Filter) || e.TechGrpResponse.Contains(input.Filter))
                        .WhereIf(input.RequestStatusIdFilter > -1, e => e.RequestStatusId == requestStatusIdFilter)
                        .Where(e => e.RequestorId == AbpSession.UserId || e.RequestorPartnerId == AbpSession.UserId || e.RequestorManagerId == AbpSession.UserId)
                        .Where(e => e.RequestStatusId != RequestStatus.Completed); ;

                filteredRequests = filteredRequests_1;
            }
            else if (await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.TechnicalTeam) &&
                !(await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.Admin) || await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.IFRS_Helpdesk) ||
                await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.CMACsManager) || await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.FinalApprover)))
                    // (PermissionChecker.IsGranted(AppPermissions.Pages_Requests_TechnicalTeam) &&
                    // (PermissionChecker.IsGranted(AppPermissions.Pages_Requests_CmacsManager) || PermissionChecker.IsGranted(AppPermissions.Pages_Requests_MarkAsCompleted)))
            {
                //Filter to just request that has been assigned to tech team members
                var techTeamMembers = _techTeamRepository.GetAll().Where(x => x.CMACSUserId == AbpSession.UserId); //.ToListAsync();

                var filteredRequests_All = _requestRepository.GetAll()
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocalSubCode.Contains(input.Filter) || e.LocalChargeCode.Contains(input.Filter) || e.ReasonResponseDate.Contains(input.Filter) || e.IssueDiscussedWith.Contains(input.Filter) || e.OOTReviewer.Contains(input.Filter) || e.ConsultationIssue.Contains(input.Filter) || e.Background.Contains(input.Filter) || e.TechReference.Contains(input.Filter) || e.AgreedGuidance.Contains(input.Filter) || e.TechGrpResponse.Contains(input.Filter))
                            .WhereIf(input.RequestStatusIdFilter > -1, e => e.RequestStatusId == requestStatusIdFilter)
                            .Where(e => techTeamMembers.Any(x => x.RequestId == e.Id))
                            .Where(e => e.RequestStatusId != RequestStatus.Completed);

                filteredRequests = filteredRequests_All;
            }
            else
            {
                //Return all request for all others (Helpdesk, CMACS Manager, CMACS Partner)
                var filteredRequests_All = _requestRepository.GetAll()
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocalSubCode.Contains(input.Filter) || e.LocalChargeCode.Contains(input.Filter) || e.ReasonResponseDate.Contains(input.Filter) || e.IssueDiscussedWith.Contains(input.Filter) || e.OOTReviewer.Contains(input.Filter) || e.ConsultationIssue.Contains(input.Filter) || e.Background.Contains(input.Filter) || e.TechReference.Contains(input.Filter) || e.AgreedGuidance.Contains(input.Filter) || e.TechGrpResponse.Contains(input.Filter))
                            .WhereIf(input.RequestStatusIdFilter > -1, e => e.RequestStatusId == requestStatusIdFilter)
                            .Where(e => e.RequestStatusId != RequestStatus.Completed); ;

                filteredRequests = filteredRequests_All;
            }

            var pagedAndFiltered = filteredRequests
                .OrderBy(input.Sorting ?? "creationTime desc")
                .PageBy(input);

            var query = (from o in pagedAndFiltered
                         join o1 in _requestAreaRepository.GetAll() on o.RequestAreaId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _requestDomainRepository.GetAll() on o.RequestDomainId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         join o3 in _userRepository.GetAll() on o.RequestorId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         join o4 in _userRepository.GetAll() on o.RequestorPartnerId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()
                         join o5 in _userRepository.GetAll() on o.RequestorManagerId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()
                         join o6 in _clientListRepository.GetAll() on o.ClientListId equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()
                         join o7 in _userRepository.GetAll() on o.AssigneeId equals o7.Id into j7
                         from s7 in j7.DefaultIfEmpty()

                         select new GetRequestForView()
                         {
                             Request = ObjectMapper.Map<RequestDto>(o),
                             RequestAreaRequestAreaName = s1 == null ? "" : s1.RequestAreaName.ToString(),
                             RequestDomainDomainName = s2 == null ? "" : s2.DomainName.ToString(),
                             RequestorName = s3 == null ? "" : s3.Name.ToString() + " " + s3.Surname.ToString(),
                             PartnerName = s4 == null ? "" : s4.Name.ToString() + " " + s4.Surname.ToString(),
                             ManagerName = s5 == null ? "" : s5.Name.ToString() + " " + s5.Surname.ToString(),
                             ClientListClientName = s6 == null ? "" : s6.ClientName.ToString(),
                             AssigneeName = s7 == null ? "" : s7.Name.ToString() + " " + s7.Surname.ToString(),
                             //NextAction = GetRequestNextAction(o, torApprovals, requestApprovals),
                             //PercentageComplete = GetRequestPercentageCompletion(o, torApprovals, requestApprovals)
                         });

            var totalCount = await query.CountAsync();
            var requests = await query.ToListAsync();

            foreach(var request in requests)
            {
                request.NextAction = GetRequestNextAction(request.Request, torApprovals, requestApprovals);
                request.PercentageComplete = GetRequestPercentageCompletion(request.Request, torApprovals, requestApprovals);
            }

            //bool check = requests[0].RequestorName.ToLower().Contains("ad");

            return new PagedResultDto<GetRequestForView>(
                totalCount,
                requests
            );
        }

        public async Task<PagedResultDto<GetRequestForView>> GetAllForWorkspace(GetAllRequestsInput input)
        {
            string filterText = input.Filter?.ToLower().ToString();
            var requestStatusIdFilter = (RequestStatus)input.RequestStatusIdFilter;

            var filteredRequests = _requestRepository.GetAll();
            var torApprovals = _torApprovalRepository.GetAll().ToList();
            var requestApprovals = _requestApprovalRepository.GetAll().ToList();

            if (PermissionChecker.IsGranted(AppPermissions.Pages_Requests_AssignRequest))
            {
                var filteredRequests_All = _requestRepository.GetAll()
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocalSubCode.Contains(input.Filter) || e.LocalChargeCode.Contains(input.Filter) || e.ReasonResponseDate.Contains(input.Filter) || e.IssueDiscussedWith.Contains(input.Filter) || e.OOTReviewer.Contains(input.Filter) || e.ConsultationIssue.Contains(input.Filter) || e.Background.Contains(input.Filter) || e.TechReference.Contains(input.Filter) || e.AgreedGuidance.Contains(input.Filter) || e.TechGrpResponse.Contains(input.Filter))
                            .WhereIf(input.RequestStatusIdFilter > -1, e => e.RequestStatusId == requestStatusIdFilter)
                            .Where(e => e.RequestStatusId != RequestStatus.Completed);

                filteredRequests = filteredRequests_All;
            }
            else if (PermissionChecker.IsGranted(AppPermissions.Pages_Requests_CreateTOR) && PermissionChecker.IsGranted(AppPermissions.Pages_Requests_TechnicalTeam))
            {
                var techTeamMembers = await _techTeamRepository.GetAll().Where(x => x.CMACSUserId == AbpSession.UserId).ToListAsync();

                var filteredRequests_All = _requestRepository.GetAll()
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocalSubCode.Contains(input.Filter) || e.LocalChargeCode.Contains(input.Filter) || e.ReasonResponseDate.Contains(input.Filter) || e.IssueDiscussedWith.Contains(input.Filter) || e.OOTReviewer.Contains(input.Filter) || e.ConsultationIssue.Contains(input.Filter) || e.Background.Contains(input.Filter) || e.TechReference.Contains(input.Filter) || e.AgreedGuidance.Contains(input.Filter) || e.TechGrpResponse.Contains(input.Filter))
                            //.WhereIf(input.RequestStatusIdFilter > -1, e => e.RequestStatusId == requestStatusIdFilter)
                            .Where(e => techTeamMembers.Any(x => x.RequestId == e.Id));
                //.Where(e => (e.RequestStatusId == RequestStatus.Assigned || e.RequestStatusId == RequestStatus.AwaitingTOR || e.RequestStatusId == RequestStatus.WIP || e.RequestStatusId == RequestStatus.Prepared || e.RequestStatusId == RequestStatus.CMASManagerRequestReview) && techTeamMembers.Any(x => x.RequestId == e.Id));

                filteredRequests = filteredRequests_All;
            }
            else if (PermissionChecker.IsGranted(AppPermissions.Pages_Requests_CmacsManager))
            {
                var filteredRequests_All = _requestRepository.GetAll()
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocalSubCode.Contains(input.Filter) || e.LocalChargeCode.Contains(input.Filter) || e.ReasonResponseDate.Contains(input.Filter) || e.IssueDiscussedWith.Contains(input.Filter) || e.OOTReviewer.Contains(input.Filter) || e.ConsultationIssue.Contains(input.Filter) || e.Background.Contains(input.Filter) || e.TechReference.Contains(input.Filter) || e.AgreedGuidance.Contains(input.Filter) || e.TechGrpResponse.Contains(input.Filter))
                            //.WhereIf(input.RequestStatusIdFilter > -1, e => e.RequestStatusId == requestStatusIdFilter)
                            //.Where(e => e.RequestStatusId == RequestStatus.AwaitingTOR || e.RequestStatusId == RequestStatus.WIP || e.RequestStatusId == RequestStatus.Accepted || e.RequestStatusId == RequestStatus.CMASManagerRequestReview);
                            ;
                filteredRequests = filteredRequests_All;
            }
            else if (PermissionChecker.IsGranted(AppPermissions.Pages_Requests_MarkAsCompleted))
            {
                var filteredRequests_All = _requestRepository.GetAll()
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocalSubCode.Contains(input.Filter) || e.LocalChargeCode.Contains(input.Filter) || e.ReasonResponseDate.Contains(input.Filter) || e.IssueDiscussedWith.Contains(input.Filter) || e.OOTReviewer.Contains(input.Filter) || e.ConsultationIssue.Contains(input.Filter) || e.Background.Contains(input.Filter) || e.TechReference.Contains(input.Filter) || e.AgreedGuidance.Contains(input.Filter) || e.TechGrpResponse.Contains(input.Filter))
                            //.WhereIf(input.RequestStatusIdFilter > -1, e => e.RequestStatusId == requestStatusIdFilter)
                            .Where(e => e.RequestStatusId == RequestStatus.Accepted || e.RequestStatusId == RequestStatus.CMASManagerApproved);

                filteredRequests = filteredRequests_All;
            }
            else
            {
                var torNotApproved = _torApprovalRepository.GetAll().Where(e => e.ApproverId == AbpSession.UserId).Where(e => e.Approved == false);
                var requestNotApproved = _requestApprovalRepository.GetAll().Where(e => e.ApproverId == AbpSession.UserId).Where(e => e.Approved == false);

                var filteredRequests_1 = _requestRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocalSubCode.Contains(input.Filter) || e.LocalChargeCode.Contains(input.Filter) || e.ReasonResponseDate.Contains(input.Filter) || e.IssueDiscussedWith.Contains(input.Filter) || e.OOTReviewer.Contains(input.Filter) || e.ConsultationIssue.Contains(input.Filter) || e.Background.Contains(input.Filter) || e.TechReference.Contains(input.Filter) || e.AgreedGuidance.Contains(input.Filter) || e.TechGrpResponse.Contains(input.Filter))
                        .WhereIf(input.RequestStatusIdFilter > -1, e => e.RequestStatusId == requestStatusIdFilter)
                        .WhereIf(input.RequestStatusIdFilter == -1, e => e.RequestStatusId != RequestStatus.Completed)
                        .Where(e => e.RequestorId == AbpSession.UserId);

                var filteredRequests_2 = _requestRepository.GetAll()
                        .Where(e =>
                                    (e.RequestorPartnerId == AbpSession.UserId || e.RequestorManagerId == AbpSession.UserId)
                                 && (e.RequestStatusId == RequestStatus.AwaitingTOR || e.RequestStatusId == RequestStatus.Prepared)
                                 && (torNotApproved.Any(x => x.RequestId == e.Id) || requestNotApproved.Any(x => x.RequestId == e.Id))
                        );

                filteredRequests = filteredRequests_1.Union(filteredRequests_2);
            }

            var pagedAndFiltered = filteredRequests
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var query = (from o in pagedAndFiltered
                         join o1 in _requestAreaRepository.GetAll() on o.RequestAreaId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _requestDomainRepository.GetAll() on o.RequestDomainId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         join o3 in _userRepository.GetAll() on o.RequestorId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         join o4 in _userRepository.GetAll() on o.RequestorPartnerId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()
                         join o5 in _userRepository.GetAll() on o.RequestorManagerId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()
                         join o6 in _clientListRepository.GetAll() on o.ClientListId equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()
                         join o7 in _userRepository.GetAll() on o.AssigneeId equals o7.Id into j7
                         from s7 in j7.DefaultIfEmpty()

                         select new GetRequestForView()
                         {
                             Request = ObjectMapper.Map<RequestDto>(o),
                             RequestAreaRequestAreaName = s1 == null ? "" : s1.RequestAreaName.ToString(),
                             RequestDomainDomainName = s2 == null ? "" : s2.DomainName.ToString(),
                             RequestorName = s3 == null ? "" : s3.Name.ToString() + " " + s3.Surname.ToString(),
                             PartnerName = s4 == null ? "" : s4.Name.ToString() + " " + s4.Surname.ToString(),
                             ManagerName = s5 == null ? "" : s5.Name.ToString() + " " + s5.Surname.ToString(),
                             ClientListClientName = s6 == null ? "" : s6.ClientName.ToString(),
                             AssigneeName = s7 == null ? "" : s7.Name.ToString() + " " + s7.Surname.ToString(),
                             //NextAction = GetRequestNextAction(o, torApprovals, requestApprovals),
                             //PercentageComplete = GetRequestPercentageCompletion(o, torApprovals, requestApprovals)
                         });

            var totalCount = await query.CountAsync();
            var requests = await query.ToListAsync();

            foreach (var request in requests)
            {
                request.NextAction = GetRequestNextAction(request.Request, torApprovals, requestApprovals);
                request.PercentageComplete = GetRequestPercentageCompletion(request.Request, torApprovals, requestApprovals);
            }

            //bool check = requests[0].RequestorName.ToLower().Contains("ad");

            return new PagedResultDto<GetRequestForView>(
                totalCount,
                requests
            );
        }

        public async Task<PagedResultDto<GetRequestForView>> GetCompletedForWorkspace(GetAllRequestsInput input)
        {
            string filterText = input.Filter?.ToLower().ToString();

            var filteredRequests = _requestRepository.GetAll();
            var torApprovals = _torApprovalRepository.GetAll().ToList();
            var requestApprovals = _requestApprovalRepository.GetAll().ToList();

            if (await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.RequestingTeam)) //  PermissionChecker.IsGranted(AppPermissions.Pages_Requests_RequestingTeam))
            {
                //Filter to just request that has been assigned to user as the manager or Partner (for Requesting Team)
                var filteredRequests_1 = _requestRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocalSubCode.Contains(input.Filter) || e.LocalChargeCode.Contains(input.Filter) || e.ReasonResponseDate.Contains(input.Filter) || e.IssueDiscussedWith.Contains(input.Filter) || e.OOTReviewer.Contains(input.Filter) || e.ConsultationIssue.Contains(input.Filter) || e.Background.Contains(input.Filter) || e.TechReference.Contains(input.Filter) || e.AgreedGuidance.Contains(input.Filter) || e.TechGrpResponse.Contains(input.Filter))
                        //.WhereIf(input.RequestStatusIdFilter > -1, e => e.RequestStatusId == requestStatusIdFilter)
                        .Where(e => e.RequestorId == AbpSession.UserId || e.RequestorPartnerId == AbpSession.UserId || e.RequestorManagerId == AbpSession.UserId)
                        .Where(e => e.RequestStatusId == RequestStatus.Completed); ;

                filteredRequests = filteredRequests_1;
            }
            else if (await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.TechnicalTeam) &&
                !(await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.Admin) || await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.IFRS_Helpdesk) ||
                await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.CMACsManager) || await UserManager.IsInRoleAsync(GetCurrentUser(), StaticRoleNames.Tenants.FinalApprover)))
            // (PermissionChecker.IsGranted(AppPermissions.Pages_Requests_TechnicalTeam) &&
            // (PermissionChecker.IsGranted(AppPermissions.Pages_Requests_CmacsManager) || PermissionChecker.IsGranted(AppPermissions.Pages_Requests_MarkAsCompleted)))
            {
                //Filter to just request that has been assigned to tech team members
                var techTeamMembers = _techTeamRepository.GetAll().Where(x => x.CMACSUserId == AbpSession.UserId); //.ToListAsync();

                var filteredRequests_All = _requestRepository.GetAll()
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocalSubCode.Contains(input.Filter) || e.LocalChargeCode.Contains(input.Filter) || e.ReasonResponseDate.Contains(input.Filter) || e.IssueDiscussedWith.Contains(input.Filter) || e.OOTReviewer.Contains(input.Filter) || e.ConsultationIssue.Contains(input.Filter) || e.Background.Contains(input.Filter) || e.TechReference.Contains(input.Filter) || e.AgreedGuidance.Contains(input.Filter) || e.TechGrpResponse.Contains(input.Filter))
                            //.WhereIf(input.RequestStatusIdFilter > -1, e => e.RequestStatusId == requestStatusIdFilter)
                            .Where(e => techTeamMembers.Any(x => x.RequestId == e.Id))
                            .Where(e => e.RequestStatusId == RequestStatus.Completed);

                filteredRequests = filteredRequests_All;
            }
            else
            {
                //Return all request for all others (Helpdesk, CMACS Manager, CMACS Partner)
                var filteredRequests_All = _requestRepository.GetAll()
                            .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocalSubCode.Contains(input.Filter) || e.LocalChargeCode.Contains(input.Filter) || e.ReasonResponseDate.Contains(input.Filter) || e.IssueDiscussedWith.Contains(input.Filter) || e.OOTReviewer.Contains(input.Filter) || e.ConsultationIssue.Contains(input.Filter) || e.Background.Contains(input.Filter) || e.TechReference.Contains(input.Filter) || e.AgreedGuidance.Contains(input.Filter) || e.TechGrpResponse.Contains(input.Filter))
                            //.WhereIf(input.RequestStatusIdFilter > -1, e => e.RequestStatusId == requestStatusIdFilter)
                            .Where(e => e.RequestStatusId == RequestStatus.Completed); ;

                filteredRequests = filteredRequests_All;
            }

            var pagedAndFiltered = filteredRequests
                .OrderBy(input.Sorting ?? "creationTime desc")
                .PageBy(input);

            var query = (from o in pagedAndFiltered
                         join o1 in _requestAreaRepository.GetAll() on o.RequestAreaId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _requestDomainRepository.GetAll() on o.RequestDomainId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         join o3 in _userRepository.GetAll() on o.RequestorId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         join o4 in _userRepository.GetAll() on o.RequestorPartnerId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()
                         join o5 in _userRepository.GetAll() on o.RequestorManagerId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()
                         join o6 in _clientListRepository.GetAll() on o.ClientListId equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()
                         join o7 in _userRepository.GetAll() on o.AssigneeId equals o7.Id into j7
                         from s7 in j7.DefaultIfEmpty()

                         select new GetRequestForView()
                         {
                             Request = ObjectMapper.Map<RequestDto>(o),
                             RequestAreaRequestAreaName = s1 == null ? "" : s1.RequestAreaName.ToString(),
                             RequestDomainDomainName = s2 == null ? "" : s2.DomainName.ToString(),
                             RequestorName = s3 == null ? "" : s3.Name.ToString() + " " + s3.Surname.ToString(),
                             PartnerName = s4 == null ? "" : s4.Name.ToString() + " " + s4.Surname.ToString(),
                             ManagerName = s5 == null ? "" : s5.Name.ToString() + " " + s5.Surname.ToString(),
                             ClientListClientName = s6 == null ? "" : s6.ClientName.ToString(),
                             AssigneeName = s7 == null ? "" : s7.Name.ToString() + " " + s7.Surname.ToString(),
                             //NextAction = GetRequestNextAction(o, torApprovals, requestApprovals),
                             //PercentageComplete = GetRequestPercentageCompletion(o, torApprovals, requestApprovals)
                         });

            var totalCount = await query.CountAsync();

            var requests = await query.ToListAsync();

            foreach (var request in requests)
            {
                request.NextAction = GetRequestNextAction(request.Request, torApprovals, requestApprovals);
                request.PercentageComplete = GetRequestPercentageCompletion(request.Request, torApprovals, requestApprovals);
            }

            //bool check = requests[0].RequestorName.ToLower().Contains("ad");

            return new PagedResultDto<GetRequestForView>(
                totalCount,
                requests
            );
        }

        private string GetRequestNextAction(RequestDto request, List<TORApproval> torApprovals, List<RequestApproval> requestApprovals)
        {
            switch (request.RequestStatusId)
            {
                case RequestStatus.Requested:
                    return "Yet to be assigned";
                case RequestStatus.Assigned:
                    return "Request is being treated";
                case RequestStatus.AwaitingTOR:
                    return CheckTORNextApprover(request, torApprovals);
                case RequestStatus.WIP:
                    return "Request is being treated";
                case RequestStatus.CMASManagerRequestReview:
                    return "Awaiting CMACS Manager's Review";
                case RequestStatus.Prepared:
                    return CheckRequestNextApprover(request, requestApprovals);
                case RequestStatus.Accepted:
                    return "Awaiting CMACS Manager";
                case RequestStatus.CMASManagerApproved:
                    return "Waiting for final approval";
                case RequestStatus.Completed:
                    return "Completed";
                default:
                    return "...";
            }
        }

        private double GetRequestPercentageCompletion(RequestDto request, List<TORApproval> torApprovals, List<RequestApproval> requestApprovals)
        {
            var interval = 12.5;
            switch (request.RequestStatusId)
            {
                case RequestStatus.Requested:
                    return 0;
                case RequestStatus.Assigned:
                    return interval;
                case RequestStatus.AwaitingTOR:
                    string nextTorApprover = CheckTORNextApprover(request, torApprovals);

                    if (nextTorApprover.Contains("Manager"))
                    {
                        return interval * 2;
                    }
                    else
                    {
                        return interval * 3;
                    }
                case RequestStatus.WIP:
                    return interval * 4;
                case RequestStatus.Prepared:
                    string nextRequestApprover = CheckRequestNextApprover(request, requestApprovals);

                    if (nextRequestApprover.Contains("Partner"))
                    {
                        return interval * 5;
                    }
                    else
                    {
                        return interval * 6;
                    }
                case RequestStatus.Accepted:
                    return interval * 7;
                case RequestStatus.CMASManagerApproved:
                    return interval * 7.5;
                case RequestStatus.Completed:
                    return interval * 8;
                default:
                    return interval;
            }
        }

        private string CheckTORNextApprover(RequestDto request, List<TORApproval> approvals)
        {
            var torApprovals = approvals.Where(e => e.RequestId == request.Id).ToList();

            if (torApprovals != null)
            {
                bool hasManagerApproved = torApprovals.Any(x => x.ApproverId == request.RequestorManagerId && x.Approved == true);

                if (hasManagerApproved)
                {
                    return "Awaiting Partner's approval";
                }
                else
                {
                    return "Awaiting Manager's approval";
                }
            }

            return "Awaiting Manager's approval";
        }

        private string CheckRequestNextApprover(RequestDto request, List<RequestApproval> approvals)
        {
            var requestApprovals = approvals.Where(e => e.RequestId == request.Id).ToList();

            if (requestApprovals != null)
            {
                bool hasManagerApproved = requestApprovals.Any(x => x.ApproverId == request.RequestorManagerId && x.Approved == true);

                if (hasManagerApproved)
                {
                    return "Awaiting Partner's approval";
                }
                else
                {
                    return "Awaiting Manager's approval";
                }
            }

            return "Awaiting Manager's approval";
        }

        //Commented out temporarily

        //[AbpAuthorize(AppPermissions.Pages_Requests_Edit)]
        //public async Task<GetRequestForEditOutput> GetRequestForEdit(EntityDto input)
        //{
        //    var request = await _requestRepository.FirstOrDefaultAsync(input.Id);

        //    var output = new GetRequestForEditOutput { Request = ObjectMapper.Map<CreateOrEditRequestDto>(request) };

        //    if (output.Request.RequestAreaId != null)
        //    {
        //        var requestArea = await _requestAreaRepository.FirstOrDefaultAsync((int)output.Request.RequestAreaId);
        //        output.RequestAreaRequestAreaName = requestArea.RequestAreaName.ToString();
        //    }

        //    if (output.Request.RequestDomainId != null)
        //    {
        //        var requestDomain = await _requestDomainRepository.FirstOrDefaultAsync((int)output.Request.RequestDomainId);
        //        output.RequestDomainDomainName = requestDomain.DomainName.ToString();
        //    }

        //    if (output.Request.RequestorId != null)
        //    {
        //        var user = await _userRepository.FirstOrDefaultAsync((long)output.Request.RequestorId);
        //        output.UserName = user.Name.ToString();
        //    }

        //    if (output.Request.RequestorPartnerId != null)
        //    {
        //        var user = await _userRepository.FirstOrDefaultAsync((long)output.Request.RequestorPartnerId);
        //        output.UserName2 = user.Name.ToString();
        //    }

        //    if (output.Request.RequestorManagerId != null)
        //    {
        //        var user = await _userRepository.FirstOrDefaultAsync((long)output.Request.RequestorManagerId);
        //        output.UserName3 = user.Name.ToString();
        //    }

        //    if (output.Request.ClientListId != null)
        //    {
        //        var clientList = await _clientListRepository.FirstOrDefaultAsync((int)output.Request.ClientListId);
        //        output.ClientListClientName = clientList.ClientName.ToString();
        //    }

        //    if (output.Request.AssigneeId != null)
        //    {
        //        var user = await _userRepository.FirstOrDefaultAsync((long)output.Request.AssigneeId);
        //        output.UserName4 = user.Name.ToString();
        //    }

        //    return output;
        //}


        //Temporarily
        //[AbpAuthorize(AppPermissions.Pages_Requests_Edit)]
        public async Task<GetRequestForEditOutput> GetRequestForEdit(EntityDto input)
        {
            var request = await _requestRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRequestForEditOutput { Request = ObjectMapper.Map<CreateOrEditRequestDto>(request) };

            if (output.Request.TermsOfRef == null)
            {
                output.Request.TermsOfRef = L("TermOfRef");
            }

            if (output.Request.RequestAreaId != null)
            {
                var requestArea = await _requestAreaRepository.FirstOrDefaultAsync((int)output.Request.RequestAreaId);
                output.RequestAreaRequestAreaName = requestArea?.RequestAreaName?.ToString();
            }

            if (output.Request.RequestDomainId != null)
            {
                var requestDomain = await _requestDomainRepository.FirstOrDefaultAsync((int)output.Request.RequestDomainId);
                output.RequestDomainDomainName = requestDomain?.DomainName?.ToString();
            }

            if (output.Request.RequestorId != null)
            {
                var user = await _userRepository.FirstOrDefaultAsync((long)output.Request.RequestorId);
                output.UserName = user?.Name?.ToString() + ' ' + user?.Surname?.ToString();
            }

            if (output.Request.RequestorPartnerId != null)
            {
                var user = await _userRepository.FirstOrDefaultAsync((long)output.Request.RequestorPartnerId);
                output.UserName2 = user?.Name?.ToString() + ' ' + user?.Surname?.ToString();
            }

            if (output.Request.RequestorManagerId != null)
            {
                var user = await _userRepository.FirstOrDefaultAsync((long)output.Request.RequestorManagerId);
                output.UserName3 = user?.Name?.ToString() + ' ' + user?.Surname?.ToString();
            }

            if (output.Request.ClientListId != null)
            {
                var clientList = await _clientListRepository.FirstOrDefaultAsync((int)output.Request.ClientListId);
                output.ClientListClientName = clientList?.ClientName?.ToString();
            }

            if (output.Request.AssigneeId != null)
            {
                var user = await _userRepository.FirstOrDefaultAsync((long)output.Request.AssigneeId);
                output.UserName4 = user?.Name?.ToString() + ' ' + user?.Surname?.ToString();
            }

            var filteredTeamMembers = _techTeamRepository.GetAll()
                .Where(e => e.RequestId == output.Request.Id);

            var query = (from tt in filteredTeamMembers
                         join s1 in _userRepository.GetAll() on tt.CMACSUserId equals s1.Id into j2
                         from j3 in j2.DefaultIfEmpty()
                         select new TechTeamTmpDto
                         {
                             CMACSUserId = tt == null ? 0 : tt.CMACSUserId,
                             Name = j3 == null ? "" : j3.Name + ' ' + j3.Surname
                         });

            var techTeam = await query.ToListAsync();

            foreach (var item in techTeam)
            {
                string roles = "";
                var userRoles = await _userRoleRepository.GetAll().Where(x => x.UserId == item.CMACSUserId).ToListAsync();

                if (userRoles.Count > 0)
                {
                    foreach (var userRole in userRoles)
                    {
                        var roleName = (await _roleManager.GetRoleByIdAsync(userRole.RoleId)).DisplayName;
                        roles = roles + (roles == "" ? "" : ", ") + roleName;
                    }
                }

                //item.Role = roles;
                item.Name = item.Name + " (" + roles + ")";
            }

            output.TechTeam_ = techTeam;


            //Get Create audit information
            var created_user = await _userRepository.FirstOrDefaultAsync((int)request.CreatorUserId);
            output.CreatedBy = created_user.FullName;
            output.DateCreated = Convert.ToDateTime(request.CreationTime).ToString("dddd, dd MMMM yyyy");

            //Get update information
            if (request.LastModifierUserId != null)
            {
                var last_user = await _userRepository.FirstOrDefaultAsync((int)request.LastModifierUserId);
                output.LastUpdatedBy = last_user.FullName;
                output.LastUpdatedDate = Convert.ToDateTime(request.LastModificationTime).ToString("dddd, dd MMMM yyyy");
            }

            //Get approval audit info
            var requestTORApprovals = _torApprovalRepository.GetAll().Where(x => x.RequestId == output.Request.Id);
            var requestApprovals = _requestApprovalRepository.GetAll().Where(x => x.RequestId == output.Request.Id);
            var cmacsManagerApproval = await _cmacsManagerApprovalsRepository.FirstOrDefaultAsync(x => x.RequestId == output.Request.Id);

            if (requestTORApprovals != null && requestTORApprovals.Count() > 1)
            {
                var torApprovalsQuery = (from tor in requestTORApprovals
                                         join u in _userRepository.GetAll() on tor.ApproverId equals u.Id into u1
                                         from u2 in u1.DefaultIfEmpty()
                                         select new ApprovalAuditInfo
                                         {
                                             ApprovalDate = tor.ApprovedTime,
                                             ApprovalStatus = tor.Approved,
                                             ApprovalSentDate = tor.TORTimeSent,
                                             ApproverName = u2 == null ? "" : u2.FullName
                                         });
                output.TORApprovalsAuditInfo = await torApprovalsQuery.ToListAsync();
            }

            if (requestApprovals != null && requestApprovals.Count() > 1)
            {
                var approvalsQuery = (from ra in requestApprovals
                                      join u in _userRepository.GetAll() on ra.ApproverId equals u.Id into u1
                                      from u2 in u1.DefaultIfEmpty()
                                      select new ApprovalAuditInfo
                                      {
                                          ApprovalDate = ra.ApprovedTime,
                                          ApprovalStatus = ra.Approved,
                                          ApprovalSentDate = ra.ApprovalSentTime,
                                          ApproverName = u2 == null ? "" : u2.FullName
                                      });
                output.RequestApprovalsAuditInfo = await approvalsQuery.ToListAsync();
            }

            if (cmacsManagerApproval != null)
            {
                output.CmacsManagerApprovalInfo = new ApprovalAuditInfo
                {
                    ApprovalDate = cmacsManagerApproval.ApprovedTime,
                    ApprovalStatus = cmacsManagerApproval.Approved,
                    ApproverName = _userRepository.FirstOrDefault((int)cmacsManagerApproval.UserId).FullName
                };
            }


            //Get request attachments
            var requestAttachments = _attachedDocRepository.GetAll()
                                    .Where(x => x.RequestId == output.Request.Id)
                                    .Select(x => new AttachedDocs.Dtos.AttachedDocDto
                                    {
                                        AttachmentType = x.AttachmentType,
                                        RequestId = x.RequestId,
                                        DocumentId = x.DocumentId,
                                        FileName = x.FileName,
                                        Id = x.Id
                                    });

            if (requestAttachments.Count() > 0)
            {
                output.RequestAttachments = await requestAttachments.Where(x => x.AttachmentType == AttachmentTypes.Attachment).ToListAsync();
                output.SignedTORAttachments = await requestAttachments.Where(x => x.AttachmentType == AttachmentTypes.SignedTOR).ToListAsync();
                output.ReviewedFSAttachments = await requestAttachments.Where(x => x.AttachmentType == AttachmentTypes.ReviewedFS).ToListAsync();
            }

            //Get request sub areas
            var requestSubAreas = _requestSubAreaMappingRepository.GetAll()
                                                                  .Include(x => x.RequestSubAreaFk)
                                                                  .Where(x => x.RequestId == request.Id)
                                                                  .Select(x => new NameValueDto()
                                                                  {
                                                                      Value = x.RequestSubAreaFk != null ? x.Id.ToString() : null,
                                                                      Name = x.RequestSubAreaFk != null ? x.RequestSubAreaFk.Name : null
                                                                  }).ToList();
            output.SubAreas = requestSubAreas;
            output.NextAction = GetRequestNextAction(ObjectMapper.Map<RequestDto>(request), requestTORApprovals.ToList(), requestApprovals.ToList());

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRequestDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Requests_Create)]
        private async Task Create(CreateOrEditRequestDto input)
        {
            var request = ObjectMapper.Map<Request>(input);

            //Added to set status
            request.RequestStatusId = RequestStatus.Requested;
            request.RequestorId = AbpSession.UserId;
            request.SubmissionDate = DateTime.Now;
            //request.CompletionDate = DateTime.Now;
            if (request.RequestTypeId == RequestType.Enquiry)
            {
                request.RequiredResponseDate = DateTime.Now;
            }

            if (AbpSession.TenantId != null)
            {
                request.TenantId = (int?)AbpSession.TenantId;
            }

            //Had to use InsertAndGetIdAsync so i can generate unique reference code
            //await _requestRepository.InsertAsync(request);

            int requestId = await _requestRepository.InsertAndGetIdAsync(request);

            request.RequestNo = L("RequestNo_Prefix") + requestId.ToString().PadLeft(4, '0');

            await _requestRepository.UpdateAsync(request);

            //Save sub areas
            if (input.SubAreas != null)
            {
                await SaveSubAreas(input.SubAreas, requestId);
            }

            if (input.Attachments != null && input.Attachments.Count > 0)
            {
                input.Attachments.ForEach(x => x.RequestId = requestId);
                await _attachedDocsAppService.CreateOrEditMultipleDoc(input.Attachments);
            }

            //Send Emails to Requesting User and Ifrs Helpdesk 
            await SendEmailToRequestingUser();
            await SendEmailToHelpDesk();
        }

        private async Task SendEmailToRequestingUser()
        {
            string _toEmail = _userRepository.Get((long)AbpSession.UserId).EmailAddress;
            string _userName = _userRepository.Get((long)AbpSession.UserId).FullName;
            string _emailTitle = L("RequestReceived_Title");
            string _emailSubTitle = L("RequestReceived_SubTitle");
            string _emailSubject = L("RequestReceived_Email_Subject");

            var mailMessage = new StringBuilder();
            mailMessage.AppendLine(L("RequestReceived_Email_Body") + "<br />");

            try
            {
                await SwapTemplateAndSendEmail(_toEmail, _emailTitle, _emailSubTitle, _emailSubject, mailMessage, _userName);
            }
            catch
            {

            }
        }

        private async Task SendEmailToHelpDesk()
        {
            string _emailTitle = L("NewRequest_Title");
            string _emailSubTitle = L("NewRequest_SubTitle");
            string _emailSubject = L("NewRequest_Email_Subject");

            var mailMessageToHelpdesk = new StringBuilder();
            mailMessageToHelpdesk.AppendLine(L("NewRequest_Email_Body") + "<br />");

            //Had to hardcode this role, but will look for a better way
            var user = await _userManager.GetUsersInRoleAsync("IFRS_Helpdesk");

            foreach (var _user in user)
            {
                string _toEmail = _user.EmailAddress;
                try
                {
                    await SwapTemplateAndSendEmail(_toEmail, _emailTitle, _emailSubTitle, _emailSubject, mailMessageToHelpdesk, _user.FullName);
                }
                catch
                {

                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Requests_Edit)]
        private async Task Update(CreateOrEditRequestDto input)
        {
            var OriginalStatus = input.RequestStatusId;
            if (!string.IsNullOrWhiteSpace(input.TechGrpResponse))
            {
                input.RequestStatusId = RequestStatus.WIP;
            }

            var request = await _requestRepository.FirstOrDefaultAsync((int)input.Id);


            if (!string.IsNullOrWhiteSpace(input.EnquiryResponse) && !request.VoluntryRequiredTor)
            {
                input.RequestStatusId = RequestStatus.WIP;
            }
            ObjectMapper.Map(input, request);
            //Save sub areas
            if (input.SubAreas != null)
            {
                await SaveSubAreas(input.SubAreas, (int)input.Id);
            }

            if (input.RequestTypeId == RequestType.Enquiry && OriginalStatus == RequestStatus.Accepted)
            {
                await SubmitToCMASManagerForApproval((int)input.Id);
            }
        }

        private async Task SaveSubAreas(List<NameValueDto> subAreas, int requestId)
        {
            //Delete request sub area 
            await _requestSubAreaMappingRepository.DeleteAsync(x => x.RequestId == requestId);

            //Save new values
            foreach (var item in subAreas)
            {
                await _requestSubAreaMappingRepository.InsertAsync(new RequestSubAreaMapping
                {
                    RequestId = requestId,
                    RequestSubAreaId = Convert.ToInt32(item.Value)
                });
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Requests_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _requestRepository.DeleteAsync(input.Id);
        }

        public async Task CmacsManagerApproval(int input)
        {
            if (PermissionChecker.IsGranted(AppPermissions.Pages_Requests_CmacsManager))
            {
                var request = await _requestRepository.FirstOrDefaultAsync(input);

                if (request.RequestTypeId == RequestType.Enquiry)
                {
                    if (request.RequestStatusId != RequestStatus.Completed)
                    {
                        await MarkTreated(input);
                        await SaveCmacsManagerApproval(input);
                    }
                    else
                    {
                        throw new UserFriendlyException("This request has already been approved by a CMACS Manager.");
                    }
                }
                else
                {
                    if (request.RequestStatusId != RequestStatus.CMASManagerApproved)
                    {
                        request.RequestStatusId = RequestStatus.CMASManagerApproved;
                        await _requestRepository.UpdateAsync(request);

                        await SaveCmacsManagerApproval(input);
                    }
                    else
                    {
                        throw new UserFriendlyException("This request has already been approved by a CMACS Manager.");
                    }
                }
            }
            else
            {
                throw new UserFriendlyException("You don't have the permission to approve this request.");
            }

            //Notification to inform when consultation is completed

            //string _toEmail = _userRepository.Get((long)request.RequestorId).EmailAddress;
            //string _userName = _userRepository.Get((long)request.RequestorId).FullName;
            //string _emailTitle = L("RequestCompleted_Title");
            //string _emailSubTitle = L("RequestCompleted_SubTitle");
            //string _emailSubject = L("RequestCompleted_Email_Subject");

            //var mailMessage = new StringBuilder();
            //mailMessage.AppendLine(L("RequestCompleted_Email_Body") + "<br />");

            //try
            //{
            //    await SwapTemplateAndSendEmail(_toEmail, _emailTitle, _emailSubTitle, _emailSubject, mailMessage, _userName);
            //}
            //catch
            //{

            //}
        }

        public async Task SaveCmacsManagerApproval(int input)
        {
            await _cmacsManagerApprovalsRepository.InsertAsync(new RequestCmacsManagerApproval()
            {
                Approved = true,
                ApprovedTime = DateTime.Now,
                RequestId = input,
                UserId = AbpSession.UserId,
            });
        }

        public async Task MarkTreated(int input)
        {
            var request = await _requestRepository.FirstOrDefaultAsync(input);
            request.RequestStatusId = RequestStatus.Completed;
            request.CompletionDate = DateTime.Now;
            await _requestRepository.UpdateAsync(request);

            //Notification to inform when consultation is completed

            string _toEmail = _userRepository.Get((long)request.RequestorId).EmailAddress;
            string _userName = _userRepository.Get((long)request.RequestorId).FullName;
            string _emailTitle = L("RequestCompleted_Title");
            string _emailSubTitle = L("RequestCompleted_SubTitle");
            string _emailSubject = L("RequestCompleted_Email_Subject");

            var mailMessage = new StringBuilder();
            mailMessage.AppendLine(L("RequestCompleted_Email_Body") + "<br />");

            try
            {
                await SwapTemplateAndSendEmail(_toEmail, _emailTitle, _emailSubTitle, _emailSubject, mailMessage, _userName);
            }
            catch
            {

            }
        }

        #region Temporarily commented out the inital implementation because of performance consideration

        //public async Task SendTOR(CreateOrEditRequestDto input)
        //{
        //    var request = await _requestRepository.FirstOrDefaultAsync((int)input.Id);
        //    //request.TermsOfRefApproved = false;
        //    //request.RequestStatusId = RequestStatus.AwaitingTOR;

        //    input.TermsOfRefApproved = false;
        //    input.RequestStatusId = RequestStatus.AwaitingTOR;
        //    ObjectMapper.Map(input, request);

        //    //Email should be triggered when TOR is sent


        //    string _emailTitle = L("SendTOR_Title");
        //    string _emailSubTitle = L("SendTOR_SubTitle");
        //    string _emailSubject = L("SendTOR_Email_Subject");

        //    var mailMessage = new StringBuilder();
        //    mailMessage.AppendLine("<br>" + L("SendTOR_Email_Body") + "<br />");

        //    var torApproval = new TORApproval();
        //    torApproval.RequestId = request.Id;
        //    //Insert for manager
        //    torApproval.ApproverId = request.RequestorManagerId;
        //    torApproval.TORTimeSent = DateTime.Now;
        //    torApproval.Approved = false;

        //    if (AbpSession.TenantId != null)
        //    {
        //        torApproval.TenantId = (int?)AbpSession.TenantId;
        //    }

        //    //This code needs to be optimized such that once 
        //    //the insertion is made once we can get all into a list and iterate through

        //    //Insert for manager & send email
        //    await _torApprovalRepository.InsertAsync(torApproval);
        //    string _toEmail = _userRepository.Get((long)request.RequestorManagerId).EmailAddress;
        //    try
        //    {
        //        await SwapTemplateAndSendEmail(_toEmail, _emailTitle, _emailSubTitle, _emailSubject, mailMessage);
        //    }
        //    catch
        //    {

        //    }

        //    //Insert for partner & send email
        //    var torApprovalPartner = new TORApproval();
        //    torApprovalPartner.RequestId = request.Id;
        //    torApprovalPartner.ApproverId = request.RequestorPartnerId;
        //    torApprovalPartner.TORTimeSent = DateTime.Now;
        //    torApprovalPartner.Approved = false;

        //    if (AbpSession.TenantId != null)
        //    {
        //        torApprovalPartner.TenantId = (int?)AbpSession.TenantId;
        //    }

        //    await _torApprovalRepository.InsertAsync(torApprovalPartner);
        //    _toEmail = _userRepository.Get((long)request.RequestorPartnerId).EmailAddress;
        //    try
        //    {
        //        await SwapTemplateAndSendEmail(_toEmail, _emailTitle, _emailSubTitle, _emailSubject, mailMessage);
        //    }
        //    catch
        //    {

        //    }
        //}
        #endregion


        public async Task SubmitTorToCmacsManager(CreateRequestTORDto input)
        {
            var request = await _requestRepository.FirstOrDefaultAsync(input.RequestId);

            request.TermsOfRefApproved = false;
            request.RequestStatusId = RequestStatus.CMASManagerRequestReview;
            request.TermsOfRef = input.Tor;
            request.TORSentDate = DateTime.Now;
            request.HasSignedTOR = input.HasSignedTOR;

            if (input.HasSignedTOR)
            {
                //Skipp approval workflow
                request.TermsOfRefApproved = true;
                request.RequestStatusId = RequestStatus.WIP;
                request.TORApprovedDate = DateTime.Now;

                await _requestRepository.UpdateAsync(request);

            }
            else
            {
                await _requestRepository.UpdateAsync(request);
            }

        }

        public async Task SendTOR(CreateRequestTORDto input)
        {
            var request = await _requestRepository.FirstOrDefaultAsync(input.RequestId);

            request.TermsOfRefApproved = false;
            request.RequestStatusId = RequestStatus.AwaitingTOR;
            request.TermsOfRef = input.Tor;
            request.TORSentDate = DateTime.Now;
            request.HasSignedTOR = input.HasSignedTOR;



            //Email should be triggered when TOR is sent

            string _emailTitle = L("SendTOR_Title");
            string _emailSubTitle = L("SendTOR_SubTitle");
            string _emailSubject = L("SendTOR_Email_Subject");

            var mailMessage = new StringBuilder();
            mailMessage.AppendLine(L("SendTOR_Email_Body") + "<br />");

            if (input.HasSignedTOR)
            {
                //Skipp approval workflow
                request.TermsOfRefApproved = true;
                request.RequestStatusId = RequestStatus.WIP;
                request.TORApprovedDate = DateTime.Now;

                await _requestRepository.UpdateAsync(request);

            }
            else
            {
                await _requestRepository.UpdateAsync(request);


                var torApproval = new TORApproval();
                torApproval.RequestId = request.Id;
                //Insert for manager
                torApproval.ApproverId = request.RequestorManagerId;
                torApproval.TORTimeSent = DateTime.Now;
                torApproval.Approved = false;

                if (AbpSession.TenantId != null)
                {
                    torApproval.TenantId = (int?)AbpSession.TenantId;
                }

                //This code needs to be optimized such that once 
                //the insertion is made once we can get all into a list and iterate through

                //Insert for manager & send email
                await _torApprovalRepository.InsertAsync(torApproval);
                string _toEmail = _userRepository.Get((long)request.RequestorManagerId).EmailAddress;
                string _userName = _userRepository.Get((long)request.RequestorManagerId).FullName;
                try
                {
                    await SwapTemplateAndSendEmail(_toEmail, _emailTitle, _emailSubTitle, _emailSubject, mailMessage, _userName);
                }
                catch
                {

                }

                //Insert for partner & send email
                var torApprovalPartner = new TORApproval();
                torApprovalPartner.RequestId = request.Id;
                torApprovalPartner.ApproverId = request.RequestorPartnerId;
                torApprovalPartner.TORTimeSent = DateTime.Now;
                torApprovalPartner.Approved = false;

                if (AbpSession.TenantId != null)
                {
                    torApprovalPartner.TenantId = (int?)AbpSession.TenantId;
                }

                await _torApprovalRepository.InsertAsync(torApprovalPartner);
                _toEmail = _userRepository.Get((long)request.RequestorPartnerId).EmailAddress;
                _userName = _userRepository.Get((long)request.RequestorPartnerId).FullName;
                try
                {
                    await SwapTemplateAndSendEmail(_toEmail, _emailTitle, _emailSubTitle, _emailSubject, mailMessage, _userName);
                }
                catch
                {

                }
            }

        }

        public async Task ApproveTOR(int input)
        {
            var request = await _requestRepository.FirstOrDefaultAsync(input);

            var query = _torApprovalRepository.GetAll()
                                .Where(e => e.RequestId == input)
                                .Where(e => e.ApproverId == AbpSession.UserId)
                                .Where(e => e.Approved == false);

            var approver = await query.ToListAsync();

            if (!approver.Any(x => x.ApproverId == AbpSession.UserId))
            {
                throw new UserFriendlyException("You cannot approve this request!");
            }

            foreach (var _approver in approver)
            {
                _approver.Approved = true;
                _approver.ApprovedTime = DateTime.Now;
                await _torApprovalRepository.UpdateAsync(_approver);
                //_torApprovalRepository.Update(_approver);
            }

            var totalCount = _torApprovalRepository.GetAll()
                                .Where(e => e.RequestId == input && e.Approved == false)
                                //.Where(e => e.Approved == false)
                                .Count();


            //Notification to inform when TOR is approved
            string _toEmail = _userRepository.Get((long)request.RequestorId).EmailAddress;
            string _userName = _userRepository.Get((long)request.RequestorId).FullName;
            string _emailTitle = L("TORApproved_Title");
            string _emailSubTitle = L("TORApproved_SubTitle");
            string _emailSubject = L("TORApproved_Email_Subject");
            var mailMessage = new StringBuilder();
            mailMessage.AppendLine(L("TORApproved_Email_Body") + "<br />");


            //Temporary fix
            if (request.RequestorPartnerId == AbpSession.UserId)
            {
                request.TermsOfRefApproved = true;
                request.RequestStatusId = RequestStatus.WIP;
                request.TORApprovedDate = DateTime.Now;

                await _requestRepository.UpdateAsync(request);
                await SendEmailToTechTeamsMembers(request.Id, _emailSubject, _emailSubTitle, _emailSubject, mailMessage);
            }


            try
            {
                await SwapTemplateAndSendEmail(_toEmail, _emailTitle, _emailSubTitle, _emailSubject, mailMessage, _userName);
            }
            catch
            {

            }
        }

        public async Task SubmitToCMASManagerForApproval(int input)
        {
            var request = await _requestRepository.FirstOrDefaultAsync(input);

            if (request.RequestTypeId == RequestType.Enquiry)
            {
                request.RequestStatusId = RequestStatus.Accepted;
            }
            else
            {
                request.RequestStatusId = RequestStatus.Prepared;
                await SendRequestForApproval(input);
            }
            request.RequestSentDate = DateTime.Now;

            await _requestRepository.UpdateAsync(request);

            //Email should be triggered to the CMAS Manager on on the Technical team
            await SendEmailToTechTeamsCmacsManager(input);
        }

        private async Task SendEmailToTechTeamsCmacsManager(int input)
        {
            string _emailTitle = L("SendRequestForApproval_Title");
            string _emailSubTitle = L("SendRequestForApproval_SubTitle");
            string _emailSubject = L("SendRequestForApproval_Email_Subject");

            var mailMessage = new StringBuilder();
            mailMessage.AppendLine(L("SendRequestForApproval_Email_Body") + "<br />");

            //Get CMACS Manager and send email
            var techTeamMembers = await _techTeamRepository.GetAll().Where(x => x.RequestId == input).ToListAsync();
            foreach (var member in techTeamMembers)
            {
                var user = await UserManager.GetUserByIdAsync((long)member.CMACSUserId);
                if (PermissionChecker.IsGranted(user.ToUserIdentifier(), AppPermissions.Pages_Requests_CmacsManager))
                {
                    string _toEmail = user.EmailAddress;
                    string _userName = user.FullName;
                    try
                    {
                        await SwapTemplateAndSendEmail(_toEmail, _emailTitle, _emailSubTitle, _emailSubject, mailMessage, _userName);
                    }
                    catch
                    {

                    }
                }
            }
        }

        public async Task SendRequestForApproval(int input)
        {
            var request = await _requestRepository.FirstOrDefaultAsync(input);

            request.RequestStatusId = RequestStatus.Prepared;
            request.RequestSentDate = DateTime.Now;

            await _requestRepository.UpdateAsync(request);

            //Email should be triggered when TOR is sent

            string _emailTitle = L("SendRequestForApproval_Title");
            string _emailSubTitle = L("SendRequestForApproval_SubTitle");
            string _emailSubject = L("SendRequestForApproval_Email_Subject");

            var mailMessage = new StringBuilder();
            mailMessage.AppendLine(L("SendRequestForApproval_Email_Body") + "<br />");


            var requestApproval = new RequestApproval();
            requestApproval.RequestId = request.Id;

            //Insert for manager

            requestApproval.ApproverId = request.RequestorManagerId;
            requestApproval.ApprovalSentTime = DateTime.Now;
            requestApproval.Approved = false;


            if (AbpSession.TenantId != null)
            {
                requestApproval.TenantId = (int?)AbpSession.TenantId;
            }

            //This code needs to be optimized such that once 
            //the insertion is made once we can get all into a list and iterate through

            //Insert for manager & send email
            await _requestApprovalRepository.InsertAsync(requestApproval);
            string _toEmail = _userRepository.Get((long)request.RequestorManagerId).EmailAddress;
            string _userName = _userRepository.Get((long)request.RequestorManagerId).FullName;
            try
            {
                await SwapTemplateAndSendEmail(_toEmail, _emailTitle, _emailSubTitle, _emailSubject, mailMessage, _userName);
            }
            catch
            {

            }

            //Insert for partner & send email
            var requestApprovalPartner = new RequestApproval();
            requestApprovalPartner.RequestId = request.Id;
            requestApprovalPartner.ApproverId = request.RequestorPartnerId;
            requestApprovalPartner.ApprovalSentTime = DateTime.Now;
            requestApprovalPartner.Approved = false;

            if (AbpSession.TenantId != null)
            {
                requestApprovalPartner.TenantId = (int?)AbpSession.TenantId;
            }

            await _requestApprovalRepository.InsertAsync(requestApprovalPartner);
            _toEmail = _userRepository.Get((long)request.RequestorPartnerId).EmailAddress;
            _userName = _userRepository.Get((long)request.RequestorPartnerId).FullName;
            try
            {
                await SwapTemplateAndSendEmail(_toEmail, _emailTitle, _emailSubTitle, _emailSubject, mailMessage, _userName);
            }
            catch
            {

            }
        }

        public async Task ApproveRequest(int input)
        {
            var request = await _requestRepository.FirstOrDefaultAsync(input);

            var query = _requestApprovalRepository.GetAll()
                                .Where(e => e.RequestId == input)
                                .Where(e => e.ApproverId == AbpSession.UserId)
                                .Where(e => e.Approved == false);

            var approver = await query.ToListAsync();

            if (!approver.Any(x => x.ApproverId == AbpSession.UserId))
            {
                throw new UserFriendlyException("You cannot approve this request!");
            }

            foreach (var _approver in approver)
            {
                _approver.Approved = true;
                _approver.ApprovedTime = DateTime.Now;
                await _requestApprovalRepository.UpdateAsync(_approver);
            }

            var totalCount = _requestApprovalRepository.GetAll()
                                .Where(e => e.RequestId == input && e.Approved == false)
                                //.Where(e => e.Approved == false)
                                .Count();

            //Temporary fix
            if (request.RequestorPartnerId == AbpSession.UserId)
            {
                request.RequestStatusId = RequestStatus.Accepted;
                request.RequestApprovedDate = DateTime.Now;
                request.RequestApproved = true;
                await _requestRepository.UpdateAsync(request);
            }

            //Notification to inform when request is approved
            //To confirm who should get notification at this point

            string _toEmail = _userRepository.Get((long)request.RequestorId).EmailAddress;
            string _userName = _userRepository.Get((long)request.RequestorId).FullName;
            string _emailTitle = L("RequestApproval_Title");
            string _emailSubTitle = L("RequestApproval_SubTitle");
            string _emailSubject = L("RequestApproval_Email_Subject");

            var mailMessage = new StringBuilder();
            mailMessage.AppendLine(L("RequestApproval_Email_Body") + "<br />");

            try
            {
                await SwapTemplateAndSendEmail(_toEmail, _emailTitle, _emailSubTitle, _emailSubject, mailMessage, _userName);
            }
            catch
            {

            }

            await SendEmailToTechTeamsMembers(request.Id, _emailTitle, _emailSubTitle, _emailSubject, mailMessage);
        }

        private async Task SendEmailToTechTeamsMembers(int input, string _emailTitle, string _emailSubTitle, string _emailSubject, StringBuilder mailMessage)
        {
            //Get Tech team members and send email
            var techTeamMembers = await _techTeamRepository.GetAll().Where(x => x.RequestId == input).ToListAsync();
            foreach (var member in techTeamMembers)
            {
                var user = await UserManager.GetUserByIdAsync((long)member.CMACSUserId);
                string _toEmail = user.EmailAddress;
                string _userName = user.FullName;
                try
                {
                    await SwapTemplateAndSendEmail(_toEmail, _emailTitle, _emailSubTitle, _emailSubject, mailMessage, _userName);
                }
                catch
                {

                }
            }
        }

        public async Task UpdateRequestStatus(int requestid, RequestStatus status, string comment = "")
        {
            var request = await _requestRepository.FirstOrDefaultAsync(requestid);
            if (status == RequestStatus.Completed)
            {
                request.CompletionDate = DateTime.Now;
            }
            if (status == RequestStatus.Returned)
            {
                request.RequestStatusId = RequestStatus.Requested;
                request.ReturnComment = comment;
            }
            else
            {
                request.RequestStatusId = status;
            }
            await _requestRepository.UpdateAsync(request);
            if (status == RequestStatus.Returned && request.RequestorId != null)
            {
                await SendReturnEmailToRequestingUser((long)request.RequestorId);
            }
        }

        private async Task SendReturnEmailToRequestingUser(long requestCreator)
        {
            string _toEmail = _userRepository.Get((long)requestCreator).EmailAddress;
            string _userName = _userRepository.Get((long)requestCreator).FullName;
            string _emailTitle = L("RequestReturned_Title");
            string _emailSubTitle = L("RequestReceived_SubTitle");
            string _emailSubject = L("RequestReturned_Email_Subject");

            var mailMessage = new StringBuilder();
            mailMessage.AppendLine(L("RequestReturned_Email_Body") + "<br />");

            try
            {
                await SwapTemplateAndSendEmail(_toEmail, _emailTitle, _emailSubTitle, _emailSubject, mailMessage, _userName);
            }
            catch
            {

            }
        }

        public async Task<FileDto> GetRequestsToExcel(GetAllRequestsForExcelInput input)
        {
            var requestStatusIdFilter = (RequestStatus)input.RequestStatusIdFilter;

            var filteredRequests = _requestRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.LocalSubCode.Contains(input.Filter) || e.LocalChargeCode.Contains(input.Filter) || e.ReasonResponseDate.Contains(input.Filter) || e.IssueDiscussedWith.Contains(input.Filter) || e.OOTReviewer.Contains(input.Filter) || e.ConsultationIssue.Contains(input.Filter) || e.Background.Contains(input.Filter) || e.TechReference.Contains(input.Filter) || e.AgreedGuidance.Contains(input.Filter) || e.TechGrpResponse.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocalSubCodeFilter), e => e.LocalSubCode.ToLower() == input.LocalSubCodeFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LocalChargeCodeFilter), e => e.LocalChargeCode.ToLower() == input.LocalChargeCodeFilter.ToLower().Trim())
                        .WhereIf(input.MinSubmissionDateFilter != null, e => e.SubmissionDate >= input.MinSubmissionDateFilter)
                        .WhereIf(input.MaxSubmissionDateFilter != null, e => e.SubmissionDate <= input.MaxSubmissionDateFilter)
                        .WhereIf(input.MinRequiredResponseDateFilter != null, e => e.RequiredResponseDate >= input.MinRequiredResponseDateFilter)
                        .WhereIf(input.MaxRequiredResponseDateFilter != null, e => e.RequiredResponseDate <= input.MaxRequiredResponseDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReasonResponseDateFilter), e => e.ReasonResponseDate.ToLower() == input.ReasonResponseDateFilter.ToLower().Trim())
                        //.WhereIf(input.IssueDiscussedFilter > -1, e => Convert.ToInt32(e.IssueDiscussed) == input.IssueDiscussedFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IssueDiscussedWithFilter), e => e.IssueDiscussedWith.ToLower() == input.IssueDiscussedWithFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OOTReviewerFilter), e => e.OOTReviewer.ToLower() == input.OOTReviewerFilter.ToLower().Trim())
                        .WhereIf(input.MinOOTReviewerTimeFilter != null, e => e.OOTReviewerTime >= input.MinOOTReviewerTimeFilter)
                        .WhereIf(input.MaxOOTReviewerTimeFilter != null, e => e.OOTReviewerTime <= input.MaxOOTReviewerTimeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ConsultationIssueFilter), e => e.ConsultationIssue.ToLower() == input.ConsultationIssueFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BackgroundFilter), e => e.Background.ToLower() == input.BackgroundFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TechReferenceFilter), e => e.TechReference.ToLower() == input.TechReferenceFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AgreedGuidanceFilter), e => e.AgreedGuidance.ToLower() == input.AgreedGuidanceFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TechGrpResponseFilter), e => e.TechGrpResponse.ToLower() == input.TechGrpResponseFilter.ToLower().Trim())
                        .WhereIf(input.MinCompletionDateFilter != null, e => e.CompletionDate >= input.MinCompletionDateFilter)
                        .WhereIf(input.MaxCompletionDateFilter != null, e => e.CompletionDate <= input.MaxCompletionDateFilter)
                        .WhereIf(input.RequestStatusIdFilter > -1, e => e.RequestStatusId == requestStatusIdFilter);


            var query = (from o in filteredRequests
                         join o1 in _requestAreaRepository.GetAll() on o.RequestAreaId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _requestDomainRepository.GetAll() on o.RequestDomainId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         join o3 in _userRepository.GetAll() on o.RequestorId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         join o4 in _userRepository.GetAll() on o.RequestorPartnerId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()
                         join o5 in _userRepository.GetAll() on o.RequestorManagerId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()
                         join o6 in _clientListRepository.GetAll() on o.ClientListId equals o6.Id into j6
                         from s6 in j6.DefaultIfEmpty()
                         join o7 in _userRepository.GetAll() on o.AssigneeId equals o7.Id into j7
                         from s7 in j7.DefaultIfEmpty()

                         select new GetRequestForView()
                         {
                             Request = ObjectMapper.Map<RequestDto>(o),
                             RequestAreaRequestAreaName = s1 == null ? "" : s1.RequestAreaName.ToString(),
                             RequestDomainDomainName = s2 == null ? "" : s2.DomainName.ToString(),
                             RequestorName = s3 == null ? "" : s3.FullName.ToString(),
                             PartnerName = s4 == null ? "" : s4.FullName.ToString(),
                             ManagerName = s5 == null ? "" : s5.FullName.ToString(),
                             ClientListClientName = s6 == null ? "" : s6.ClientName.ToString(),
                             AssigneeName = s7 == null ? "" : s7.Name.ToString()
                         })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.RequestAreaRequestAreaNameFilter), e => e.RequestAreaRequestAreaName.ToLower() == input.RequestAreaRequestAreaNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RequestDomainDomainNameFilter), e => e.RequestDomainDomainName.ToLower() == input.RequestDomainDomainNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.RequestorName.ToLower() == input.UserNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.PartnerName.ToLower() == input.UserName2Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserName3Filter), e => e.ManagerName.ToLower() == input.UserName3Filter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ClientListClientNameFilter), e => e.ClientListClientName.ToLower() == input.ClientListClientNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserName4Filter), e => e.AssigneeName.ToLower() == input.UserName4Filter.ToLower().Trim());


            var requestListDtos = await query.ToListAsync();

            var requestForExportListDto = new List<RequestForExcelExport>();

            foreach (var request in requestListDtos)
            {
                var requestForExport = new RequestForExcelExport();

                requestForExport.Request = request;
                requestForExport.TechTeam_ = await GetRequestTechTeamMembers(request.Request.Id);
                requestForExport.SubAreas = GetRequestSubAreas(request.Request.Id);

                requestForExportListDto.Add(requestForExport);
            }

            return _requestsExcelExporter.ExportToFile(requestForExportListDto);
        }

        private async Task<List<TechTeamTmpDto>> GetRequestTechTeamMembers(int RequestId)
        {

            var filteredTeamMembers = _techTeamRepository.GetAll()
                .Where(e => e.RequestId == RequestId);

            var query = (from tt in filteredTeamMembers
                         join s1 in _userRepository.GetAll() on tt.CMACSUserId equals s1.Id into j2
                         from j3 in j2.DefaultIfEmpty()
                         select new TechTeamTmpDto
                         {
                             CMACSUserId = tt == null ? 0 : tt.CMACSUserId,
                             Name = j3 == null ? "" : j3.Name + ' ' + j3.Surname
                         });

            var techTeam = await query.ToListAsync();

            foreach (var item in techTeam)
            {
                string roles = "";
                var userRoles = await _userRoleRepository.GetAll().Where(x => x.UserId == item.CMACSUserId).ToListAsync();

                if (userRoles.Count > 0)
                {
                    foreach (var userRole in userRoles)
                    {
                        var roleName = (await _roleManager.GetRoleByIdAsync(userRole.RoleId)).DisplayName;
                        roles = roles + (roles == "" ? "" : ", ") + roleName;
                    }
                }

                //item.Role = roles;
                item.Name = item.Name + " (" + roles + ")";
            }

            return techTeam;
        }

        private List<NameValueDto> GetRequestSubAreas(int RequestId)
        {
            var requestSubAreas = _requestSubAreaMappingRepository.GetAll()
                                                                  .Include(x => x.RequestSubAreaFk)
                                                                  .Where(x => x.RequestId == RequestId)
                                                                  .Select(x => new NameValueDto()
                                                                  {
                                                                      Value = x.RequestSubAreaFk != null ? x.Id.ToString() : null,
                                                                      Name = x.RequestSubAreaFk != null ? x.RequestSubAreaFk.Name : null
                                                                  }).ToList();
            return requestSubAreas;
        }

        [AbpAuthorize(AppPermissions.Pages_Requests)]
        public async Task<PagedResultDto<RequestAreaLookupTableDto>> GetAllRequestAreaForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _requestAreaRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.RequestAreaName.ToString().ToLower().Contains(input.Filter.ToLower())
               );

            var totalCount = await query.CountAsync();

            var requestAreaList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<RequestAreaLookupTableDto>();
            foreach (var requestArea in requestAreaList)
            {
                lookupTableDtoList.Add(new RequestAreaLookupTableDto
                {
                    Id = requestArea.Id,
                    DisplayName = requestArea.RequestAreaName.ToString()
                });
            }

            return new PagedResultDto<RequestAreaLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Requests)]
        public async Task<PagedResultDto<RequestDomainLookupTableDto>> GetAllRequestDomainForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _requestDomainRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DomainName.ToString().ToLower().Contains(input.Filter.ToLower())
               );

            var totalCount = await query.CountAsync();

            var requestDomainList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<RequestDomainLookupTableDto>();
            foreach (var requestDomain in requestDomainList)
            {
                lookupTableDtoList.Add(new RequestDomainLookupTableDto
                {
                    Id = requestDomain.Id,
                    DisplayName = requestDomain.DomainName.ToString()
                });
            }

            return new PagedResultDto<RequestDomainLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Requests)]
        public async Task<PagedResultDto<UserLookupTableDto_4Tech>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _userRepository.GetAll().Include(x => x.Roles)
                .WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name.ToLower().Contains(input.Filter.ToLower()) || e.Surname.ToLower().Contains(input.Filter.ToLower())
               );

            var helpdesk_user = await _userManager.GetUsersInRoleAsync("IFRS_Helpdesk");

            foreach (var _user in helpdesk_user)
            {

            }

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<UserLookupTableDto_4Tech>();
            foreach (var user in userList)
            {
                string roles = "";
                var userRoles = await _userRoleRepository.GetAll().Where(x => x.UserId == user.Id).ToListAsync();

                if (userRoles.Count > 0)
                {
                    foreach (var userRole in userRoles)
                    {
                        var roleName = (await _roleManager.GetRoleByIdAsync(userRole.RoleId)).DisplayName;
                        roles = roles + (roles == "" ? "" : ", ") + roleName;
                    }
                }

                lookupTableDtoList.Add(new UserLookupTableDto_4Tech
                {
                    Id = user.Id,
                    DisplayName = user.Name.ToString() + ' ' + user.Surname.ToString() + " (" + roles + ")"
                });

            }

            return new PagedResultDto<UserLookupTableDto_4Tech>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Requests)]
        public async Task<PagedResultDto<UserLookupTableDto_4Tech>> GetAllUserForRoleForLookupTable(GetUsersForRoleForLookupTableInput input)
        {
            var roleUsers = await UserManager.GetUsersInRoleAsync(input.Role);
            var query = _userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name.ToLower().Contains(input.Filter.ToLower()) || e.Surname.ToLower().Contains(input.Filter.ToLower())
               );

            var totalCount = query.Count();
            var userList = query.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var lookupTableDtoList = new List<UserLookupTableDto_4Tech>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new UserLookupTableDto_4Tech
                {
                    Id = user.Id,
                    DisplayName = user.Name.ToString() + ' ' + user.Surname.ToString()
                });

            }

            return new PagedResultDto<UserLookupTableDto_4Tech>(
                totalCount,
                lookupTableDtoList
            );
        }


        [AbpAuthorize(AppPermissions.Pages_Requests)]
        public async Task<PagedResultDto<ClientListLookupTableDto>> GetAllClientListForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _clientListRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.ClientName.ToLower().Contains(input.Filter.ToLower())
               );

            var totalCount = await query.CountAsync();

            var clientListList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ClientListLookupTableDto>();
            foreach (var clientList in clientListList)
            {
                lookupTableDtoList.Add(new ClientListLookupTableDto
                {
                    Id = clientList.Id,
                    DisplayName = clientList.ClientName?.ToString()
                });
            }

            return new PagedResultDto<ClientListLookupTableDto>(
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

        public List<GetRequestStatistics> GetStatInfo()
        {

            var query = (from tt in _requestRepository.GetAll()
                         group tt by tt.RequestStatusId into requestsumm
                         select requestsumm
                         );

            return null;
        }

    }
}