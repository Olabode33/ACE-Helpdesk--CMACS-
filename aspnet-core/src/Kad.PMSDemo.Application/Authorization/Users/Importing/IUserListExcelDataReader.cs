using System.Collections.Generic;
using Kad.PMSDemo.Authorization.Users.Importing.Dto;
using Abp.Dependency;

namespace Kad.PMSDemo.Authorization.Users.Importing
{
    public interface IUserListExcelDataReader: ITransientDependency
    {
        List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes);
    }
}
