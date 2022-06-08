using Abp.IdentityServer4;
using Abp.Organizations;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Kad.PMSDemo.Authorization.Delegation;
using Kad.PMSDemo.Authorization.Roles;
using Kad.PMSDemo.Authorization.Users;
using Kad.PMSDemo.Chat;
using Kad.PMSDemo.Editions;
using Kad.PMSDemo.Friendships;
using Kad.PMSDemo.MultiTenancy;
using Kad.PMSDemo.MultiTenancy.Accounting;
using Kad.PMSDemo.MultiTenancy.Payments;
using Kad.PMSDemo.Storage;
using Test.PwcReferenceEntity;
using Test.Requests;
using Test.AttachedDocs;
using Test.RequestApprovals;
using Test.TORApprovals;
using Test.RequestDocs;
using Test.RequestThreads;
using Test.TechTeams;
using Test.Requests;
using Test.StockExchanges;
using Test.ClientLists;
using Test.Industries;
using Test.ReportingTerritories;
using Test.RequestAreas;
using Test.RequestDomains;

namespace Kad.PMSDemo.EntityFrameworkCore
{
    public class PMSDemoDbContext : AbpZeroDbContext<Tenant, Role, User, PMSDemoDbContext>, IAbpPersistedGrantDbContext
    {

        public virtual DbSet<BosResource> BosResources { get; set; }
        public virtual DbSet<BosCustomer> BosCustomers { get; set; }
        public virtual DbSet<BosProject> BosProjects { get; set; }
        public virtual DbSet<RequestSubAreaMapping> RequestSubAreaMappings { get; set; }

        public virtual DbSet<RequestSubArea> RequestSubAreas { get; set; }

        public virtual DbSet<RequestCmacsManagerApproval> RequestCmacsManagerApprovals { get; set; }

        public virtual DbSet<AttachedDoc> AttachedDocs { get; set; }

        public virtual DbSet<RequestApproval> RequestApprovals { get; set; }

        public virtual DbSet<TORApproval> TORApprovals { get; set; }

        public virtual DbSet<RequestDoc> RequestDocs { get; set; }

        public virtual DbSet<RequestThread> RequestThreads { get; set; }

        public virtual DbSet<TechTeam> TechTeams { get; set; }

        public virtual DbSet<Request> Requests { get; set; }

        public virtual DbSet<StockExchange> StockExchanges { get; set; }

        public virtual DbSet<ClientList> ClientLists { get; set; }

        public virtual DbSet<Industry> Industries { get; set; }

        public virtual DbSet<ReportingTerritory> ReportingTerritories { get; set; }

        public virtual DbSet<RequestArea> RequestAreas { get; set; }

        public virtual DbSet<RequestDomain> RequestDomains { get; set; }


        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public PMSDemoDbContext(DbContextOptions<PMSDemoDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
