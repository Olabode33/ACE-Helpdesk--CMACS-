
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Test.Industries.Exporting;
using Test.Industries.Dtos;
using Kad.PMSDemo.Dto;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Kad.PMSDemo;
using Abp.Linq.Extensions;

namespace Test.Industries
{
    [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
    public class IndustriesAppService : PMSDemoAppServiceBase, IIndustriesAppService
    {
        private readonly IRepository<Industry> _industryRepository;
        private readonly IIndustriesExcelExporter _industriesExcelExporter;


        public IndustriesAppService(IRepository<Industry> industryRepository, IIndustriesExcelExporter industriesExcelExporter)
        {
            _industryRepository = industryRepository;
            _industriesExcelExporter = industriesExcelExporter;

        }

        public async Task<PagedResultDto<GetIndustryForView>> GetAll(GetAllIndustriesInput input)
        {

            var filteredPositions = _industryRepository.GetAll()
                       .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IndustryName.Contains(input.Filter));

            var pagedAndFilteredPositions = filteredPositions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var positions = from o in pagedAndFilteredPositions
                            select new GetIndustryForView()
                            {
                                Industry = ObjectMapper.Map<IndustryDto>(o)
                            };

            var totalCount = await filteredPositions.CountAsync();

            return new PagedResultDto<GetIndustryForView>(
                totalCount,
                await positions.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
        public async Task<GetIndustryForEditOutput> GetIndustryForEdit(EntityDto input)
        {
            var industry = await _industryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetIndustryForEditOutput { Industry = ObjectMapper.Map<CreateOrEditIndustryDto>(industry) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditIndustryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
        private async Task Create(CreateOrEditIndustryDto input)
        {

            var industry = ObjectMapper.Map<Industry>(input);


            if (AbpSession.TenantId != null)
            {
                industry.TenantId = (int?)AbpSession.TenantId;
            }


            await _industryRepository.InsertAsync(industry);
        }

        [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
        private async Task Update(CreateOrEditIndustryDto input)
        {
            var industry = await _industryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, industry);
        }

        [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
        public async Task Delete(EntityDto input)
        {
            await _industryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetIndustriesToExcel(GetAllIndustriesForExcelInput input)
        {

            var filteredIndustries = _industryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.IndustryName.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndustryNameFilter), e => e.IndustryName.ToLower() == input.IndustryNameFilter.ToLower().Trim());


            var query = (from o in filteredIndustries

                         select new GetIndustryForView()
                         {
                             Industry = ObjectMapper.Map<IndustryDto>(o)

                         })
                         ;


            var industryListDtos = await query.ToListAsync();

            return _industriesExcelExporter.ExportToFile(industryListDtos);
        }


    }
}