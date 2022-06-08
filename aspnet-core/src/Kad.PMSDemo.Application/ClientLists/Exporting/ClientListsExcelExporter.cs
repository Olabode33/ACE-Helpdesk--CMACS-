using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Kad.PMSDemo.DataExporting.Excel.NPOI;
using Test.ClientLists.Dtos;
using Kad.PMSDemo.Dto;
using Kad.PMSDemo.Storage;

namespace Test.ClientLists.Exporting
{
    public class ClientListsExcelExporter : NpoiExcelExporterBase, IClientListsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ClientListsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetClientListForView> clientLists)
        {
            return CreateExcelPackage(
                "ClientLists.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ClientLists");
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("ClientName"),
                        L("ClientAddress"),
                        L("ParentEntity"),
                        L("UltimateParentEntity"),
                        L("SecRegistered"),
                        L("FinancialYearEnd"),
                        L("ChannelTypeName"),
                        (L("Industry")) + L("IndustryName"),
                        (L("ReportingTerritory")) + L("TerritoryName"),
                        (L("StockExchange")) + L("StockExchangeName")
                        );

                    AddObjects(
                        sheet, 2, clientLists,
                        _ => _.ClientList.ClientName,
                        _ => _.ClientList.ClientAddress,
                        _ => _.ClientList.ParentEntity,
                        _ => _.ClientList.UltimateParentEntity,
                        _ => _.ClientList.SecRegistered,
                        _ => _.ClientList.FinancialYearEnd,
                        _ => _.ClientList.ChannelTypeName,
                        _ => _.IndustryIndustryName,
                        _ => _.ReportingTerritoryTerritoryName,
                        _ => _.StockExchangeStockExchangeName
                        );

					

                });
        }
    }
}
