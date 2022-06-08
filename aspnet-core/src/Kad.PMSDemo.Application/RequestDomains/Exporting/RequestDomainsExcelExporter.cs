using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Kad.PMSDemo.DataExporting.Excel.NPOI;
using Test.RequestDomains.Dtos;
using Kad.PMSDemo.Dto;
using Kad.PMSDemo.Storage;

namespace Test.RequestDomains.Exporting
{
    public class RequestDomainsExcelExporter : NpoiExcelExporterBase, IRequestDomainsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public RequestDomainsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetRequestDomainForView> requestDomains)
        {
            return CreateExcelPackage(
                "RequestDomains.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("RequestDomains");
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("DomainName")
                        );

                    AddObjects(
                        sheet, 2, requestDomains,
                        _ => _.RequestDomain.DomainName
                        );

					

                });
        }
    }
}
