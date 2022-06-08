using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Kad.PMSDemo.DataExporting.Excel.NPOI;
using Test.AttachedDocs.Dtos;
using Kad.PMSDemo.Dto;
using Kad.PMSDemo.Storage;

namespace Test.AttachedDocs.Exporting
{
    public class AttachedDocsExcelExporter : NpoiExcelExporterBase, IAttachedDocsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public AttachedDocsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetAttachedDocForView> attachedDocs)
        {
            return CreateExcelPackage(
                "AttachedDocs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("AttachedDocs");
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("FileName"),
                        L("DocOwnerTypeId"),
                        (L("Request")) + L("RequestNo"),
                        (L("User")) + L("Name"),
                        (L("BinaryObject")) + L("TenantId")
                        );

                    AddObjects(
                        sheet, 2, attachedDocs,
                        _ => _.AttachedDoc.FileName,
                        _ => _.AttachedDoc.DocOwnerTypeId,
                        _ => _.RequestRequestNo,
                        _ => _.UserName,
                        _ => _.BinaryObjectTenantId
                        );

					

                });
        }
    }
}
