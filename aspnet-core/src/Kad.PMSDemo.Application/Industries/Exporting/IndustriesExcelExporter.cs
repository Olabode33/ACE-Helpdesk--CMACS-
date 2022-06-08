using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Kad.PMSDemo.DataExporting.Excel.NPOI;
using Test.Industries.Dtos;
using Kad.PMSDemo.Dto;
using Kad.PMSDemo.Storage;

namespace Test.Industries.Exporting
{
    public class IndustriesExcelExporter : NpoiExcelExporterBase, IIndustriesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public IndustriesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetIndustryForView> industries)
        {
            return CreateExcelPackage(
                "Industries.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Industries");
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("IndustryName")
                        );

                    AddObjects(
                        sheet, 2, industries,
                        _ => _.Industry.IndustryName
                        );

					

                });
        }
    }
}
