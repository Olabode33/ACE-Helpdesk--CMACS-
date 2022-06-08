using System.Collections.Generic;
using Test.Industries.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.Industries.Exporting
{
    public interface IIndustriesExcelExporter
    {
        FileDto ExportToFile(List<GetIndustryForView> industries);
    }
}