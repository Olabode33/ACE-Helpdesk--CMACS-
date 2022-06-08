
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Test.StockExchanges.Exporting;
using Test.StockExchanges.Dtos;
using Kad.PMSDemo.Dto;
using Abp.Application.Services.Dto;
using Kad.PMSDemo.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Kad.PMSDemo;

namespace Test.StockExchanges
{
	[AbpAuthorize]
    public class StockExchangesAppService : PMSDemoAppServiceBase, IStockExchangesAppService
    {
		 private readonly IRepository<StockExchange> _stockExchangeRepository;
		 private readonly IStockExchangesExcelExporter _stockExchangesExcelExporter;
		 

		  public StockExchangesAppService(IRepository<StockExchange> stockExchangeRepository, IStockExchangesExcelExporter stockExchangesExcelExporter ) 
		  {
			_stockExchangeRepository = stockExchangeRepository;
			_stockExchangesExcelExporter = stockExchangesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetStockExchangeForView>> GetAll(GetAllStockExchangesInput input)
         {

            var filtered = _stockExchangeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.StockExchangeName.Contains(input.Filter) || e.Country.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StockExchangeNameFilter), e => e.StockExchangeName.ToLower() == input.StockExchangeNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryFilter), e => e.Country.ToLower() == input.CountryFilter.ToLower().Trim());


            var pagedAndFiltered = filtered
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var query = (from o in pagedAndFiltered

                         select new GetStockExchangeForView()
                         {
                             StockExchange = ObjectMapper.Map<StockExchangeDto>(o)
                         });


            var totalCount = await query.CountAsync();

            var stockExchanges = await query.ToListAsync();

            return new PagedResultDto<GetStockExchangeForView>(
                totalCount,
                stockExchanges
            );
         }
		 
		 public async Task<GetStockExchangeForEditOutput> GetStockExchangeForEdit(EntityDto input)
         {
            var stockExchange = await _stockExchangeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetStockExchangeForEditOutput {StockExchange = ObjectMapper.Map<CreateOrEditStockExchangeDto>(stockExchange)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditStockExchangeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
		 private async Task Create(CreateOrEditStockExchangeDto input)
         {
            var stockExchange = ObjectMapper.Map<StockExchange>(input);

			
			if (AbpSession.TenantId != null)
			{
				stockExchange.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _stockExchangeRepository.InsertAsync(stockExchange);
         }

		 [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
		 private async Task Update(CreateOrEditStockExchangeDto input)
         {
            var stockExchange = await _stockExchangeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, stockExchange);
         }

		 [AbpAuthorize(AppPermissions.Pages_Requests_Configuration)]
         public async Task Delete(EntityDto input)
         {
            await _stockExchangeRepository.DeleteAsync(input.Id);
         }

		 public async Task<FileDto> GetStockExchangesToExcel(GetAllStockExchangesForExcelInput input)
         {

            var filtered = _stockExchangeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.StockExchangeName.Contains(input.Filter) || e.Country.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StockExchangeNameFilter), e => e.StockExchangeName.ToLower() == input.StockExchangeNameFilter.ToLower().Trim())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryFilter), e => e.Country.ToLower() == input.CountryFilter.ToLower().Trim());


            var query = (from o in filtered

                         select new GetStockExchangeForView()
                         {
                             StockExchange = ObjectMapper.Map<StockExchangeDto>(o)
                         });


            var stockExchangeListDtos = await query.ToListAsync();

            return _stockExchangesExcelExporter.ExportToFile(stockExchangeListDtos);
         }


    }
}