using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Kad.PMSDemo.DataExporting.Excel.NPOI;
using Test.Requests.Dtos;
using Kad.PMSDemo.Dto;
using System.Linq;
using Kad.PMSDemo.Storage;

namespace Test.Requests.Exporting
{
    public class RequestsExcelExporter : NpoiExcelExporterBase, IRequestsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RequestsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<RequestForExcelExport> requests)
        {
            return CreateExcelPackage(
                "Requests.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Requests");
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("SubmissionDate"),
                        L("LocalChargeCode"),
                        L("RequiredResponseDate"),
                        "Reason",
                        L("IssueDiscussedWith"),
                        "Area",
                        "Domain",
                        "Requested By",
                        "Client",
                        "Manager",
                        "Partner",
                        "Status",
                        "Completion Date",
                        "Tech Team",
                        "Request type (Enquiries/ Consultations/ FSRs)",
                        "Applicable standards"
                        );

                    AddObjects(
                        sheet, 2, requests,
                        _ => _timeZoneConverter.Convert(_.Request.Request.SubmissionDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Request.Request.LocalChargeCode,
                        _ => _timeZoneConverter.Convert(_.Request.Request.RequiredResponseDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.Request.Request.ReasonResponseDate,
                        _ => _.Request.Request.IssueDiscussedWith,
                        _ => _.Request.RequestAreaRequestAreaName,
                        _ => _.Request.RequestDomainDomainName,
                        _ => _.Request.RequestorName,
                        _ => _.Request.ClientListClientName,
                        _ => _.Request.ManagerName,
                        _ => _.Request.PartnerName,
                        _ => _.Request.Request.RequestStatusId,
                        _ => _timeZoneConverter.Convert(_.Request.Request.CompletionDate, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => string.Join(", ", _.TechTeam_.Select(x => x.Name)),
                        _ => _.Request.Request.RequestTypeId.ToString(),
                        _ => string.Join(", ", _.SubAreas.Select(x => x.Name))
                        );

                    for (var i = 1; i <= requests.Count; i++)
                    {
                        //Formatting cells
                        SetCellDataFormat(sheet.GetRow(i).Cells[1], "yyyy-mm-dd");
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                        SetCellDataFormat(sheet.GetRow(i).Cells[13], "yyyy-mm-dd");
                    }

                    for(int i = 1; i < 17; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }

                });
        }
    }
}
