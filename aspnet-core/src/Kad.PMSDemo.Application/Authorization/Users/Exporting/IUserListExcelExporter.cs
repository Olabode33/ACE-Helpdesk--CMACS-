using System.Collections.Generic;
using Kad.PMSDemo.Authorization.Users.Dto;
using Kad.PMSDemo.Dto;

namespace Kad.PMSDemo.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos);
    }
}