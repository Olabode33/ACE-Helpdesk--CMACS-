using System.Collections.Generic;
using Test.TORApprovals.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.TORApprovals.Exporting
{
    public interface ITORApprovalsExcelExporter
    {
        FileDto ExportToFile(List<GetTORApprovalForView> torApprovals);
    }
}