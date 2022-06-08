using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Kad.PMSDemo.DataExporting.Excel.NPOI;
using Test.RequestDocs.Dtos;
using Kad.PMSDemo.Dto;
using Kad.PMSDemo.Storage;

namespace Test.RequestDocs.Exporting
{
    public class RequestDocsExcelExporter : NpoiExcelExporterBase, IRequestDocsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RequestDocsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRequestDocForView> requestDocs)
        {
            return CreateExcelPackage(
                "RequestDocs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("RequestDocs");
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DocumentName"),
                        L("DocumentLocation"),
                        L("PreparerTypeId"),
                        L("DocumentGUID"),
                        (L("Request")) + L("LocalChargeCode"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, requestDocs,
                        _ => _.RequestDoc.DocumentName,
                        _ => _.RequestDoc.DocumentLocation,
                        _ => _.RequestDoc.PreparerTypeId,
                        _ => _.RequestDoc.DocumentGUID,
                        _ => _.RequestLocalChargeCode,
                        _ => _.UserName
                        );

					

                });
        }
    }
}
