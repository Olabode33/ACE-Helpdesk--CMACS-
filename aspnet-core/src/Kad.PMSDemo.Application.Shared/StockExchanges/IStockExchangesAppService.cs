using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Test.StockExchanges.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.StockExchanges
{
    public interface IStockExchangesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetStockExchangeForView>> GetAll(GetAllStockExchangesInput input);

		Task<GetStockExchangeForEditOutput> GetStockExchangeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditStockExchangeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetStockExchangesToExcel(GetAllStockExchangesForExcelInput input);

		
    }
}