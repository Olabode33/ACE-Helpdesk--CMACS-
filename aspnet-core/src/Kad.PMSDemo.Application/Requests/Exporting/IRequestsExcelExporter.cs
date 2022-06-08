using System.Collections.Generic;
using Test.Requests.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.Requests.Exporting
{
    public interface IRequestsExcelExporter
    {
        FileDto ExportToFile(List<RequestForExcelExport> requests);
    }
}