using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Kad.PMSDemo.DataExporting.Excel.NPOI;
using Test.RequestAreas.Dtos;
using Kad.PMSDemo.Dto;
using Kad.PMSDemo.Storage;

namespace Test.RequestAreas.Exporting
{
    public class RequestAreasExcelExporter : NpoiExcelExporterBase, IRequestAreasExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RequestAreasExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRequestAreaForView> requestAreas)
        {
            return CreateExcelPackage(
                "RequestAreas.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("RequestAreas");
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("RequestAreaName")
                        );

                    AddObjects(
                        sheet, 2, requestAreas,
                        _ => _.RequestArea.RequestAreaName
                        );

					

                });
        }
    }
}
