using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.IdentityFramework;
using Abp.Linq;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Kad.PMSDemo.Authorization.Roles;
using Kad.PMSDemo.Configuration;
using Kad.PMSDemo.Debugging;
using Kad.PMSDemo.MultiTenancy;
using Kad.PMSDemo.Notifications;
using Abp.Domain.Repositories;
using Test.PwcReferenceEntity;
using System.Collections.ObjectModel;

namespace Kad.PMSDemo.Authorization.Users
{
    public class UserRegistrationManager : PMSDemoDomainServiceBase
    {
        public IAbpSession AbpSession { get; set; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IUserEmailer _userEmailer;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IUserPolicy _userPolicy;
        private readonly IRepository<BosResource> _pwcStaffRepository;


        public UserRegistrationManager(
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<BosResource> pwcStaffRepository,
            IUserEmailer userEmailer,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IAppNotifier appNotifier,
            IUserPolicy userPolicy)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _pwcStaffRepository = pwcStaffRepository;
            _userEmailer = userEmailer;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _userPolicy = userPolicy;

            AbpSession = NullAbpSession.Instance;
            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        public async Task<User> RegisterAsync(string name, string surname, string emailAddress, string userName, string plainPassword, bool isEmailConfirmed, string emailActivationLink)
        {
            var existingUser = await _userManager.FindByNameOrEmailAsync(emailAddress);
            if (existingUser != null)
            {
                return existingUser;
            }

            CheckForTenant();
            CheckSelfRegistrationIsEnabled();

            var tenant = await GetActiveTenantAsync();
            var isNewRegisteredUserActiveByDefault = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.IsNewRegisteredUserActiveByDefault);

            await _userPolicy.CheckMaxUserCountAsync(tenant.Id);

            var user = new User
            {
                TenantId = tenant.Id,
                Name = name,
                Surname = surname,
                EmailAddress = emailAddress,
                IsActive = isNewRegisteredUserActiveByDefault,
                UserName = userName,
                IsEmailConfirmed = isEmailConfirmed,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            var defaultRoles = await AsyncQueryableExecuter.ToListAsync(_roleManager.Roles.Where(r => r.IsDefault));
            foreach (var defaultRole in defaultRoles)
            {
                user.Roles.Add(new UserRole(tenant.Id, user.Id, defaultRole.Id));
            }

            var pwcStaff = _pwcStaffRepository.FirstOrDefault(x => x.EmailID == emailAddress);
            if (pwcStaff != null)
            {
                var allRoles = await AsyncQueryableExecuter.ToListAsync(_roleManager.Roles);
                user.Roles = new List<UserRole>();

                if ((pwcStaff.Designation == "Manager" || pwcStaff.Designation == "Senior Manager" || pwcStaff.Designation == "Director" || pwcStaff.Designation == "Partner") && pwcStaff.Costcenter != "ASS - CMAAS")
                {
                    var managerPartnerRole = await _roleManager.FindByNameAsync(StaticRoleNames.Tenants.ManagersPartners);
                    user.Roles.Add(new UserRole(tenant.Id, user.Id, managerPartnerRole.Id));
                }

                if ((pwcStaff.Designation == "Manager" || pwcStaff.Designation == "Senior Manager") && pwcStaff.Costcenter == "ASS - CMAAS")
                {
                    var cmacsManagerRole = await _roleManager.FindByNameAsync(StaticRoleNames.Tenants.CMACsManager);
                    user.Roles.Add(new UserRole(tenant.Id, user.Id, cmacsManagerRole.Id));
                }

                if ((pwcStaff.Designation == "Director" || pwcStaff.Designation == "Partner") && pwcStaff.Costcenter == "ASS - CMAAS")
                {
                    var finalApprovalRole = await _roleManager.FindByNameAsync(StaticRoleNames.Tenants.FinalApprover);
                    user.Roles.Add(new UserRole(tenant.Id, user.Id, finalApprovalRole.Id));
                }

                if (pwcStaff.Costcenter == "ASS - CMAAS")
                {
                    var techTeamRole = await _roleManager.FindByNameAsync(StaticRoleNames.Tenants.TechnicalTeam);
                    user.Roles.Add(new UserRole(tenant.Id, user.Id, techTeamRole.Id));
                }

                if (pwcStaff.Costcenter != "ASS - CMAAS")
                {
                    var requestTeamRole = await _roleManager.FindByNameAsync(StaticRoleNames.Tenants.RequestingTeam);
                    user.Roles.Add(new UserRole(tenant.Id, user.Id, requestTeamRole.Id));
                }
            }

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);
            CheckErrors(await _userManager.CreateAsync(user, plainPassword));
            await CurrentUnitOfWork.SaveChangesAsync();

            if (!user.IsEmailConfirmed)
            {
                user.SetNewEmailConfirmationCode();
                await _userEmailer.SendEmailActivationLinkAsync(user, emailActivationLink);
            }

            //Notifications
            await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
            await _appNotifier.WelcomeToTheApplicationAsync(user);
            await _appNotifier.NewUserRegisteredAsync(user);

            return user;
        }

        private void CheckForTenant()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                throw new InvalidOperationException("Can not register host users!");
            }
        }

        private void CheckSelfRegistrationIsEnabled()
        {
            if (!SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.AllowSelfRegistration))
            {
                throw new UserFriendlyException(L("SelfUserRegistrationIsDisabledMessage_Detail"));
            }
        }

        private bool UseCaptchaOnRegistration()
        {
            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnRegistration);
        }

        private async Task<Tenant> GetActiveTenantAsync()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return await GetActiveTenantAsync(AbpSession.TenantId.Value);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await _tenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
