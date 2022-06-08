using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using Kad.PMSDemo.Common.Dto;
using Kad.PMSDemo.Editions;
using Kad.PMSDemo.Editions.Dto;
using Abp.Domain.Repositories;
using Test.RequestAreas;
using Test.RequestDomains;
using Test.Requests;
using System.Collections.Generic;

namespace Kad.PMSDemo.Common
{
    [AbpAuthorize]
    public class CommonLookupAppService : PMSDemoAppServiceBase, ICommonLookupAppService
    {
        private readonly EditionManager _editionManager;
        private readonly IRepository<RequestArea> _requestAreaRepository;
        private readonly IRepository<RequestDomain> _requestDomainRepository;
        private readonly IRepository<RequestSubArea> _requestSubAreaRepository;
        private readonly IRepository<RequestSubAreaMapping> _requestSubAreaMappingRepository;

        public CommonLookupAppService(EditionManager editionManager, IRepository<RequestArea> requestAreaRepository, IRepository<RequestDomain> requestDomainRepository,
                                      IRepository<RequestSubArea> requestSubAreaRepository, IRepository<RequestSubAreaMapping> requestSubAreaMappingRepository)
        {
            _editionManager = editionManager;
            _requestAreaRepository = requestAreaRepository;
            _requestDomainRepository = requestDomainRepository;
            _requestSubAreaRepository = requestSubAreaRepository;
            _requestSubAreaMappingRepository = requestSubAreaMappingRepository;
        }

        public async Task<ListResultDto<SubscribableEditionComboboxItemDto>> GetEditionsForCombobox(bool onlyFreeItems = false)
        {
            var subscribableEditions = (await _editionManager.Editions.Cast<SubscribableEdition>().ToListAsync())
                .WhereIf(onlyFreeItems, e => e.IsFree)
                .OrderBy(e => e.MonthlyPrice);

            return new ListResultDto<SubscribableEditionComboboxItemDto>(
                subscribableEditions.Select(e => new SubscribableEditionComboboxItemDto(e.Id.ToString(), e.DisplayName, e.IsFree)).ToList()
            );
        }

        public async Task<PagedResultDto<NameValueDto>> FindUsers(FindUsersInput input)
        {
            if (AbpSession.TenantId != null)
            {
                //Prevent tenants to get other tenant's users.
                input.TenantId = AbpSession.TenantId;
            }

            using (CurrentUnitOfWork.SetTenantId(input.TenantId))
            {
                var query = UserManager.Users
                    .WhereIf(
                        !input.Filter.IsNullOrWhiteSpace(),
                        u =>
                            u.Name.Contains(input.Filter) ||
                            u.Surname.Contains(input.Filter) ||
                            u.UserName.Contains(input.Filter) ||
                            u.EmailAddress.Contains(input.Filter)
                    ).WhereIf(input.ExcludeCurrentUser, u => u.Id != AbpSession.GetUserId());

                var userCount = await query.CountAsync();
                var users = await query
                    .OrderBy(u => u.Name)
                    .ThenBy(u => u.Surname)
                    .PageBy(input)
                    .ToListAsync();

                return new PagedResultDto<NameValueDto>(
                    userCount,
                    users.Select(u =>
                        new NameValueDto(
                            u.FullName + " (" + u.EmailAddress + ")",
                            u.Id.ToString()
                            )
                        ).ToList()
                    );
            }
        }

        public GetDefaultEditionNameOutput GetDefaultEditionName()
        {
            return new GetDefaultEditionNameOutput
            {
                Name = EditionManager.DefaultEditionName
            };
        }


        public async Task<List<NameValueDto>> GetAllRequestAreas()
        {
            var requestAreas = await _requestAreaRepository.GetAll()
                               .Select(x => new NameValueDto
                               {
                                   Value = x.Id.ToString(),
                                   Name = x.RequestAreaName
                               }).ToListAsync();

            return requestAreas;
        }

        public async Task<List<NameValueDto>> GetAllRequestDomains()
        {
            var requestDomains = await _requestDomainRepository.GetAll()
                               .Select(x => new NameValueDto
                               {
                                   Value = x.Id.ToString(),
                                   Name = x.DomainName
                               }).ToListAsync();

            return requestDomains;
        }

        public async Task<List<NameValueDto>> GetAllRequestSubArea()
        {
            var requestDomains = await _requestSubAreaRepository.GetAll()
                               .Select(x => new NameValueDto
                               {
                                   Value = x.Id.ToString(),
                                   Name = x.Name
                               }).ToListAsync();

            return requestDomains;
        }

        public async Task<List<NameValueDto>> GetRequestsSubAreas(EntityDto<int> input)
        {
            var requestDomains = await _requestSubAreaMappingRepository.GetAll()
                                                                       .Include(x => x.RequestSubAreaFk)
                                                                       .Where(x => x.RequestId == input.Id)
                               .Select(x => new NameValueDto
                               {
                                   Value = x.RequestSubAreaFk != null ? x.RequestSubAreaFk.Id.ToString() : null,
                                   Name = x.RequestSubAreaFk != null ? x.RequestSubAreaFk.Name : null
                               }).ToListAsync();

            return requestDomains;
        }
    }
}
