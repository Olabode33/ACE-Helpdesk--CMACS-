using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Kad.PMSDemo.DataExporting.Excel.NPOI;
using Test.ReportingTerritories.Dtos;
using Kad.PMSDemo.Dto;
using Kad.PMSDemo.Storage;

namespace Test.ReportingTerritories.Exporting
{
    public class ReportingTerritoriesExcelExporter : NpoiExcelExporterBase, IReportingTerritoriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ReportingTerritoriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetReportingTerritoryForView> reportingTerritories)
        {
            return CreateExcelPackage(
                "ReportingTerritories.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ReportingTerritories");
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TerritoryName")
                        );

                    AddObjects(
                        sheet, 2, reportingTerritories,
                        _ => _.ReportingTerritory.TerritoryName
                        );

					

                });
        }
    }
}
