using System.Collections.Generic;
using Test.RequestDomains.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.RequestDomains.Exporting
{
    public interface IRequestDomainsExcelExporter
    {
        FileDto ExportToFile(List<GetRequestDomainForView> requestDomains);
    }
}