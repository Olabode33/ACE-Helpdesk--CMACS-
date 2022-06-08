using System.Collections.Generic;
using Kad.PMSDemo.Authorization.Users.Importing.Dto;
using Kad.PMSDemo.Dto;

namespace Kad.PMSDemo.Authorization.Users.Importing
{
    public interface IInvalidUserExporter
    {
        FileDto ExportToFile(List<ImportUserDto> userListDtos);
    }
}
