using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using Kad.PMSDemo.Authorization.Users;
using Kad.PMSDemo.MultiTenancy;

namespace Kad.PMSDemo.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}