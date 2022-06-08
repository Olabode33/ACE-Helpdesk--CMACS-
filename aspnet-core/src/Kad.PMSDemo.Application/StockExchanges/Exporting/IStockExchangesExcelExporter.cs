using System.Collections.Generic;
using Test.StockExchanges.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.StockExchanges.Exporting
{
    public interface IStockExchangesExcelExporter
    {
        FileDto ExportToFile(List<GetStockExchangeForView> stockExchanges);
    }
}