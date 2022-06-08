using System.Collections.Generic;
using Abp;
using Kad.PMSDemo.Chat.Dto;
using Kad.PMSDemo.Dto;

namespace Kad.PMSDemo.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
