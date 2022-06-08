using System.Linq;
using Abp;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;
using Abp.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Kad.PMSDemo.Authorization;
using Kad.PMSDemo.Authorization.Roles;
using Kad.PMSDemo.Authorization.Users;
using Kad.PMSDemo.EntityFrameworkCore;
using Kad.PMSDemo.Notifications;

namespace Kad.PMSDemo.Migrations.Seed.Tenants
{
    public class TenantRoleAndUserBuilder
    {
        private readonly PMSDemoDbContext _context;
        private readonly int _tenantId;

        public TenantRoleAndUserBuilder(PMSDemoDbContext context, int tenantId)
        {
            _context = context;
            _tenantId = tenantId;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            //Admin role

            var adminRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.Admin);
            if (adminRole == null)
            {
                adminRole = _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.Admin, StaticRoleNames.Tenants.Admin) { IsStatic = true }).Entity;
                _context.SaveChanges();
            }

            //User role

            var userRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.User);
            if (userRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.User, StaticRoleNames.Tenants.User) { IsStatic = true, IsDefault = true });
                _context.SaveChanges();
            }

            //admin user

            var adminUser = _context.Users.IgnoreQueryFilters().FirstOrDefault(u => u.TenantId == _tenantId && u.UserName == AbpUserBase.AdminUserName);
            if (adminUser == null)
            {
                adminUser = User.CreateTenantAdminUser(_tenantId, "admin@defaulttenant.com");
                adminUser.Password = new PasswordHasher<User>(new OptionsWrapper<PasswordHasherOptions>(new PasswordHasherOptions())).HashPassword(adminUser, "123qwe");
                adminUser.IsEmailConfirmed = true;
                adminUser.ShouldChangePasswordOnNextLogin = false;
                adminUser.IsActive = true;

                _context.Users.Add(adminUser);
                _context.SaveChanges();

                //Assign Admin role to admin user
                _context.UserRoles.Add(new UserRole(_tenantId, adminUser.Id, adminRole.Id));
                _context.SaveChanges();

                //User account of admin user
                if (_tenantId == 1)
                {
                    _context.UserAccounts.Add(new UserAccount
                    {
                        TenantId = _tenantId,
                        UserId = adminUser.Id,
                        UserName = AbpUserBase.AdminUserName,
                        EmailAddress = adminUser.EmailAddress
                    });
                    _context.SaveChanges();
                }

                //Notification subscription
                _context.NotificationSubscriptions.Add(new NotificationSubscriptionInfo(SequentialGuidGenerator.Instance.Create(), _tenantId, adminUser.Id, AppNotificationNames.NewUserRegistered));
                _context.SaveChanges();
            }

            //IFRS Helpdesk role

            var ifrsHelpdeskRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.IFRS_Helpdesk);
            if (ifrsHelpdeskRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.IFRS_Helpdesk, StaticRoleNames.Tenants.IFRS_Helpdesk) { IsStatic = true, IsDefault = false });
                _context.SaveChanges();
            }

            //Manager & Partner role

            var managerPartnerRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.ManagersPartners);
            if (managerPartnerRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.ManagersPartners, StaticRoleNames.Tenants.ManagersPartners) { IsStatic = true, IsDefault = false });
                _context.SaveChanges();
            }

            //Requesting Team role

            var requestingTeamRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.RequestingTeam);
            if (requestingTeamRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.RequestingTeam, StaticRoleNames.Tenants.RequestingTeam) { IsStatic = true, IsDefault = true });
                _context.SaveChanges();
            }

            //Technical Team role

            var techTeamRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.TechnicalTeam);
            if (techTeamRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.TechnicalTeam, StaticRoleNames.Tenants.TechnicalTeam) { IsStatic = true, IsDefault = false });
                _context.SaveChanges();
            }

            //Final Approval role

            var finalApproverRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.FinalApprover);
            if (finalApproverRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.FinalApprover, StaticRoleNames.Tenants.FinalApprover) { IsStatic = true, IsDefault = false });
                _context.SaveChanges();
            }

            //User role

            var cmacsManagerRole = _context.Roles.IgnoreQueryFilters().FirstOrDefault(r => r.TenantId == _tenantId && r.Name == StaticRoleNames.Tenants.CMACsManager);
            if (cmacsManagerRole == null)
            {
                _context.Roles.Add(new Role(_tenantId, StaticRoleNames.Tenants.CMACsManager, StaticRoleNames.Tenants.CMACsManager) { IsStatic = true, IsDefault = false });
                _context.SaveChanges();
            }
        }
    }
}
