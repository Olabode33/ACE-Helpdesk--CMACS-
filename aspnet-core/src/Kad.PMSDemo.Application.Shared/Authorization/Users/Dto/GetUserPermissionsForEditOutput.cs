using System.Collections.Generic;
using Kad.PMSDemo.Authorization.Permissions.Dto;

namespace Kad.PMSDemo.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}