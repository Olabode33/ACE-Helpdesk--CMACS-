using System.Collections.Generic;
using Test.ClientLists.Dtos;
using Kad.PMSDemo.Dto;

namespace Test.ClientLists.Exporting
{
    public interface IClientListsExcelExporter
    {
        FileDto ExportToFile(List<GetClientListForView> clientLists);
    }
}