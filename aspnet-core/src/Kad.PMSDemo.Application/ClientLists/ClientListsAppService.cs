using Test.Industries;
using Test.ReportingTerritories;
using Test.StockExchanges;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Test.ClientLists.Exporting;
using Test.ClientLists.Dtos;
using Kad.PMSDemo.Dto;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Kad.PMSDemo;

namespace Test.ClientLists
{
    [AbpAuthorize]
    public class ClientListsAppService : PMSDemoAppServiceBase, IClientListsAppService
    {
        private readonly IRepository<ClientList> _clientListRepository;
        private readonly IClientListsExcelExporter _clientListsExcelExporter;
        private readonly IRepository<Industry, int> _industryRepository;
        private readonly IRepository<ReportingTerritory, int> _reportingTerritoryRepository;
        private readonly IRepository<StockExchange, int> _stockExchangeRepository;


        public ClientListsAppService(IRepository<ClientList> clientListRepository, IClientListsExcelExporter clientListsExcelExporter, IRepository<Industry, int> industryRepository, IRepository<ReportingTerritory, int> reportingTerritoryRepository, IRepository<StockExchange, int> stockExchangeRepository)
        {
            _clientListRepository = clientListRepository;
            _clientListsExcelExporter = clientListsExcelExporter;
            _industryRepository = industryRepository;
            _reportingTerritoryRepository = reportingTerritoryRepository;
            _stockExchangeRepository = stockExchangeRepository;

        }

        public async Task<PagedResultDto<GetClientListForView>> GetAll(GetAllClientListsInput input)
        {
            var financialYearEndFilter = (FinYearEnd)input.FinancialYearEndFilter;
            var channelTypeNameFilter = (ChannelType)input.ChannelTypeNameFilter;

            var filtered = _clientListRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ClientName.Contains(input.Filter) || e.ClientAddress.Contains(input.Filter) || e.ParentEntity.Contains(input.Filter) || e.UltimateParentEntity.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ClientNameFilter), e => e.ClientName.ToLower() == input.ClientNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ClientAddressFilter), e => e.ClientAddress.ToLower() == input.ClientAddressFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ParentEntityFilter), e => e.ParentEntity.ToLower() == input.ParentEntityFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UltimateParentEntityFilter), e => e.UltimateParentEntity.ToLower() == input.UltimateParentEntityFilter.ToLower().Trim())
                        //.WhereIf(input.SecRegisteredFilter > -1, e => Convert.ToInt32(e.SecRegistered) == input.SecRegisteredFilter)
                        .WhereIf(input.FinancialYearEndFilter > -1, e => e.FinancialYearEnd == financialYearEndFilter)
                        .WhereIf(input.ChannelTypeNameFilter > -1, e => e.ChannelTypeName == channelTypeNameFilter);

            var pagedAndFiltered = filtered
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var query = (from o in pagedAndFiltered
                         join o1 in _industryRepository.GetAll() on o.IndustryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _reportingTerritoryRepository.GetAll() on o.ReportingTerritoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         join o3 in _stockExchangeRepository.GetAll() on o.StockExchangeId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetClientListForView()
                         {
                             ClientList = ObjectMapper.Map<ClientListDto>(o),
                             IndustryIndustryName = s1 == null ? "" : s1.IndustryName.ToString(),
                             ReportingTerritoryTerritoryName = s2 == null ? "" : s2.TerritoryName.ToString(),
                             StockExchangeStockExchangeName = s3 == null ? "" : s3.StockExchangeName.ToString()

                         })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndustryIndustryNameFilter), e => e.IndustryIndustryName.ToLower() == input.IndustryIndustryNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReportingTerritoryTerritoryNameFilter), e => e.ReportingTerritoryTerritoryName.ToLower() == input.ReportingTerritoryTerritoryNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StockExchangeStockExchangeNameFilter), e => e.StockExchangeStockExchangeName.ToLower() == input.StockExchangeStockExchangeNameFilter.ToLower().Trim());

            var totalCount = await filtered.CountAsync();

            return new PagedResultDto<GetClientListForView>(
                totalCount,
                await query.ToListAsync()
            );
        }

        public async Task<GetClientListForEditOutput> GetClientListForEdit(EntityDto input)
        {
            var clientList = await _clientListRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetClientListForEditOutput { ClientList = ObjectMapper.Map<CreateOrEditClientListDto>(clientList) };

            if (output.ClientList.IndustryId != null)
            {
                var industry = await _industryRepository.FirstOrDefaultAsync((int)output.ClientList.IndustryId);
                output.IndustryIndustryName = industry.IndustryName.ToString();
            }

            if (output.ClientList.ReportingTerritoryId != null)
            {
                var reportingTerritory = await _reportingTerritoryRepository.FirstOrDefaultAsync((int)output.ClientList.ReportingTerritoryId);
                output.ReportingTerritoryTerritoryName = reportingTerritory.TerritoryName.ToString();
            }

            if (output.ClientList.StockExchangeId != null)
            {
                var stockExchange = await _stockExchangeRepository.FirstOrDefaultAsync((int)output.ClientList.StockExchangeId);
                output.StockExchangeStockExchangeName = stockExchange.StockExchangeName.ToString();
            }

            return output;
        }

        public async Task<CreateOrEditClientListDto> CreateOrEdit(CreateOrEditClientListDto input)
        {
            if (input.Id == null)
            {
                return await Create(input);
            }
            else
            {
                return await Update(input);
            }
        }

        private async Task<CreateOrEditClientListDto> Create(CreateOrEditClientListDto input)
        {
            var clientList = ObjectMapper.Map<ClientList>(input);


            if (AbpSession.TenantId != null)
            {
                clientList.TenantId = (int?)AbpSession.TenantId;
            }


            var id = await _clientListRepository.InsertAndGetIdAsync(clientList);
            input.Id = id;
            return input;
        }

        private async Task<CreateOrEditClientListDto> Update(CreateOrEditClientListDto input)
        {
            var clientList = await _clientListRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, clientList);
            return input;
        }

        [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
        public async Task Delete(EntityDto input)
        {
            await _clientListRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetClientListsToExcel(GetAllClientListsForExcelInput input)
        {
            var financialYearEndFilter = (FinYearEnd)input.FinancialYearEndFilter;
            var channelTypeNameFilter = (ChannelType)input.ChannelTypeNameFilter;

            var filtered = _clientListRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ClientName.Contains(input.Filter) || e.ClientAddress.Contains(input.Filter) || e.ParentEntity.Contains(input.Filter) || e.UltimateParentEntity.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ClientNameFilter), e => e.ClientName.ToLower() == input.ClientNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ClientAddressFilter), e => e.ClientAddress.ToLower() == input.ClientAddressFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ParentEntityFilter), e => e.ParentEntity.ToLower() == input.ParentEntityFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UltimateParentEntityFilter), e => e.UltimateParentEntity.ToLower() == input.UltimateParentEntityFilter.ToLower().Trim())
                        //.WhereIf(input.SecRegisteredFilter > -1, e => Convert.ToInt32(e.SecRegistered) == input.SecRegisteredFilter)
                        .WhereIf(input.FinancialYearEndFilter > -1, e => e.FinancialYearEnd == financialYearEndFilter)
                        .WhereIf(input.ChannelTypeNameFilter > -1, e => e.ChannelTypeName == channelTypeNameFilter);

            var query = (from o in filtered
                         join o1 in _industryRepository.GetAll() on o.IndustryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         join o2 in _reportingTerritoryRepository.GetAll() on o.ReportingTerritoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         join o3 in _stockExchangeRepository.GetAll() on o.StockExchangeId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetClientListForView()
                         {
                             ClientList = ObjectMapper.Map<ClientListDto>(o),
                             IndustryIndustryName = s1 == null ? "" : s1.IndustryName.ToString(),
                             ReportingTerritoryTerritoryName = s2 == null ? "" : s2.TerritoryName.ToString(),
                             StockExchangeStockExchangeName = s3 == null ? "" : s3.StockExchangeName.ToString()

                         })

                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndustryIndustryNameFilter), e => e.IndustryIndustryName.ToLower() == input.IndustryIndustryNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReportingTerritoryTerritoryNameFilter), e => e.ReportingTerritoryTerritoryName.ToLower() == input.ReportingTerritoryTerritoryNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StockExchangeStockExchangeNameFilter), e => e.StockExchangeStockExchangeName.ToLower() == input.StockExchangeStockExchangeNameFilter.ToLower().Trim());


            var clientListListDtos = await query.ToListAsync();

            return _clientListsExcelExporter.ExportToFile(clientListListDtos);
        }

        public async Task<PagedResultDto<IndustryLookupTableDto>> GetAllIndustryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _industryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.IndustryName.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var industryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<IndustryLookupTableDto>();
            foreach (var industry in industryList)
            {
                lookupTableDtoList.Add(new IndustryLookupTableDto
                {
                    Id = industry.Id,
                    DisplayName = industry.IndustryName.ToString()
                });
            }

            return new PagedResultDto<IndustryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        
        [AbpAuthorize]
        public async Task<PagedResultDto<ReportingTerritoryLookupTableDto>> GetAllReportingTerritoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _reportingTerritoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.TerritoryName.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var reportingTerritoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ReportingTerritoryLookupTableDto>();
            foreach (var reportingTerritory in reportingTerritoryList)
            {
                lookupTableDtoList.Add(new ReportingTerritoryLookupTableDto
                {
                    Id = reportingTerritory.Id,
                    DisplayName = reportingTerritory.TerritoryName.ToString()
                });
            }

            return new PagedResultDto<ReportingTerritoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        
        [AbpAuthorize]
        public async Task<PagedResultDto<StockExchangeLookupTableDto>> GetAllStockExchangeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _stockExchangeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.StockExchangeName.ToString().Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var stockExchangeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StockExchangeLookupTableDto>();
            foreach (var stockExchange in stockExchangeList)
            {
                lookupTableDtoList.Add(new StockExchangeLookupTableDto
                {
                    Id = stockExchange.Id,
                    DisplayName = stockExchange.StockExchangeName.ToString()
                });
            }

            return new PagedResultDto<StockExchangeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}