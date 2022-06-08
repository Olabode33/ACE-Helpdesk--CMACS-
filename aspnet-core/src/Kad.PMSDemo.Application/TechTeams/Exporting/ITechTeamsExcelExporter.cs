using System.Collections.Generic;
using Test.TechTeams.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.TechTeams.Exporting
{
    public interface ITechTeamsExcelExporter
    {
        FileDto ExportToFile(List<GetTechTeamForView> techTeams);
    }
}