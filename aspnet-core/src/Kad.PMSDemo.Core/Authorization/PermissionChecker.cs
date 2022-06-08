using Abp.Authorization;
using Kad.PMSDemo.Authorization.Roles;
using Kad.PMSDemo.Authorization.Users;

namespace Kad.PMSDemo.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
