using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kad.PMSDemo.Migrations
{
    public partial class Added_Entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BOS_Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientCode = table.Column<string>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    UpdateDateTime = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    Address3 = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOS_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BOS_Projects",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCode = table.Column<string>(nullable: true),
                    JobDescription = table.Column<string>(nullable: true),
                    DepartmentCode = table.Column<string>(nullable: true),
                    DepartmentName = table.Column<string>(nullable: true),
                    UpdateDateTime = table.Column<string>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOS_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BOS_Resources",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResourceID = table.Column<string>(nullable: true),
                    EmployeeID = table.Column<string>(nullable: true),
                    LegacyResourceID = table.Column<string>(nullable: true),
                    ResourceFirstName = table.Column<string>(nullable: true),
                    ResourceMiddleName = table.Column<string>(nullable: true),
                    ResourceLastName = table.Column<string>(nullable: true),
                    Designation = table.Column<string>(nullable: true),
                    CostCenterCode = table.Column<string>(nullable: true),
                    Costcenter = table.Column<string>(nullable: true),
                    EmailID = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    InternalBUDescription = table.Column<string>(nullable: true),
                    LoginID = table.Column<string>(nullable: true),
                    UpdateDateTime = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOS_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Industries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    IndustryName = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Industries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportingTerritories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    TerritoryName = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportingTerritories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestAreas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    RequestAreaName = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestDomains",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    DomainName = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestDomains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockExchanges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    StockExchangeName = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockExchanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestSubAreas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    RequestAreaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestSubAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestSubAreas_RequestAreas_RequestAreaId",
                        column: x => x.RequestAreaId,
                        principalTable: "RequestAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientLists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    ClientName = table.Column<string>(maxLength: 250, nullable: true),
                    ClientAddress = table.Column<string>(maxLength: 500, nullable: true),
                    ParentEntity = table.Column<string>(maxLength: 200, nullable: true),
                    UltimateParentEntity = table.Column<string>(maxLength: 200, nullable: true),
                    SecRegistered = table.Column<bool>(nullable: false),
                    FinancialYearEnd = table.Column<int>(nullable: false),
                    ChannelTypeName = table.Column<int>(nullable: false),
                    IndustryId = table.Column<int>(nullable: true),
                    ReportingTerritoryId = table.Column<int>(nullable: true),
                    StockExchangeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientLists_Industries_IndustryId",
                        column: x => x.IndustryId,
                        principalTable: "Industries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientLists_ReportingTerritories_ReportingTerritoryId",
                        column: x => x.ReportingTerritoryId,
                        principalTable: "ReportingTerritories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientLists_StockExchanges_StockExchangeId",
                        column: x => x.StockExchangeId,
                        principalTable: "StockExchanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    LocalSubCode = table.Column<string>(maxLength: 20, nullable: true),
                    LocalChargeCode = table.Column<string>(maxLength: 20, nullable: true),
                    SubmissionDate = table.Column<DateTime>(nullable: false),
                    RequiredResponseDate = table.Column<DateTime>(nullable: false),
                    ReasonResponseDate = table.Column<string>(nullable: true),
                    IssueDiscussed = table.Column<bool>(nullable: false),
                    IssueDiscussedWith = table.Column<string>(nullable: true),
                    OOTReviewer = table.Column<string>(maxLength: 250, nullable: true),
                    OOTReviewerTime = table.Column<int>(nullable: false),
                    ConsultationIssue = table.Column<string>(nullable: true),
                    Background = table.Column<string>(nullable: true),
                    TechReference = table.Column<string>(nullable: true),
                    AgreedGuidance = table.Column<string>(nullable: true),
                    TechGrpResponse = table.Column<string>(nullable: true),
                    CompletionDate = table.Column<DateTime>(nullable: true),
                    RequestStatusId = table.Column<int>(nullable: false),
                    RequestAreaId = table.Column<int>(nullable: true),
                    RequestDomainId = table.Column<int>(nullable: true),
                    RequestorId = table.Column<long>(nullable: true),
                    RequestorPartnerId = table.Column<long>(nullable: true),
                    RequestorManagerId = table.Column<long>(nullable: true),
                    ClientListId = table.Column<int>(nullable: true),
                    AssigneeId = table.Column<long>(nullable: true),
                    RequestTypeId = table.Column<int>(nullable: false),
                    Enquiry = table.Column<string>(nullable: true),
                    EnquiryResponse = table.Column<string>(nullable: true),
                    RequestNo = table.Column<string>(maxLength: 20, nullable: true),
                    TermsOfRef = table.Column<string>(nullable: true),
                    TermsOfRefApproved = table.Column<bool>(nullable: false),
                    TORSentDate = table.Column<DateTime>(nullable: true),
                    TORApprovedDate = table.Column<DateTime>(nullable: true),
                    RequestSentDate = table.Column<DateTime>(nullable: true),
                    RequestApprovedDate = table.Column<DateTime>(nullable: true),
                    TechConclusion = table.Column<string>(nullable: true),
                    OtherConsideration = table.Column<string>(nullable: true),
                    RequestApproved = table.Column<bool>(nullable: false),
                    HasSignedTOR = table.Column<bool>(nullable: false),
                    ReturnComment = table.Column<string>(nullable: true),
                    VoluntryRequiredTor = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_AbpUsers_AssigneeId",
                        column: x => x.AssigneeId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_ClientLists_ClientListId",
                        column: x => x.ClientListId,
                        principalTable: "ClientLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_RequestAreas_RequestAreaId",
                        column: x => x.RequestAreaId,
                        principalTable: "RequestAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_RequestDomains_RequestDomainId",
                        column: x => x.RequestDomainId,
                        principalTable: "RequestDomains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_AbpUsers_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_AbpUsers_RequestorManagerId",
                        column: x => x.RequestorManagerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_AbpUsers_RequestorPartnerId",
                        column: x => x.RequestorPartnerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AttachedDocs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    DocOwnerTypeId = table.Column<int>(nullable: false),
                    RequestId = table.Column<int>(nullable: true),
                    DocOwnerId = table.Column<long>(nullable: true),
                    DocumentId = table.Column<Guid>(nullable: true),
                    AttachmentType = table.Column<int>(nullable: false),
                    FileFormat = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachedDocs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttachedDocs_AbpUsers_DocOwnerId",
                        column: x => x.DocOwnerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttachedDocs_AppBinaryObjects_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "AppBinaryObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AttachedDocs_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    ApprovalSentTime = table.Column<DateTime>(nullable: true),
                    Approved = table.Column<bool>(nullable: false),
                    ApprovedTime = table.Column<DateTime>(nullable: true),
                    RequestId = table.Column<int>(nullable: true),
                    ApproverId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestApprovals_AbpUsers_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestApprovals_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestCmacsManagerApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Approved = table.Column<bool>(nullable: false),
                    ApprovedTime = table.Column<DateTime>(nullable: true),
                    RequestId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestCmacsManagerApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestCmacsManagerApprovals_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestCmacsManagerApprovals_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestDocs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    DocumentName = table.Column<string>(maxLength: 100, nullable: true),
                    DocumentLocation = table.Column<string>(maxLength: 200, nullable: true),
                    PreparerTypeId = table.Column<int>(nullable: false),
                    DocumentGUID = table.Column<Guid>(nullable: false),
                    RequestId = table.Column<int>(nullable: true),
                    PreparerId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestDocs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestDocs_AbpUsers_PreparerId",
                        column: x => x.PreparerId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestDocs_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestSubAreaMappings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestId = table.Column<int>(nullable: false),
                    RequestSubAreaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestSubAreaMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestSubAreaMappings_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestSubAreaMappings_RequestSubAreas_RequestSubAreaId",
                        column: x => x.RequestSubAreaId,
                        principalTable: "RequestSubAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestThreads",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    CommentDate = table.Column<DateTime>(nullable: false),
                    RequestId = table.Column<int>(nullable: true),
                    CommentById = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestThreads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestThreads_AbpUsers_CommentById",
                        column: x => x.CommentById,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestThreads_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TechTeams",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    TimeCharge = table.Column<int>(nullable: false),
                    Role = table.Column<int>(nullable: false),
                    RequestId = table.Column<int>(nullable: true),
                    CMACSUserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechTeams_AbpUsers_CMACSUserId",
                        column: x => x.CMACSUserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechTeams_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TORApprovals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(nullable: true),
                    TORTimeSent = table.Column<DateTime>(nullable: true),
                    Approved = table.Column<bool>(nullable: false),
                    ApprovedTime = table.Column<DateTime>(nullable: true),
                    ApproverId = table.Column<long>(nullable: true),
                    RequestId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TORApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TORApprovals_AbpUsers_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TORApprovals_Requests_RequestId",
                        column: x => x.RequestId,
                        principalTable: "Requests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttachedDocs_DocOwnerId",
                table: "AttachedDocs",
                column: "DocOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachedDocs_DocumentId",
                table: "AttachedDocs",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_AttachedDocs_RequestId",
                table: "AttachedDocs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLists_IndustryId",
                table: "ClientLists",
                column: "IndustryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLists_ReportingTerritoryId",
                table: "ClientLists",
                column: "ReportingTerritoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLists_StockExchangeId",
                table: "ClientLists",
                column: "StockExchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestApprovals_ApproverId",
                table: "RequestApprovals",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestApprovals_RequestId",
                table: "RequestApprovals",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestCmacsManagerApprovals_RequestId",
                table: "RequestCmacsManagerApprovals",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestCmacsManagerApprovals_UserId",
                table: "RequestCmacsManagerApprovals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDocs_PreparerId",
                table: "RequestDocs",
                column: "PreparerId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestDocs_RequestId",
                table: "RequestDocs",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_AssigneeId",
                table: "Requests",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ClientListId",
                table: "Requests",
                column: "ClientListId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestAreaId",
                table: "Requests",
                column: "RequestAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestDomainId",
                table: "Requests",
                column: "RequestDomainId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestorId",
                table: "Requests",
                column: "RequestorId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestorManagerId",
                table: "Requests",
                column: "RequestorManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_RequestorPartnerId",
                table: "Requests",
                column: "RequestorPartnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestSubAreaMappings_RequestId",
                table: "RequestSubAreaMappings",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestSubAreaMappings_RequestSubAreaId",
                table: "RequestSubAreaMappings",
                column: "RequestSubAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestSubAreas_RequestAreaId",
                table: "RequestSubAreas",
                column: "RequestAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestThreads_CommentById",
                table: "RequestThreads",
                column: "CommentById");

            migrationBuilder.CreateIndex(
                name: "IX_RequestThreads_RequestId",
                table: "RequestThreads",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_TechTeams_CMACSUserId",
                table: "TechTeams",
                column: "CMACSUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TechTeams_RequestId",
                table: "TechTeams",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_TORApprovals_ApproverId",
                table: "TORApprovals",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_TORApprovals_RequestId",
                table: "TORApprovals",
                column: "RequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttachedDocs");

            migrationBuilder.DropTable(
                name: "BOS_Customers");

            migrationBuilder.DropTable(
                name: "BOS_Projects");

            migrationBuilder.DropTable(
                name: "BOS_Resources");

            migrationBuilder.DropTable(
                name: "RequestApprovals");

            migrationBuilder.DropTable(
                name: "RequestCmacsManagerApprovals");

            migrationBuilder.DropTable(
                name: "RequestDocs");

            migrationBuilder.DropTable(
                name: "RequestSubAreaMappings");

            migrationBuilder.DropTable(
                name: "RequestThreads");

            migrationBuilder.DropTable(
                name: "TechTeams");

            migrationBuilder.DropTable(
                name: "TORApprovals");

            migrationBuilder.DropTable(
                name: "RequestSubAreas");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "ClientLists");

            migrationBuilder.DropTable(
                name: "RequestAreas");

            migrationBuilder.DropTable(
                name: "RequestDomains");

            migrationBuilder.DropTable(
                name: "Industries");

            migrationBuilder.DropTable(
                name: "ReportingTerritories");

            migrationBuilder.DropTable(
                name: "StockExchanges");
        }
    }
}
