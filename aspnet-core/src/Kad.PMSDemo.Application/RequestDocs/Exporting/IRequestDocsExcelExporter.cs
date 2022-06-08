using System.Collections.Generic;
using Test.RequestDocs.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.RequestDocs.Exporting
{
    public interface IRequestDocsExcelExporter
    {
        FileDto ExportToFile(List<GetRequestDocForView> requestDocs);
    }
}