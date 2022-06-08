using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Kad.PMSDemo.DataExporting.Excel.NPOI;
using Test.TechTeams.Dtos;
using Kad.PMSDemo.Dto;
using Kad.PMSDemo.Storage;

namespace Test.TechTeams.Exporting
{
    public class TechTeamsExcelExporter : NpoiExcelExporterBase, ITechTeamsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TechTeamsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager)
            : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTechTeamForView> techTeams)
        {
            return CreateExcelPackage(
                "TechTeams.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("TechTeams"));
                    //sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("TimeCharge"),
                        L("Role"),
                        (L("Request")) + L("LocalChargeCode"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, techTeams,
                        _ => _.TechTeam.TimeCharge,
                        _ => _.TechTeam.Role,
                        _ => _.RequestLocalChargeCode,
                        _ => _.UserName
                        );

					

                });
        }
    }
}
