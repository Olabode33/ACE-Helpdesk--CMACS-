
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Test.ReportingTerritories.Exporting;
using Test.ReportingTerritories.Dtos;
using Kad.PMSDemo.Dto;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Kad.PMSDemo;

namespace Test.ReportingTerritories
{
	[AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
    public class ReportingTerritoriesAppService : PMSDemoAppServiceBase, IReportingTerritoriesAppService
    {
		 private readonly IRepository<ReportingTerritory> _reportingTerritoryRepository;
		 private readonly IReportingTerritoriesExcelExporter _reportingTerritoriesExcelExporter;
		 

		  public ReportingTerritoriesAppService(IRepository<ReportingTerritory> reportingTerritoryRepository, IReportingTerritoriesExcelExporter reportingTerritoriesExcelExporter ) 
		  {
			_reportingTerritoryRepository = reportingTerritoryRepository;
			_reportingTerritoriesExcelExporter = reportingTerritoriesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetReportingTerritoryForView>> GetAll(GetAllReportingTerritoriesInput input)
         {
            var filtered = _reportingTerritoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TerritoryName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TerritoryNameFilter), e => e.TerritoryName.ToLower() == input.TerritoryNameFilter.ToLower().Trim());

            var pagedAndFiltered = filtered
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var query = (from o in pagedAndFiltered

                         select new GetReportingTerritoryForView()
                         {
                             ReportingTerritory = ObjectMapper.Map<ReportingTerritoryDto>(o)

                         });

            var totalCount = await filtered.CountAsync();

            return new PagedResultDto<GetReportingTerritoryForView>(
                totalCount,
                await query.ToListAsync()
            );

         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
		 public async Task<GetReportingTerritoryForEditOutput> GetReportingTerritoryForEdit(EntityDto input)
         {
            var reportingTerritory = await _reportingTerritoryRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetReportingTerritoryForEditOutput {ReportingTerritory = ObjectMapper.Map<CreateOrEditReportingTerritoryDto>(reportingTerritory)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditReportingTerritoryDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
		 private async Task Create(CreateOrEditReportingTerritoryDto input)
         {
            var reportingTerritory = ObjectMapper.Map<ReportingTerritory>(input);

			
			if (AbpSession.TenantId != null)
			{
				reportingTerritory.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _reportingTerritoryRepository.InsertAsync(reportingTerritory);
         }

		 [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
		 private async Task Update(CreateOrEditReportingTerritoryDto input)
         {
            var reportingTerritory = await _reportingTerritoryRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, reportingTerritory);
         }

		 [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
         public async Task Delete(EntityDto input)
         {
            await _reportingTerritoryRepository.DeleteAsync(input.Id);
         }

		 public async Task<FileDto> GetReportingTerritoriesToExcel(GetAllReportingTerritoriesForExcelInput input)
         {

            var filtered = _reportingTerritoryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TerritoryName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TerritoryNameFilter), e => e.TerritoryName.ToLower() == input.TerritoryNameFilter.ToLower().Trim());

            var query = (from o in filtered

                         select new GetReportingTerritoryForView()
                         {
                             ReportingTerritory = ObjectMapper.Map<ReportingTerritoryDto>(o)

                         });


            var reportingTerritoryListDtos = await query.ToListAsync();

            return _reportingTerritoriesExcelExporter.ExportToFile(reportingTerritoryListDtos);
         }


    }
}