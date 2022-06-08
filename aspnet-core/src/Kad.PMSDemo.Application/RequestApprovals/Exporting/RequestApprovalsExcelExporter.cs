using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Kad.PMSDemo.DataExporting.Excel.NPOI;
using Test.RequestApprovals.Dtos;
using Kad.PMSDemo.Dto;
using Kad.PMSDemo.Storage;

namespace Test.RequestApprovals.Exporting
{
    public class RequestApprovalsExcelExporter : NpoiExcelExporterBase, IRequestApprovalsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RequestApprovalsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRequestApprovalForView> requestApprovals)
        {
            return CreateExcelPackage(
                "RequestApprovals.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("RequestApprovals");
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("ApprovalSentTime"),
                        L("Approved"),
                        L("ApprovedTime"),
                        (L("Request")) + L("RequestNo"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, requestApprovals,
                        _ => _timeZoneConverter.Convert(_.RequestApproval.ApprovalSentTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.RequestApproval.Approved,
                        _ => _timeZoneConverter.Convert(_.RequestApproval.ApprovedTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.RequestRequestNo,
                        _ => _.UserName
                        );

					//var approvalSentTimeColumn = sheet.Column(1);
     //               approvalSentTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					//approvalSentTimeColumn.AutoFit();
					//var approvedTimeColumn = sheet.Column(3);
     //               approvedTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";
					//approvedTimeColumn.AutoFit();
					

                });
        }
    }
}
