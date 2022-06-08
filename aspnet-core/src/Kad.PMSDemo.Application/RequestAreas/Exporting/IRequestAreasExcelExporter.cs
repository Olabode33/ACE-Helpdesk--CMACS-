using System.Collections.Generic;
using Test.RequestAreas.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.RequestAreas.Exporting
{
    public interface IRequestAreasExcelExporter
    {
        FileDto ExportToFile(List<GetRequestAreaForView> requestAreas);
    }
}