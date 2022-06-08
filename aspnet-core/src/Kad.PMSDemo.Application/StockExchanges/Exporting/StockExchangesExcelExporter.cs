using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Kad.PMSDemo.DataExporting.Excel.NPOI;
using Test.StockExchanges.Dtos;
using Kad.PMSDemo.Dto;
using Kad.PMSDemo.Storage;

namespace Test.StockExchanges.Exporting
{
    public class StockExchangesExcelExporter : NpoiExcelExporterBase, IStockExchangesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public StockExchangesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetStockExchangeForView> stockExchanges)
        {
            return CreateExcelPackage(
                "StockExchanges.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("StockExchanges"));
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("StockExchangeName"),
                        L("Country")
                        );

                    AddObjects(
                        sheet, 2, stockExchanges,
                        _ => _.StockExchange.StockExchangeName,
                        _ => _.StockExchange.Country
                        );

					

                });
        }
    }
}
