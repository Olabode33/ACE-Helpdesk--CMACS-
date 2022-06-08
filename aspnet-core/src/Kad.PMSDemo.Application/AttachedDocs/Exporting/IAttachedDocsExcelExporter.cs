using System.Collections.Generic;
using Test.AttachedDocs.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.AttachedDocs.Exporting
{
    public interface IAttachedDocsExcelExporter
    {
        FileDto ExportToFile(List<GetAttachedDocForView> attachedDocs);
    }
}