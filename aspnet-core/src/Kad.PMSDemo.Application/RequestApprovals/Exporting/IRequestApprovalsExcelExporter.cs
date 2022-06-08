using System.Collections.Generic;
using Test.RequestApprovals.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.RequestApprovals.Exporting
{
    public interface IRequestApprovalsExcelExporter
    {
        FileDto ExportToFile(List<GetRequestApprovalForView> requestApprovals);
    }
}