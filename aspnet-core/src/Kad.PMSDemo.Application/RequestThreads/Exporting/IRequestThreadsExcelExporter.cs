using System.Collections.Generic;
using Test.RequestThreads.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.RequestThreads.Exporting
{
    public interface IRequestThreadsExcelExporter
    {
        FileDto ExportToFile(List<GetRequestThreadForView> requestThreads);
    }
}