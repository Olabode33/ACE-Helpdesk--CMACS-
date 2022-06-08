using System.Collections.Generic;
using Test.ReportingTerritories.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.ReportingTerritories.Exporting
{
    public interface IReportingTerritoriesExcelExporter
    {
        FileDto ExportToFile(List<GetReportingTerritoryForView> reportingTerritories);
    }
}