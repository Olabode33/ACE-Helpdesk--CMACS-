using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Kad.PMSDemo.DataExporting.Excel.NPOI;
using Test.RequestThreads.Dtos;
using Kad.PMSDemo.Dto;
using Kad.PMSDemo.Storage;

namespace Test.RequestThreads.Exporting
{
    public class RequestThreadsExcelExporter : NpoiExcelExporterBase, IRequestThreadsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RequestThreadsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRequestThreadForView> requestThreads)
        {
            return CreateExcelPackage(
                "RequestThreads.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("RequestThreads");
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("Comment"),
                        L("CommentDate"),
                        (L("Request")) + L("LocalChargeCode"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, requestThreads,
                        _ => _.RequestThread.Comment,
                        _ => _timeZoneConverter.Convert(_.RequestThread.CommentDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.RequestLocalChargeCode,
                        _ => _.UserName
                        );

					//var commentDateColumn = sheet.Column(2);
     //               commentDateColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					//commentDateColumn.AutoFit();
					

                });
        }
    }
}
