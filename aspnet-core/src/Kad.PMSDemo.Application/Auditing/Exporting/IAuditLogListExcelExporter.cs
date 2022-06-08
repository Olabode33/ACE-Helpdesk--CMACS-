using System.Collections.Generic;
using Kad.PMSDemo.Auditing.Dto;
using Kad.PMSDemo.Dto;

namespace Kad.PMSDemo.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
