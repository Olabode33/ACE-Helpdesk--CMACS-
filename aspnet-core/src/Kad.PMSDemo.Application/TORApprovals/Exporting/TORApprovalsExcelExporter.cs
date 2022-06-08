using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Kad.PMSDemo.DataExporting.Excel.NPOI;
using Test.TORApprovals.Dtos;
using Kad.PMSDemo.Dto;
using Kad.PMSDemo.Storage;

namespace Test.TORApprovals.Exporting
{
    public class TORApprovalsExcelExporter : NpoiExcelExporterBase, ITORApprovalsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        //       public TORApprovalsExcelExporter(
        //           ITimeZoneConverter timeZoneConverter,
        //           IAbpSession abpSession,
        //		ITempFileCacheManager tempFileCacheManager) :  
        //base(tempFileCacheManager)
        //       {
        //           _timeZoneConverter = timeZoneConverter;
        //           _abpSession = abpSession;
        //       }

        public TORApprovalsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTORApprovalForView> torApprovals)
        {
            return CreateExcelPackage(
                "TORApprovals.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("TORApprovals"));
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TORTimeSent"),
                        L("Approved"),
                        L("ApprovedTime"),
                        (L("User")) + L("Name"),
                        (L("Request")) + L("RequestNo")
                        );

                    AddObjects(
                        sheet, 2, torApprovals,
                        _ => _timeZoneConverter.Convert(_.TORApproval.TORTimeSent, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.TORApproval.Approved,
                        _ => _timeZoneConverter.Convert(_.TORApproval.ApprovedTime, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.UserName,
                        _ => _.RequestRequestNo
                        );

                    //for (var i = 1; i <= userListDtos.Count; i++)
                    //{
                    //    //Formatting cells
                    //    SetCellDataFormat(sheet.GetRow(i).Cells[8], "yyyy-mm-dd");
                    //}

                    //var torTimeSentColumn = sheet.(1);
                    //torTimeSentColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    //torTimeSentColumn.AutoFit();
                    //var approvedTimeColumn = sheet.Column(3);
                    //approvedTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                    //approvedTimeColumn.AutoFit();


                });
        }
    }
}
