using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityProperties;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using Kad.PMSDemo.Auditing.Dto;
using Kad.PMSDemo.Authorization.Accounts.Dto;
using Kad.PMSDemo.Authorization.Delegation;
using Kad.PMSDemo.Authorization.Permissions.Dto;
using Kad.PMSDemo.Authorization.Roles;
using Kad.PMSDemo.Authorization.Roles.Dto;
using Kad.PMSDemo.Authorization.Users;
using Kad.PMSDemo.Authorization.Users.Delegation.Dto;
using Kad.PMSDemo.Authorization.Users.Dto;
using Kad.PMSDemo.Authorization.Users.Importing.Dto;
using Kad.PMSDemo.Authorization.Users.Profile.Dto;
using Kad.PMSDemo.Chat;
using Kad.PMSDemo.Chat.Dto;
using Kad.PMSDemo.DynamicEntityProperties.Dto;
using Kad.PMSDemo.Editions;
using Kad.PMSDemo.Editions.Dto;
using Kad.PMSDemo.Friendships;
using Kad.PMSDemo.Friendships.Cache;
using Kad.PMSDemo.Friendships.Dto;
using Kad.PMSDemo.Localization.Dto;
using Kad.PMSDemo.MultiTenancy;
using Kad.PMSDemo.MultiTenancy.Dto;
using Kad.PMSDemo.MultiTenancy.HostDashboard.Dto;
using Kad.PMSDemo.MultiTenancy.Payments;
using Kad.PMSDemo.MultiTenancy.Payments.Dto;
using Kad.PMSDemo.Notifications.Dto;
using Kad.PMSDemo.Organizations.Dto;
using Kad.PMSDemo.Sessions.Dto;
using Kad.PMSDemo.WebHooks.Dto;
using Test.Requests.Dtos;
using Test.AttachedDocs.Dtos;
using Test.AttachedDocs;
using Test.RequestApprovals.Dtos;
using Test.RequestApprovals;
using Test.TORApprovals.Dtos;
using Test.TORApprovals;
using Test.RequestDocs.Dtos;
using Test.RequestDocs;
using Test.RequestThreads.Dtos;
using Test.RequestThreads;
using Test.TechTeams.Dtos;
using Test.TechTeams;
using Test.Requests.Dtos;
using Test.Requests;
using Test.StockExchanges.Dtos;
using Test.StockExchanges;
using Test.ClientLists.Dtos;
using Test.ClientLists;
using Test.Industries.Dtos;
using Test.Industries;
using Test.ReportingTerritories.Dtos;
using Test.ReportingTerritories;
using Test.RequestAreas.Dtos;
using Test.RequestAreas;
using Test.RequestDomains.Dtos;
using Test.RequestDomains;

namespace Kad.PMSDemo
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditRequestSubAreaMappingDto, RequestSubAreaMapping>().ReverseMap();
            configuration.CreateMap<RequestSubAreaMappingDto, RequestSubAreaMapping>().ReverseMap();
            configuration.CreateMap<CreateOrEditRequestSubAreaDto, RequestSubArea>().ReverseMap();
            configuration.CreateMap<RequestSubAreaDto, RequestSubArea>().ReverseMap();
            configuration.CreateMap<CreateOrEditRequestCmacsManagerApprovalDto, RequestCmacsManagerApproval>().ReverseMap();
            configuration.CreateMap<RequestCmacsManagerApprovalDto, RequestCmacsManagerApproval>().ReverseMap();
            configuration.CreateMap<CreateOrEditAttachedDocDto, AttachedDoc>();
            configuration.CreateMap<AttachedDoc, AttachedDocDto>();
            configuration.CreateMap<CreateOrEditRequestApprovalDto, RequestApproval>();
            configuration.CreateMap<RequestApproval, RequestApprovalDto>();
            configuration.CreateMap<CreateOrEditTORApprovalDto, TORApproval>();
            configuration.CreateMap<TORApproval, TORApprovalDto>();
            configuration.CreateMap<CreateOrEditRequestDocDto, RequestDoc>();
            configuration.CreateMap<RequestDoc, RequestDocDto>();
            configuration.CreateMap<CreateOrEditRequestThreadDto, RequestThread>();
            configuration.CreateMap<RequestThread, RequestThreadDto>();
            configuration.CreateMap<CreateOrEditTechTeamDto, TechTeam>();
            configuration.CreateMap<TechTeam, TechTeamDto>();
            configuration.CreateMap<CreateOrEditRequestDto, Request>().ReverseMap().ForMember(x => x.Attachments, options => options.Ignore());
            configuration.CreateMap<Request, RequestDto>();
            configuration.CreateMap<CreateOrEditStockExchangeDto, StockExchange>();
            configuration.CreateMap<StockExchange, StockExchangeDto>();
            configuration.CreateMap<CreateOrEditClientListDto, ClientList>();
            configuration.CreateMap<ClientList, ClientListDto>();
            configuration.CreateMap<CreateOrEditIndustryDto, Industry>();
            configuration.CreateMap<Industry, IndustryDto>();
            configuration.CreateMap<CreateOrEditReportingTerritoryDto, ReportingTerritory>();
            configuration.CreateMap<ReportingTerritory, ReportingTerritoryDto>();
            configuration.CreateMap<CreateOrEditRequestAreaDto, RequestArea>();
            configuration.CreateMap<RequestArea, RequestAreaDto>();
            configuration.CreateMap<CreateOrEditRequestDomainDto, RequestDomain>();
            configuration.CreateMap<RequestDomain, RequestDomainDto>();

            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicProperty, DynamicPropertyDto>().ReverseMap();
            configuration.CreateMap<DynamicPropertyValue, DynamicPropertyValueDto>().ReverseMap();
            configuration.CreateMap<DynamicEntityProperty, DynamicEntityPropertyDto>()
                .ForMember(dto => dto.DynamicPropertyName,
                    options => options.MapFrom(entity => entity.DynamicProperty.PropertyName));
            configuration.CreateMap<DynamicEntityPropertyDto, DynamicEntityProperty>();

            configuration.CreateMap<DynamicEntityPropertyValue, DynamicEntityPropertyValueDto>().ReverseMap();
            
            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();

            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}
