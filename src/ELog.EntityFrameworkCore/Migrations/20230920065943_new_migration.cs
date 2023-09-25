using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ELog.EntityFrameworkCore.Migrations
{
    public partial class new_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    ServiceName = table.Column<string>(maxLength: 256, nullable: true),
                    MethodName = table.Column<string>(maxLength: 256, nullable: true),
                    Parameters = table.Column<string>(maxLength: 1024, nullable: true),
                    ReturnValue = table.Column<string>(nullable: true),
                    ExecutionTime = table.Column<DateTime>(nullable: false),
                    ExecutionDuration = table.Column<int>(nullable: false),
                    ClientIpAddress = table.Column<string>(maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(maxLength: 128, nullable: true),
                    BrowserInfo = table.Column<string>(maxLength: 512, nullable: true),
                    Exception = table.Column<string>(maxLength: 2000, nullable: true),
                    ImpersonatorUserId = table.Column<long>(nullable: true),
                    ImpersonatorTenantId = table.Column<int>(nullable: true),
                    CustomData = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditSQLResult",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlantName = table.Column<string>(nullable: true),
                    TenancyName = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    CountryName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    GS1Prefix = table.Column<string>(nullable: true),
                    License = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PlantId = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    StateName = table.Column<string>(nullable: true),
                    TaxRegistrationNo = table.Column<string>(nullable: true),
                    Website = table.Column<string>(nullable: true),
                    PlantTypeId = table.Column<int>(nullable: false),
                    MasterPlantId = table.Column<int>(nullable: false),
                    ApprovalStatus = table.Column<string>(nullable: true),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    SysStartTime = table.Column<DateTime>(nullable: false),
                    SysEndTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditSQLResult", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BackgroundJobs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    JobType = table.Column<string>(maxLength: 512, nullable: false),
                    JobArgs = table.Column<string>(maxLength: 1048576, nullable: false),
                    TryCount = table.Column<short>(nullable: false),
                    NextTryTime = table.Column<DateTime>(nullable: false),
                    LastTryTime = table.Column<DateTime>(nullable: true),
                    IsAbandoned = table.Column<bool>(nullable: false),
                    Priority = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DynamicProperties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PropertyName = table.Column<string>(nullable: true),
                    InputType = table.Column<string>(nullable: true),
                    Permission = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Editions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Editions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Elog_ClientForms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClientId = table.Column<int>(nullable: false),
                    FormName = table.Column<string>(nullable: true),
                    FormStartDate = table.Column<DateTime>(nullable: false),
                    FormEndDate = table.Column<DateTime>(nullable: false),
                    FormJson = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: false),
                    FormStatus = table.Column<int>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    ApprovedBy = table.Column<string>(nullable: true),
                    CheckedBy = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ApproveDateTime = table.Column<DateTime>(nullable: false),
                    Permissions = table.Column<string>(nullable: true),
                    MenuId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elog_ClientForms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Elog_ElogControls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ELogId = table.Column<int>(nullable: false),
                    ControlID = table.Column<int>(nullable: false),
                    ControlLabel = table.Column<string>(nullable: true),
                    ControlType = table.Column<string>(nullable: true),
                    ControlDefaults = table.Column<string>(nullable: true),
                    Sequence = table.Column<int>(nullable: false),
                    FlagIsDefaultSql = table.Column<bool>(nullable: false),
                    DBFieldName = table.Column<string>(nullable: true),
                    DBDataType = table.Column<string>(nullable: true),
                    FlagIsMandatory = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elog_ElogControls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EntityChangeSets",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BrowserInfo = table.Column<string>(maxLength: 512, nullable: true),
                    ClientIpAddress = table.Column<string>(maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(maxLength: 128, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    ExtensionData = table.Column<string>(nullable: true),
                    ImpersonatorTenantId = table.Column<int>(nullable: true),
                    ImpersonatorUserId = table.Column<long>(nullable: true),
                    Reason = table.Column<string>(maxLength: 256, nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityChangeSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FormApprovalData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FormId = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormApprovalData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 64, nullable: false),
                    Icon = table.Column<string>(maxLength: 128, nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LanguageTexts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    LanguageName = table.Column<string>(maxLength: 128, nullable: false),
                    Source = table.Column<string>(maxLength: 128, nullable: false),
                    Key = table.Column<string>(maxLength: 256, nullable: false),
                    Value = table.Column<string>(maxLength: 67108864, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LanguageTexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    notification_type = table.Column<int>(nullable: false),
                    assign_roles = table.Column<string>(nullable: true),
                    assign_email = table.Column<int>(nullable: false),
                    assign_mobile = table.Column<int>(nullable: false),
                    log_Id = table.Column<int>(nullable: false),
                    isActive = table.Column<bool>(nullable: false),
                    Repeat = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    NotificationName = table.Column<string>(maxLength: 96, nullable: false),
                    Data = table.Column<string>(maxLength: 1048576, nullable: true),
                    DataTypeName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityTypeName = table.Column<string>(maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(maxLength: 96, nullable: true),
                    Severity = table.Column<byte>(nullable: false),
                    UserIds = table.Column<string>(maxLength: 131072, nullable: true),
                    ExcludedUserIds = table.Column<string>(maxLength: 131072, nullable: true),
                    TenantIds = table.Column<string>(maxLength: 131072, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    NotificationName = table.Column<string>(maxLength: 96, nullable: true),
                    EntityTypeName = table.Column<string>(maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(maxLength: 96, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationUnitRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    OrganizationUnitId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUnitRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationUnits",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    ParentId = table.Column<long>(nullable: true),
                    Code = table.Column<string>(maxLength: 95, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationUnits_OrganizationUnits_ParentId",
                        column: x => x.ParentId,
                        principalTable: "OrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TenantNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    NotificationName = table.Column<string>(maxLength: 96, nullable: false),
                    Data = table.Column<string>(maxLength: 1048576, nullable: true),
                    DataTypeName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityTypeName = table.Column<string>(maxLength: 250, nullable: true),
                    EntityTypeAssemblyQualifiedName = table.Column<string>(maxLength: 512, nullable: true),
                    EntityId = table.Column<string>(maxLength: 96, nullable: true),
                    Severity = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    UserLinkId = table.Column<long>(nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLoginAttempts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    TenancyName = table.Column<string>(maxLength: 64, nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    UserNameOrEmailAddress = table.Column<string>(maxLength: 256, nullable: true),
                    ClientIpAddress = table.Column<string>(maxLength: 64, nullable: true),
                    ClientName = table.Column<string>(maxLength: 128, nullable: true),
                    BrowserInfo = table.Column<string>(maxLength: 512, nullable: true),
                    Result = table.Column<byte>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginAttempts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserNotifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    TenantNotificationId = table.Column<Guid>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserOrganizationUnits",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    OrganizationUnitId = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrganizationUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebhookEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WebhookName = table.Column<string>(nullable: false),
                    Data = table.Column<string>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletionTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebhookSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    WebhookUri = table.Column<string>(nullable: false),
                    Secret = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Webhooks = table.Column<string>(nullable: true),
                    Headers = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DynamicEntityProperties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EntityFullName = table.Column<string>(nullable: true),
                    DynamicPropertyId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicEntityProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicEntityProperties_DynamicProperties_DynamicPropertyId",
                        column: x => x.DynamicPropertyId,
                        principalTable: "DynamicProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DynamicPropertyValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    DynamicPropertyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicPropertyValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicPropertyValues_DynamicProperties_DynamicPropertyId",
                        column: x => x.DynamicPropertyId,
                        principalTable: "DynamicProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Features",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(maxLength: 2000, nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    EditionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Features", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Features_Editions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "Editions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityChanges",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ChangeTime = table.Column<DateTime>(nullable: false),
                    ChangeType = table.Column<byte>(nullable: false),
                    EntityChangeSetId = table.Column<long>(nullable: false),
                    EntityId = table.Column<string>(maxLength: 48, nullable: true),
                    EntityTypeFullName = table.Column<string>(maxLength: 192, nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityChanges_EntityChangeSets_EntityChangeSetId",
                        column: x => x.EntityChangeSetId,
                        principalTable: "EntityChangeSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebhookSendAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    WebhookEventId = table.Column<Guid>(nullable: false),
                    WebhookSubscriptionId = table.Column<Guid>(nullable: false),
                    Response = table.Column<string>(nullable: true),
                    ResponseStatusCode = table.Column<int>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookSendAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebhookSendAttempts_WebhookEvents_WebhookEventId",
                        column: x => x.WebhookEventId,
                        principalTable: "WebhookEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DynamicEntityPropertyValues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Value = table.Column<string>(nullable: false),
                    EntityId = table.Column<string>(nullable: true),
                    DynamicEntityPropertyId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DynamicEntityPropertyValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DynamicEntityPropertyValues_DynamicEntityProperties_DynamicE~",
                        column: x => x.DynamicEntityPropertyId,
                        principalTable: "DynamicEntityProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntityPropertyChanges",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EntityChangeId = table.Column<long>(nullable: false),
                    NewValue = table.Column<string>(maxLength: 512, nullable: true),
                    OriginalValue = table.Column<string>(maxLength: 512, nullable: true),
                    PropertyName = table.Column<string>(maxLength: 96, nullable: true),
                    PropertyTypeFullName = table.Column<string>(maxLength: 192, nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityPropertyChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntityPropertyChanges_EntityChanges_EntityChangeId",
                        column: x => x.EntityChangeId,
                        principalTable: "EntityChanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 32, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 64, nullable: false),
                    IsStatic = table.Column<bool>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    NormalizedName = table.Column<string>(maxLength: 32, nullable: false),
                    ConcurrencyStamp = table.Column<string>(maxLength: 128, nullable: true),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    ClaimType = table.Column<string>(maxLength: 256, nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AuthenticationSource = table.Column<string>(maxLength: 64, nullable: true),
                    UserName = table.Column<string>(maxLength: 256, nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    Password = table.Column<string>(maxLength: 128, nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 32, nullable: true),
                    IsActive = table.Column<bool>(nullable: false, defaultValue: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: false),
                    NormalizedEmailAddress = table.Column<string>(maxLength: 256, nullable: false),
                    ConcurrencyStamp = table.Column<string>(maxLength: 128, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    EmployeeCode = table.Column<string>(maxLength: 50, nullable: true),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    LockoutEndDateUtc = table.Column<DateTime>(nullable: true),
                    IsLockout = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ReportingManagerId = table.Column<long>(nullable: true),
                    PlantId = table.Column<int>(nullable: true),
                    ModeId = table.Column<int>(nullable: true),
                    DesignationId = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    PasswordStatus = table.Column<int>(nullable: false),
                    PasswordResetTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Users_ReportingManagerId",
                        column: x => x.ReportingManagerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActivityMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    ActivityName = table.Column<string>(nullable: true),
                    ActivityCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ModuleId = table.Column<int>(nullable: false),
                    SubModuleId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalLevelMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LevelCode = table.Column<int>(nullable: false),
                    LevelName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalLevelMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalLevelMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApprovalLevelMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApprovalLevelMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalStatusMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatus = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalStatusMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalStatusMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApprovalStatusMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApprovalStatusMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApprovalUserModuleMappingMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AppLevelId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    SubModuleId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalUserModuleMappingMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalUserModuleMappingMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApprovalUserModuleMappingMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApprovalUserModuleMappingMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AreaUsageLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ActivityID = table.Column<int>(nullable: false),
                    CubicalId = table.Column<int>(nullable: false),
                    OperatorName = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    StopTime = table.Column<DateTime>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    ApprovedBy = table.Column<int>(nullable: true),
                    VerifiedBy = table.Column<int>(nullable: true),
                    ApprovedTime = table.Column<DateTime>(nullable: true),
                    StatusId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaUsageLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AreaUsageLog_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AreaUsageLog_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AreaUsageLog_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CageLabelPrinting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DispensingId = table.Column<int>(nullable: false),
                    DispensingBarcode = table.Column<string>(nullable: true),
                    ProductID = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(nullable: true),
                    ProcessorderID = table.Column<int>(nullable: false),
                    CubicleID = table.Column<int>(nullable: false),
                    CubcileCode = table.Column<string>(nullable: true),
                    NoOfContainer = table.Column<int>(nullable: true),
                    CageLabelBarcode = table.Column<string>(nullable: true),
                    PrintCount = table.Column<int>(nullable: true),
                    PrinterID = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CageLabelPrinting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CageLabelPrinting_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CageLabelPrinting_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CageLabelPrinting_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CalibrationStatusMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StatusName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationStatusMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalibrationStatusMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalibrationStatusMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalibrationStatusMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CalibrationTestStatusMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StatusName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationTestStatusMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalibrationTestStatusMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalibrationTestStatusMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalibrationTestStatusMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckpointTypeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckpointTypeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckpointTypeMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckpointTypeMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckpointTypeMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Consumption",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CubicleId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    ProcessOrderId = table.Column<int>(nullable: true),
                    EquipmentId = table.Column<int>(nullable: true),
                    NoOfContainer = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Consumption_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Consumption_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Consumption_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CountryMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CountryName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CountryMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CountryMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CountryMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CubicleAssignmentWIP",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    ProductCode = table.Column<string>(maxLength: 100, nullable: true),
                    ProcessOrderId = table.Column<int>(nullable: true),
                    CubicleBarcodeId = table.Column<int>(nullable: false),
                    EquipmentBarcodeId = table.Column<int>(nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CubicleAssignmentWIP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CubicleAssignmentWIP_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleAssignmentWIP_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleAssignmentWIP_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CubicleCleaningTypeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Value = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CubicleCleaningTypeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningTypeMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningTypeMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningTypeMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DesignationMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    DesignationName = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DesignationMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DesignationMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DesignationMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DesignationMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeviceTypeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeviceName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTypeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceTypeMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeviceTypeMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeviceTypeMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DispatchDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OBD = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    PutawayId = table.Column<int>(nullable: true),
                    PickingId = table.Column<int>(nullable: true),
                    ProductBatchNo = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    LineItem = table.Column<string>(nullable: true),
                    Batch = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PalletBarcode = table.Column<string>(nullable: true),
                    PalletCount = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: true),
                    UOM = table.Column<string>(nullable: true),
                    NoOfPacks = table.Column<int>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    CustomerAddress = table.Column<string>(nullable: true),
                    TransportName = table.Column<string>(nullable: true),
                    VehicleNo = table.Column<string>(nullable: true),
                    isActive = table.Column<bool>(nullable: true),
                    isPicked = table.Column<bool>(nullable: true),
                    HUCode = table.Column<string>(nullable: true),
                    PlantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatchDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispatchDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispatchDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispatchDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentCleaningTypeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Value = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCleaningTypeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentCleaningTypeMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentCleaningTypeMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentCleaningTypeMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentTypeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    EquipmentName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTypeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentTypeMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentTypeMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentTypeMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUsageLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ActivityId = table.Column<int>(nullable: false),
                    OperatorName = table.Column<string>(nullable: true),
                    EquipmentType = table.Column<string>(nullable: true),
                    EquipmentBracodeId = table.Column<int>(nullable: false),
                    ProcessBarcodeId = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    ApprovedBy = table.Column<int>(nullable: true),
                    ApprovedTime = table.Column<DateTime>(nullable: true),
                    StatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUsageLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUsageLog_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentUsageLog_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentUsageLog_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgPicking",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OBD = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    ProductBatchNo = table.Column<string>(nullable: true),
                    LineItem = table.Column<string>(nullable: true),
                    Batch = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SuggestedLocationId = table.Column<int>(nullable: true),
                    LocationId = table.Column<int>(nullable: true),
                    LocationBarcode = table.Column<string>(nullable: true),
                    PalletBarcode = table.Column<string>(nullable: true),
                    PalletCount = table.Column<int>(nullable: true),
                    ShipperCount = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: true),
                    UOM = table.Column<string>(nullable: true),
                    NoOfPacks = table.Column<int>(nullable: true),
                    isActive = table.Column<bool>(nullable: true),
                    isPicked = table.Column<bool>(nullable: true),
                    HUCode = table.Column<string>(nullable: true),
                    PlantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgPicking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgPicking_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgPicking_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgPicking_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FgPutAway",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PalletId = table.Column<int>(nullable: true),
                    PalletBarcode = table.Column<string>(nullable: true),
                    PalletCount = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: true),
                    LocationBarcode = table.Column<string>(nullable: true),
                    isActive = table.Column<bool>(nullable: false),
                    isPicked = table.Column<bool>(nullable: false),
                    HUCode = table.Column<string>(nullable: true),
                    PlantId = table.Column<int>(nullable: true),
                    ProductBatchNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FgPutAway", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FgPutAway_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgPutAway_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FgPutAway_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FrequencyTypeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    FrequencyName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrequencyTypeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FrequencyTypeMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FrequencyTypeMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FrequencyTypeMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GRNHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    GRNNumber = table.Column<string>(maxLength: 50, nullable: false),
                    GRNPostingDate = table.Column<DateTime>(nullable: false),
                    PurchaseOrderId = table.Column<int>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRNHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GRNHeaders_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNHeaders_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNHeaders_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HandlingUnitTypeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    HandlingUnitName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandlingUnitTypeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HandlingUnitTypeMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HandlingUnitTypeMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HandlingUnitTypeMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HolidayTypeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    HolidayType = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayTypeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HolidayTypeMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HolidayTypeMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HolidayTypeMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InProcessLabelDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CubicleId = table.Column<int>(nullable: false),
                    ProcessOrderId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    ContainerBarcodeId = table.Column<int>(nullable: false),
                    ContainerBarcode = table.Column<string>(nullable: true),
                    ScanBalanceId = table.Column<int>(nullable: true),
                    ScanBalance = table.Column<string>(nullable: true),
                    GrossWeight = table.Column<float>(nullable: true),
                    NetWeight = table.Column<float>(nullable: true),
                    TareWeight = table.Column<float>(nullable: true),
                    NoOfContainer = table.Column<string>(nullable: true),
                    ProcessLabelBarcode = table.Column<string>(nullable: true),
                    IsPrint = table.Column<bool>(nullable: false),
                    Ischeck = table.Column<bool>(nullable: false),
                    PrintCount = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    PrinterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InProcessLabelDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InProcessLabelDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InProcessLabelDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InProcessLabelDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InspectionChecklistMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    ChecklistCode = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    PlantId = table.Column<int>(nullable: false),
                    SubModuleId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Version = table.Column<int>(nullable: false),
                    FormatNumber = table.Column<string>(maxLength: 100, nullable: true),
                    ChecklistTypeId = table.Column<int>(nullable: false),
                    ModeId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionChecklistMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionChecklistMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionChecklistMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionChecklistMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InspectionLot",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PlantId = table.Column<int>(nullable: false),
                    InspectionLotNumber = table.Column<string>(maxLength: 100, nullable: false),
                    ProductCode = table.Column<string>(maxLength: 100, nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionLot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InspectionLot_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionLot_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InspectionLot_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PurchaseOrderId = table.Column<int>(nullable: true),
                    PurchaseOrderNo = table.Column<string>(maxLength: 100, nullable: true),
                    VendorName = table.Column<string>(maxLength: 100, nullable: false),
                    VendorCode = table.Column<string>(maxLength: 100, nullable: true),
                    InvoiceNo = table.Column<string>(maxLength: 100, nullable: false),
                    InvoiceDate = table.Column<DateTime>(nullable: false),
                    LRNo = table.Column<string>(maxLength: 50, nullable: true),
                    LRDate = table.Column<DateTime>(nullable: true),
                    DriverName = table.Column<string>(maxLength: 50, nullable: false),
                    VehicleNumber = table.Column<string>(maxLength: 50, nullable: false),
                    TransporterName = table.Column<string>(maxLength: 50, nullable: true),
                    purchaseOrderDeliverSchedule = table.Column<string>(maxLength: 50, nullable: true),
                    VendorBatchNo = table.Column<string>(maxLength: 50, nullable: false),
                    Manufacturer = table.Column<string>(maxLength: 50, nullable: false),
                    ManufacturerCode = table.Column<string>(maxLength: 50, nullable: true),
                    DeliveryNote = table.Column<string>(maxLength: 50, nullable: false),
                    BillofLanding = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IssueToProductions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProcessOrderNo = table.Column<string>(nullable: true),
                    MaterialCode = table.Column<string>(nullable: true),
                    LineItemNo = table.Column<string>(nullable: true),
                    MaterialDescription = table.Column<string>(nullable: true),
                    Product = table.Column<string>(nullable: true),
                    ProductBatch = table.Column<string>(nullable: true),
                    ArNo = table.Column<string>(nullable: true),
                    SAPBatchNo = table.Column<string>(nullable: true),
                    DispensedQty = table.Column<string>(nullable: true),
                    UOM = table.Column<string>(nullable: true),
                    Storage_location = table.Column<string>(nullable: true),
                    MaterialIssueNoteNo = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    MvtType = table.Column<string>(nullable: true),
                    DispensingHeaderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssueToProductions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssueToProductions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssueToProductions_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IssueToProductions_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LabelPrintPacking",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    ProcessOrderId = table.Column<int>(nullable: true),
                    ContainerBarcodeId = table.Column<int>(nullable: false),
                    ContainerBarcode = table.Column<string>(nullable: true),
                    ContainerCount = table.Column<int>(nullable: false),
                    Quantity = table.Column<string>(nullable: true),
                    PackingLabelBarcode = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsPrint = table.Column<bool>(nullable: false),
                    PrintCount = table.Column<int>(nullable: false),
                    PrinterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelPrintPacking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabelPrintPacking_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LabelPrintPacking_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LabelPrintPacking_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Loading",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OBD = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    PutawayId = table.Column<int>(nullable: true),
                    PickingId = table.Column<int>(nullable: true),
                    ProductBatchNo = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    LineItem = table.Column<string>(nullable: true),
                    Batch = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    PalletBarcode = table.Column<string>(nullable: true),
                    PalletCount = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: true),
                    UOM = table.Column<string>(nullable: true),
                    NoOfPacks = table.Column<int>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    CustomerAddress = table.Column<string>(nullable: true),
                    TransportName = table.Column<string>(nullable: true),
                    VehicleNo = table.Column<string>(nullable: true),
                    isActive = table.Column<bool>(nullable: true),
                    isPicked = table.Column<bool>(nullable: true),
                    HUCode = table.Column<string>(nullable: true),
                    PlantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loading", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loading_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loading_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Loading_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogFormHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    FormId = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogFormHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogFormHistory_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogFormHistory_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogFormHistory_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogoMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ImageTitle = table.Column<string>(nullable: true),
                    ImageData = table.Column<byte[]>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogoMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LogoMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogoMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LogoMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialBatchDispensingHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CubicleCode = table.Column<string>(maxLength: 100, nullable: true),
                    GroupCode = table.Column<string>(maxLength: 100, nullable: true),
                    MaterialCode = table.Column<string>(maxLength: 100, nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    PickingTime = table.Column<DateTime>(nullable: false),
                    BatchPickingStatusId = table.Column<int>(nullable: false),
                    SAPBatchNumber = table.Column<string>(maxLength: 100, nullable: true),
                    MaterialBatchDispensingHeaderType = table.Column<int>(nullable: false),
                    IsSampling = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialBatchDispensingHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialBatchDispensingHeaders_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialBatchDispensingHeaders_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialBatchDispensingHeaders_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialDestructions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MovementType = table.Column<string>(nullable: true),
                    ContainerId = table.Column<int>(nullable: false),
                    MaterialContainerBarCode = table.Column<string>(nullable: true),
                    MaterialCode = table.Column<string>(nullable: true),
                    SAPBatchNo = table.Column<string>(nullable: true),
                    ARNo = table.Column<string>(nullable: true),
                    Quantity = table.Column<float>(nullable: true),
                    UnitOfMeasurement = table.Column<string>(nullable: true),
                    IsPostedToSAP = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialDestructions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialDestructions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialDestructions_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialDestructions_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MaterialCode = table.Column<string>(maxLength: 50, nullable: false),
                    MaterialDescription = table.Column<string>(nullable: true),
                    BaseUOM = table.Column<string>(maxLength: 50, nullable: false),
                    Grade = table.Column<string>(maxLength: 50, nullable: false),
                    Denominator = table.Column<float>(nullable: false),
                    Numerator = table.Column<float>(nullable: false),
                    ConversionUOM = table.Column<string>(maxLength: 50, nullable: false),
                    MaterialType = table.Column<string>(maxLength: 50, nullable: false),
                    Flag = table.Column<int>(nullable: true),
                    TempStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialReturnDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DocumentNo = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    ProductNo = table.Column<string>(nullable: true),
                    BatchNo = table.Column<string>(nullable: true),
                    UOMId = table.Column<int>(nullable: false),
                    ContainerId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ScanBalanceId = table.Column<int>(nullable: false),
                    ScanBalanceNo = table.Column<string>(nullable: true),
                    GrossWeight = table.Column<float>(nullable: true),
                    NetWeight = table.Column<float>(nullable: true),
                    TareWeight = table.Column<float>(nullable: true),
                    IsPrint = table.Column<bool>(nullable: false),
                    Ischeck = table.Column<bool>(nullable: false),
                    PrintCount = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    PrinterId = table.Column<int>(nullable: true),
                    MaterialReturnProcessLabelBarcode = table.Column<string>(nullable: true),
                    ProcessOrderId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialReturnDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialReturnDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialReturnDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialReturnDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialReturnDetailsSAP",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MaterialDocumentNo = table.Column<string>(nullable: true),
                    MaterialDocumentYear = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    ProductBatchNo = table.Column<string>(nullable: true),
                    ProductId = table.Column<int>(nullable: false),
                    ItemName = table.Column<string>(nullable: true),
                    ItemCode = table.Column<string>(nullable: true),
                    LineItemNo = table.Column<string>(nullable: true),
                    MaterialDescription = table.Column<string>(nullable: true),
                    ARNo = table.Column<string>(nullable: true),
                    SAPBatchNo = table.Column<string>(nullable: true),
                    Qty = table.Column<float>(nullable: true),
                    UOM = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialReturnDetailsSAP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialReturnDetailsSAP_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialReturnDetailsSAP_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialReturnDetailsSAP_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialTransferTypeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TransferType = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialTransferTypeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialTransferTypeMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialTransferTypeMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialTransferTypeMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    ModeName = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsController = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModeMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModeMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModeMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModuleMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OBDDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    OBD = table.Column<string>(nullable: true),
                    LineItemNo = table.Column<string>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    ProductDesc = table.Column<string>(nullable: true),
                    ProductBatchNo = table.Column<string>(nullable: true),
                    ARNo = table.Column<string>(nullable: true),
                    SAPBatchNo = table.Column<string>(nullable: true),
                    Qty = table.Column<float>(nullable: true),
                    UOM = table.Column<int>(nullable: true),
                    CustomerName = table.Column<string>(nullable: true),
                    CustomerAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBDDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OBDDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OBDDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OBDDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PackingMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProductId = table.Column<string>(nullable: true),
                    ProcessOrderId = table.Column<int>(nullable: true),
                    ContainerId = table.Column<int>(nullable: true),
                    ContainerCount = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackingMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PackingMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PackingMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PalletMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Pallet_Barcode = table.Column<string>(nullable: true),
                    Carton_barcode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ProductBatchNo = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    PalletBarcodeId = table.Column<int>(nullable: false),
                    CartonBarcodeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PalletMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PalletMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PalletMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PalletMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermissionMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Action = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    IsGranted = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    RoleId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Permissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickingMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProcessOrderId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Stage = table.Column<string>(nullable: true),
                    SuggestedLocationId = table.Column<int>(nullable: true),
                    LocationId = table.Column<int>(nullable: true),
                    ContainerId = table.Column<int>(nullable: true),
                    ContainerCode = table.Column<string>(nullable: true),
                    ContainerCount = table.Column<int>(nullable: true),
                    Quantity = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickingMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickingMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PickingMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PickingMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostWIPDataToSAP",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    ProcessOrderId = table.Column<int>(nullable: true),
                    InProcessLabelId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsSent = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostWIPDataToSAP", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostWIPDataToSAP_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostWIPDataToSAP_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PostWIPDataToSAP_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PRNEntryMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    PRNFileName = table.Column<string>(nullable: true),
                    PlantId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    ModuleId = table.Column<int>(nullable: false),
                    SubModuleId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRNEntryMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PRNEntryMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRNEntryMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PRNEntryMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcessOrderAfterRelease",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PlantId = table.Column<int>(nullable: false),
                    ProcessOrderNo = table.Column<string>(maxLength: 100, nullable: false),
                    ProcessOrderType = table.Column<string>(maxLength: 100, nullable: true),
                    ProcessOrderDate = table.Column<DateTime>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 100, nullable: false),
                    ProductCodeId = table.Column<int>(nullable: false),
                    TecoFlag = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsPicking = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessOrderAfterRelease", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessOrderAfterRelease_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessOrderAfterRelease_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessOrderAfterRelease_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcessOrderMaterialAfterRelease",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProcessOrderId = table.Column<int>(nullable: true),
                    ProcessOrderNo = table.Column<string>(maxLength: 100, nullable: true),
                    LineItemNo = table.Column<string>(nullable: true),
                    MaterialCode = table.Column<string>(nullable: true),
                    MaterialDescription = table.Column<string>(nullable: true),
                    ARNO = table.Column<string>(nullable: true),
                    LotNo = table.Column<string>(nullable: true),
                    SAPBatchNo = table.Column<string>(nullable: true),
                    ProductBatchNo = table.Column<string>(nullable: true),
                    CurrentStage = table.Column<string>(nullable: true),
                    NextStage = table.Column<string>(nullable: true),
                    Quantity = table.Column<float>(nullable: false),
                    UOM = table.Column<string>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    RetestDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessOrderMaterialAfterRelease", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessOrderMaterialAfterRelease_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessOrderMaterialAfterRelease_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessOrderMaterialAfterRelease_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcessOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PlantId = table.Column<int>(nullable: false),
                    ProcessOrderNo = table.Column<string>(maxLength: 100, nullable: false),
                    ProcessOrderType = table.Column<string>(maxLength: 100, nullable: true),
                    ProcessOrderDate = table.Column<DateTime>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 100, nullable: false),
                    IssueQuantityUOM = table.Column<string>(maxLength: 50, nullable: true),
                    IssueIndicator = table.Column<string>(maxLength: 50, nullable: true),
                    IsReservationNo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessOrders_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessOrders_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessOrders_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Putaway",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    ContainerId = table.Column<int>(nullable: false),
                    ContainerCode = table.Column<string>(nullable: true),
                    isActive = table.Column<bool>(nullable: false),
                    ProductCodeId = table.Column<int>(nullable: false),
                    StorageLocation = table.Column<string>(nullable: true),
                    ProcessOrderNo = table.Column<string>(nullable: true),
                    ProcessOrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Putaway", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Putaway_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Putaway_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Putaway_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RecipeId = table.Column<int>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 64, nullable: true),
                    ProductName = table.Column<string>(maxLength: 64, nullable: true),
                    ProductNo = table.Column<string>(maxLength: 64, nullable: true),
                    DocVersion = table.Column<string>(maxLength: 64, nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipeTransactionHeader",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    DocumentVersion = table.Column<string>(nullable: true),
                    RecipeNo = table.Column<string>(nullable: true),
                    ApprovedById = table.Column<int>(nullable: true),
                    ApprovedLevelId = table.Column<int>(nullable: true),
                    ApprovedDate = table.Column<DateTime>(nullable: true),
                    ApprovalRemarks = table.Column<string>(nullable: true),
                    ApprovalStatus = table.Column<string>(nullable: true),
                    RejectedById = table.Column<int>(nullable: true),
                    RejectedDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeTransactionHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeTransactionHeader_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeTransactionHeader_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeTransactionHeader_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipeWiseProcessOrderMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProcessOrderId = table.Column<int>(nullable: false),
                    RecipeTransHdrId = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(nullable: true),
                    IsActive = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeWiseProcessOrderMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeWiseProcessOrderMapping_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeWiseProcessOrderMapping_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeWiseProcessOrderMapping_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SamplingTypeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SamplingTypeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SamplingTypeMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SamplingTypeMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SamplingTypeMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SAPGRNPosting",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ItemCode = table.Column<string>(nullable: true),
                    VendorCode = table.Column<string>(nullable: true),
                    VendorBatch = table.Column<string>(nullable: true),
                    NetQty = table.Column<string>(nullable: true),
                    UOM = table.Column<string>(nullable: true),
                    PurchaseOrder = table.Column<string>(nullable: true),
                    POLineItem = table.Column<string>(nullable: true),
                    LRNo = table.Column<string>(nullable: true),
                    LRDate = table.Column<string>(nullable: true),
                    Vehicle = table.Column<string>(nullable: true),
                    NoOfCases = table.Column<string>(nullable: true),
                    TransporterName = table.Column<string>(nullable: true),
                    MfgDate = table.Column<string>(nullable: true),
                    ExpDate = table.Column<string>(nullable: true),
                    Manufacturer = table.Column<string>(nullable: true),
                    GRNNo = table.Column<string>(nullable: true),
                    InspectionLotNo = table.Column<string>(nullable: true),
                    SAPBatchNo = table.Column<string>(nullable: true),
                    NextInspectionDate = table.Column<string>(nullable: true),
                    Delivery_note_no = table.Column<string>(nullable: true),
                    Storage_location = table.Column<string>(nullable: true),
                    Bill_of_lading = table.Column<string>(nullable: true),
                    LineItem = table.Column<string>(nullable: true),
                    MfgBatchNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAPGRNPosting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SAPGRNPosting_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SAPGRNPosting_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SAPGRNPosting_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SAPPlantMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PlantCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    TaxRegNo = table.Column<string>(nullable: true),
                    License = table.Column<string>(nullable: true),
                    GS1Prefix = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAPPlantMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SAPPlantMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SAPPlantMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SAPPlantMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SAPProcessOrderReceivedMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Plant = table.Column<string>(maxLength: 50, nullable: true),
                    PONo = table.Column<string>(maxLength: 50, nullable: true),
                    PODate = table.Column<DateTime>(nullable: true),
                    LineItemNo = table.Column<string>(maxLength: 50, nullable: true),
                    OrderQty = table.Column<decimal>(nullable: true),
                    UOM = table.Column<string>(maxLength: 50, nullable: true),
                    ItemCode = table.Column<string>(maxLength: 50, nullable: true),
                    ItemDescription = table.Column<string>(maxLength: 200, nullable: true),
                    VendorName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAPProcessOrderReceivedMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SAPProcessOrderReceivedMaterials_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SAPProcessOrderReceivedMaterials_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SAPProcessOrderReceivedMaterials_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SAPProcessOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProcessOrderNo = table.Column<string>(maxLength: 100, nullable: false),
                    LineItemNo = table.Column<string>(maxLength: 100, nullable: true),
                    MaterialCode = table.Column<string>(maxLength: 100, nullable: true),
                    MaterialDescription = table.Column<string>(nullable: true),
                    ARNo = table.Column<string>(maxLength: 100, nullable: true),
                    SAPBatchNo = table.Column<string>(maxLength: 100, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 100, nullable: true),
                    ProductBatchNo = table.Column<string>(maxLength: 100, nullable: true),
                    ReqDispensedQty = table.Column<decimal>(nullable: false),
                    UOM = table.Column<string>(maxLength: 100, nullable: true),
                    BaseUOM = table.Column<string>(maxLength: 100, nullable: true),
                    BaseQty = table.Column<decimal>(nullable: false),
                    CurrentStage = table.Column<string>(maxLength: 100, nullable: true),
                    NextStage = table.Column<string>(maxLength: 100, nullable: true),
                    DispensingQty = table.Column<decimal>(nullable: false),
                    DispensingUOM = table.Column<string>(nullable: true),
                    Plant = table.Column<string>(maxLength: 100, nullable: true),
                    StorageLocation = table.Column<string>(maxLength: 100, nullable: true),
                    IsReservationNo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAPProcessOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SAPProcessOrders_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SAPProcessOrders_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SAPProcessOrders_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SAPQualityControlDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ItemCode = table.Column<string>(maxLength: 50, nullable: true),
                    InspectionlotNo = table.Column<string>(maxLength: 50, nullable: true),
                    SAPBatchNo = table.Column<string>(maxLength: 50, nullable: true),
                    BatchStockStatus = table.Column<string>(maxLength: 50, nullable: true),
                    RetestDate = table.Column<DateTime>(nullable: true),
                    ReleasedOn = table.Column<DateTime>(nullable: true),
                    ReleasedQty = table.Column<decimal>(nullable: true),
                    MovementType = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAPQualityControlDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SAPQualityControlDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SAPQualityControlDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SAPQualityControlDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SAPReturntoMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MaterialDocumentNo = table.Column<string>(maxLength: 100, nullable: true),
                    MaterialDocumentYear = table.Column<string>(maxLength: 100, nullable: true),
                    ItemCode = table.Column<string>(maxLength: 100, nullable: false),
                    LineItemNo = table.Column<string>(nullable: true),
                    MaterialDescription = table.Column<string>(maxLength: 100, nullable: true),
                    SAPBatchNo = table.Column<string>(maxLength: 100, nullable: true),
                    Qty = table.Column<decimal>(nullable: false),
                    UOM = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAPReturntoMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SAPReturntoMaterial_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SAPReturntoMaterial_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SAPReturntoMaterial_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SAPUOMMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UOM = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SAPUOMMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SAPUOMMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SAPUOMMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SAPUOMMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Settings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubModuleTypeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SubModuleType = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubModuleTypeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubModuleTypeMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubModuleTypeMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubModuleTypeMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TenancyName = table.Column<string>(maxLength: 64, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    ConnectionString = table.Column<string>(maxLength: 1024, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    EditionId = table.Column<int>(nullable: true),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tenants_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tenants_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tenants_Editions_EditionId",
                        column: x => x.EditionId,
                        principalTable: "Editions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tenants_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionStatusMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TransactionStatus = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionStatusMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionStatusMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionStatusMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionStatusMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UnitOfMeasurementTypeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UnitOfMeasurementTypeName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitOfMeasurementTypeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitOfMeasurementTypeMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitOfMeasurementTypeMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitOfMeasurementTypeMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    ClaimType = table.Column<string>(maxLength: 256, nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: true),
                    Name = table.Column<string>(maxLength: 128, nullable: true),
                    Value = table.Column<string>(maxLength: 512, nullable: true),
                    ExpireDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeightVerificationHeader",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    BatchId = table.Column<int>(nullable: true),
                    LotId = table.Column<int>(nullable: true),
                    ProcessOrderId = table.Column<int>(nullable: true),
                    CubicalId = table.Column<int>(nullable: true),
                    DispensedId = table.Column<int>(nullable: true),
                    UnitofMeasurementId = table.Column<int>(nullable: true),
                    NoOfContainers = table.Column<int>(nullable: true),
                    NoOfPacks = table.Column<int>(nullable: true),
                    RecivedNoOfPacks = table.Column<int>(nullable: true),
                    ScanBalanceId = table.Column<int>(nullable: false),
                    ScanBalanceNo = table.Column<string>(nullable: true),
                    DispGrossWeight = table.Column<float>(nullable: true),
                    NetWeight = table.Column<float>(nullable: true),
                    TareWeight = table.Column<float>(nullable: true),
                    GrossWeight = table.Column<float>(nullable: true),
                    IsGrossWeight = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightVerificationHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightVerificationHeader_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeightVerificationHeader_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeightVerificationHeader_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WIPLineClearanceTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ClearanceDate = table.Column<DateTime>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    ProductCode = table.Column<string>(maxLength: 100, nullable: true),
                    ProcessOrderId = table.Column<int>(nullable: true),
                    CubicleBarcodeId = table.Column<int>(nullable: false),
                    EquipmentBarcodeId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    StopTime = table.Column<DateTime>(nullable: true),
                    VerifiedBy = table.Column<int>(nullable: true),
                    ApprovedBy = table.Column<int>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    IsSampling = table.Column<bool>(nullable: false),
                    ApprovedTime = table.Column<DateTime>(nullable: true),
                    ChecklistTypeId = table.Column<int>(nullable: true),
                    Remarks = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WIPLineClearanceTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WIPLineClearanceTransaction_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WIPLineClearanceTransaction_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WIPLineClearanceTransaction_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WIPMaterialVerification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CubicleId = table.Column<int>(nullable: false),
                    ProcessOrderId = table.Column<int>(nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    CageBarcodeId = table.Column<int>(nullable: false),
                    CageBarcode = table.Column<string>(nullable: true),
                    NoOfCage = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WIPMaterialVerification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WIPMaterialVerification_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WIPMaterialVerification_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WIPMaterialVerification_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WMSPasswordManager",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WMSPasswordManager", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WMSPasswordManager_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMSPasswordManager_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMSPasswordManager_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    ZField = table.Column<string>(nullable: true),
                    DescriptionField = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ZMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ZMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConsumptionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ConsumptionId = table.Column<int>(nullable: true),
                    MaterialBarocdeId = table.Column<int>(nullable: true),
                    LineItemNo = table.Column<string>(nullable: true),
                    BatchNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumptionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsumptionDetails_Consumption_ConsumptionId",
                        column: x => x.ConsumptionId,
                        principalTable: "Consumption",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsumptionDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsumptionDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConsumptionDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StateMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    StateName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateMaster_CountryMaster_CountryId",
                        column: x => x.CountryId,
                        principalTable: "CountryMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StateMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StateMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StateMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CalenderMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    SubPlantId = table.Column<int>(nullable: false),
                    CalenderDate = table.Column<DateTime>(nullable: false),
                    HolidayTypeId = table.Column<int>(nullable: false),
                    HolidayName = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalenderMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalenderMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalenderMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalenderMaster_HolidayTypeMaster_HolidayTypeId",
                        column: x => x.HolidayTypeId,
                        principalTable: "HolidayTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CalenderMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CheckpointMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    InspectionChecklistId = table.Column<int>(nullable: true),
                    CheckpointName = table.Column<string>(maxLength: 100, nullable: false),
                    CheckpointTypeId = table.Column<int>(nullable: false),
                    ModeId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    ValueTag = table.Column<string>(maxLength: 100, nullable: true),
                    AcceptanceValue = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckpointMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckpointMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckpointMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckpointMaster_InspectionChecklistMaster_InspectionCheckli~",
                        column: x => x.InspectionChecklistId,
                        principalTable: "InspectionChecklistMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CheckpointMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GateEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    GatePassNo = table.Column<string>(maxLength: 50, nullable: false),
                    PrintCount = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    PrinterId = table.Column<int>(nullable: true),
                    InvoiceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GateEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GateEntry_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GateEntry_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GateEntry_InvoiceDetails_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "InvoiceDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GateEntry_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialBatchDispensingContainerDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MaterialBatchDispensingHeaderId = table.Column<int>(nullable: false),
                    ContainerBarCode = table.Column<string>(maxLength: 100, nullable: true),
                    SAPBatchNumber = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<float>(nullable: true),
                    ContainerPickingTime = table.Column<DateTime>(nullable: false),
                    IsVerified = table.Column<int>(nullable: true),
                    verifiedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialBatchDispensingContainerDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialBatchDispensingContainerDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialBatchDispensingContainerDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialBatchDispensingContainerDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialBatchDispensingContainerDetails_MaterialBatchDispens~",
                        column: x => x.MaterialBatchDispensingHeaderId,
                        principalTable: "MaterialBatchDispensingHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessOrderMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ProcessOrderId = table.Column<int>(nullable: true),
                    ProcessOrderNo = table.Column<string>(maxLength: 100, nullable: true),
                    ItemNo = table.Column<string>(maxLength: 100, nullable: false),
                    ItemCode = table.Column<string>(maxLength: 100, nullable: false),
                    ItemDescription = table.Column<string>(nullable: true),
                    OrderQuantity = table.Column<float>(nullable: true),
                    UnitOfMeasurementId = table.Column<int>(nullable: true),
                    UnitOfMeasurement = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    BatchNo = table.Column<string>(maxLength: 100, nullable: true),
                    SAPBatchNo = table.Column<string>(maxLength: 100, nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    RetestDate = table.Column<DateTime>(nullable: false),
                    ARNo = table.Column<string>(maxLength: 100, nullable: true),
                    InspectionLotId = table.Column<int>(nullable: true),
                    InspectionLotNo = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessOrderMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessOrderMaterials_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessOrderMaterials_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessOrderMaterials_InspectionLot_InspectionLotId",
                        column: x => x.InspectionLotId,
                        principalTable: "InspectionLot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessOrderMaterials_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcessOrderMaterials_ProcessOrders_ProcessOrderId",
                        column: x => x.ProcessOrderId,
                        principalTable: "ProcessOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipeTransactionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RecipeTransactionHeaderId = table.Column<int>(nullable: false),
                    Operation = table.Column<string>(nullable: true),
                    Stage = table.Column<string>(nullable: true),
                    NextOperation = table.Column<string>(nullable: true),
                    Component = table.Column<string>(nullable: true),
                    IsWeightRequired = table.Column<bool>(nullable: false),
                    IsLebalPrintingRequired = table.Column<bool>(nullable: false),
                    IsVerificationReq = table.Column<bool>(nullable: false),
                    InProcessSamplingRequired = table.Column<bool>(nullable: false),
                    IsSamplingReq = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    DocumentVersion = table.Column<string>(nullable: true),
                    RecipeNo = table.Column<string>(nullable: true),
                    MaterialDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeTransactionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeTransactionDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeTransactionDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeTransactionDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeTransactionDetails_RecipeTransactionHeader_RecipeTrans~",
                        column: x => x.RecipeTransactionHeaderId,
                        principalTable: "RecipeTransactionHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubModuleMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Sequence = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    SubModuleTypeId = table.Column<int>(nullable: true),
                    IsApprovalRequired = table.Column<bool>(nullable: false),
                    IsApprovalWorkflowRequired = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubModuleMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubModuleMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubModuleMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubModuleMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubModuleMaster_SubModuleTypeMaster_SubModuleTypeId",
                        column: x => x.SubModuleTypeId,
                        principalTable: "SubModuleTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UnitOfMeasurementMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    UOMCode = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    UnitOfMeasurementTypeId = table.Column<int>(nullable: true),
                    ConversionUOM = table.Column<string>(maxLength: 50, nullable: true),
                    UnitOfMeasurement = table.Column<string>(maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitOfMeasurementMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnitOfMeasurementMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitOfMeasurementMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitOfMeasurementMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UnitOfMeasurementMaster_UnitOfMeasurementTypeMaster_UnitOfMe~",
                        column: x => x.UnitOfMeasurementTypeId,
                        principalTable: "UnitOfMeasurementTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WIPLineClearanceCheckpoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LineClearanceTransactionId = table.Column<int>(nullable: false),
                    CheckPointId = table.Column<int>(nullable: false),
                    Observation = table.Column<string>(maxLength: 200, nullable: true),
                    Remark = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WIPLineClearanceCheckpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WIPLineClearanceCheckpoints_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WIPLineClearanceCheckpoints_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WIPLineClearanceCheckpoints_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WIPLineClearanceCheckpoints_WIPLineClearanceTransaction_Line~",
                        column: x => x.LineClearanceTransactionId,
                        principalTable: "WIPLineClearanceTransaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlantMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    PlantName = table.Column<string>(maxLength: 100, nullable: false),
                    PlantId = table.Column<string>(maxLength: 100, nullable: true),
                    MasterPlantId = table.Column<int>(nullable: true),
                    PlantTypeId = table.Column<int>(nullable: true),
                    TaxRegistrationNo = table.Column<string>(maxLength: 100, nullable: true),
                    License = table.Column<string>(maxLength: 100, nullable: true),
                    GS1Prefix = table.Column<string>(maxLength: 100, nullable: true),
                    Address1 = table.Column<string>(maxLength: 200, nullable: true),
                    Address2 = table.Column<string>(maxLength: 200, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 50, nullable: true),
                    City = table.Column<string>(maxLength: 100, nullable: true),
                    StateId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    PhoneNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Website = table.Column<string>(maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantMaster_CountryMaster_CountryId",
                        column: x => x.CountryId,
                        principalTable: "CountryMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlantMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlantMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlantMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlantMaster_PlantMaster_MasterPlantId",
                        column: x => x.MasterPlantId,
                        principalTable: "PlantMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlantMaster_StateMaster_StateId",
                        column: x => x.StateId,
                        principalTable: "StateMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AreaUsageListLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    AreaUsageHeaderId = table.Column<int>(nullable: true),
                    CheckpointId = table.Column<int>(nullable: true),
                    Observation = table.Column<string>(maxLength: 200, nullable: false),
                    DiscrepancyRemark = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaUsageListLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AreaUsageListLog_AreaUsageLog_AreaUsageHeaderId",
                        column: x => x.AreaUsageHeaderId,
                        principalTable: "AreaUsageLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AreaUsageListLog_CheckpointMaster_CheckpointId",
                        column: x => x.CheckpointId,
                        principalTable: "CheckpointMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AreaUsageListLog_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AreaUsageListLog_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AreaUsageListLog_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentUsageLogList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    EquipmentUsageHeaderId = table.Column<int>(nullable: true),
                    CheckpointId = table.Column<int>(nullable: true),
                    Observation = table.Column<string>(maxLength: 200, nullable: false),
                    DiscrepancyRemark = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentUsageLogList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentUsageLogList_CheckpointMaster_CheckpointId",
                        column: x => x.CheckpointId,
                        principalTable: "CheckpointMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentUsageLogList_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentUsageLogList_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentUsageLogList_EquipmentUsageLog_EquipmentUsageHeader~",
                        column: x => x.EquipmentUsageHeaderId,
                        principalTable: "EquipmentUsageLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentUsageLogList_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialInspectionHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    GateEntryId = table.Column<int>(nullable: true),
                    InvoiceId = table.Column<int>(nullable: true),
                    TransactionStatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialInspectionHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialInspectionHeaders_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialInspectionHeaders_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialInspectionHeaders_GateEntry_GateEntryId",
                        column: x => x.GateEntryId,
                        principalTable: "GateEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialInspectionHeaders_InvoiceDetails_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "InvoiceDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialInspectionHeaders_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialInspectionHeaders_TransactionStatusMaster_Transactio~",
                        column: x => x.TransactionStatusId,
                        principalTable: "TransactionStatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompRecipeTransDetlMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RecipeTransactiondetailId = table.Column<int>(nullable: false),
                    Operation = table.Column<string>(nullable: true),
                    ComponentId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompRecipeTransDetlMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompRecipeTransDetlMapping_MaterialMaster_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "MaterialMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompRecipeTransDetlMapping_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompRecipeTransDetlMapping_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompRecipeTransDetlMapping_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompRecipeTransDetlMapping_RecipeTransactionDetails_RecipeTr~",
                        column: x => x.RecipeTransactiondetailId,
                        principalTable: "RecipeTransactionDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModuleSubModule",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModuleId = table.Column<int>(nullable: false),
                    SubModuleId = table.Column<int>(nullable: false),
                    IsSelected = table.Column<bool>(nullable: false),
                    IsMandatory = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleSubModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleSubModule_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleSubModule_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleSubModule_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ModuleSubModule_ModuleMaster_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "ModuleMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleSubModule_SubModuleMaster_SubModuleId",
                        column: x => x.SubModuleId,
                        principalTable: "SubModuleMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<long>(nullable: true),
                    SubModuleId = table.Column<int>(nullable: false),
                    ReportSettings = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportConfiguration_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportConfiguration_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportConfiguration_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportConfiguration_SubModuleMaster_SubModuleId",
                        column: x => x.SubModuleId,
                        principalTable: "SubModuleMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatusMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(maxLength: 100, nullable: true),
                    ModuleId = table.Column<int>(nullable: false),
                    SubModuleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StatusMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatusMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatusMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatusMaster_ModuleMaster_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "ModuleMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StatusMaster_SubModuleMaster_SubModuleId",
                        column: x => x.SubModuleId,
                        principalTable: "SubModuleMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChecklistTypeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    ChecklistTypeCode = table.Column<string>(maxLength: 50, nullable: true),
                    ChecklistName = table.Column<string>(maxLength: 100, nullable: true),
                    SubPlantId = table.Column<int>(nullable: true),
                    SubModuleId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChecklistTypeMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChecklistTypeMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChecklistTypeMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChecklistTypeMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChecklistTypeMaster_SubModuleMaster_SubModuleId",
                        column: x => x.SubModuleId,
                        principalTable: "SubModuleMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChecklistTypeMaster_PlantMaster_SubPlantId",
                        column: x => x.SubPlantId,
                        principalTable: "PlantMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    SubPlantId = table.Column<int>(nullable: false),
                    DepartmentCode = table.Column<string>(maxLength: 50, nullable: false),
                    DepartmentName = table.Column<string>(maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepartmentMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepartmentMaster_PlantMaster_SubPlantId",
                        column: x => x.SubPlantId,
                        principalTable: "PlantMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GateMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    PlantId = table.Column<int>(nullable: false),
                    GateCode = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    AliasName = table.Column<string>(maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GateMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GateMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GateMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GateMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GateMaster_PlantMaster_PlantId",
                        column: x => x.PlantId,
                        principalTable: "PlantMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HandlingUnitMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    PlantId = table.Column<int>(nullable: false),
                    HUCode = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    HandlingUnitTypeId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandlingUnitMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HandlingUnitMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HandlingUnitMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HandlingUnitMaster_HandlingUnitTypeMaster_HandlingUnitTypeId",
                        column: x => x.HandlingUnitTypeId,
                        principalTable: "HandlingUnitTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HandlingUnitMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HandlingUnitMaster_PlantMaster_PlantId",
                        column: x => x.PlantId,
                        principalTable: "PlantMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PlantId = table.Column<int>(nullable: false),
                    PurchaseOrderNo = table.Column<string>(maxLength: 100, nullable: false),
                    PurchaseOrderDate = table.Column<DateTime>(nullable: false),
                    VendorName = table.Column<string>(maxLength: 100, nullable: false),
                    VendorCode = table.Column<string>(maxLength: 100, nullable: false),
                    ManufacturerName = table.Column<string>(maxLength: 100, nullable: true),
                    ManufacturerCode = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PlantMaster_PlantId",
                        column: x => x.PlantId,
                        principalTable: "PlantMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPlants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    PlantId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPlants_PlantMaster_PlantId",
                        column: x => x.PlantId,
                        principalTable: "PlantMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPlants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeighingMachineMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    WeighingMachineCode = table.Column<string>(maxLength: 50, nullable: true),
                    SubPlantId = table.Column<int>(nullable: true),
                    IPAddress = table.Column<string>(maxLength: 50, nullable: true),
                    PortNumber = table.Column<int>(nullable: true),
                    UnitOfMeasurementId = table.Column<int>(nullable: true),
                    Capacity = table.Column<float>(nullable: true),
                    Make = table.Column<string>(maxLength: 100, nullable: true),
                    Modal = table.Column<string>(maxLength: 100, nullable: true),
                    MinimumOperatingCapacity = table.Column<float>(nullable: true),
                    MaximumOperatingCapacity = table.Column<float>(nullable: true),
                    LeastCount = table.Column<string>(nullable: true),
                    LeastCountDigitAfterDecimal = table.Column<int>(nullable: true),
                    StampingDoneOn = table.Column<DateTime>(nullable: true),
                    StampingDueOn = table.Column<DateTime>(nullable: true),
                    EccentricityAcceptanceValue = table.Column<float>(nullable: true),
                    EccentricityInstruction = table.Column<string>(maxLength: 500, nullable: true),
                    LinearityAcceptanceValue = table.Column<float>(nullable: true),
                    EccentricityAcceptanceMinValue = table.Column<float>(nullable: true),
                    EccentricityAcceptanceMaxValue = table.Column<float>(nullable: true),
                    RepeatabilityAcceptanceMinValue = table.Column<float>(nullable: true),
                    RepeatabilityAcceptanceMaxValue = table.Column<float>(nullable: true),
                    LinearityAcceptanceValueWg1 = table.Column<float>(nullable: true),
                    LinearityAcceptanceValueWg2 = table.Column<float>(nullable: true),
                    LinearityAcceptanceValueWg3 = table.Column<float>(nullable: true),
                    LinearityAcceptanceValueWg4 = table.Column<float>(nullable: true),
                    LinearityAcceptanceValueWg5 = table.Column<float>(nullable: true),
                    LinearityAcceptanceMinValueWg1 = table.Column<float>(nullable: true),
                    LinearityAcceptanceMinValueWg2 = table.Column<float>(nullable: true),
                    LinearityAcceptanceMinValueWg3 = table.Column<float>(nullable: true),
                    LinearityAcceptanceMinValueWg4 = table.Column<float>(nullable: true),
                    LinearityAcceptanceMinValueWg5 = table.Column<float>(nullable: true),
                    LinearityAcceptanceMaxValueWg1 = table.Column<float>(nullable: true),
                    LinearityAcceptanceMaxValueWg2 = table.Column<float>(nullable: true),
                    LinearityAcceptanceMaxValueWg3 = table.Column<float>(nullable: true),
                    LinearityAcceptanceMaxValueWg4 = table.Column<float>(nullable: true),
                    LinearityAcceptanceMaxValueWg5 = table.Column<float>(nullable: true),
                    LinearityInstruction = table.Column<string>(maxLength: 500, nullable: true),
                    RepeatabilityAcceptanceValue = table.Column<float>(nullable: true),
                    RepeatabilityInstruction = table.Column<string>(maxLength: 500, nullable: true),
                    UncertaintyAcceptanceValue = table.Column<float>(nullable: true),
                    UncertaintyInstruction = table.Column<string>(maxLength: 500, nullable: true),
                    PercentageRSDValue = table.Column<float>(nullable: true),
                    StandardDeviationValue = table.Column<float>(nullable: true),
                    MeanValue = table.Column<float>(nullable: true),
                    MeanMinimumValue = table.Column<float>(nullable: true),
                    MeanMaximumValue = table.Column<float>(nullable: true),
                    Formula = table.Column<string>(maxLength: 100, nullable: true),
                    FrequencyTypeId = table.Column<int>(nullable: true),
                    RefrenceSOPNo = table.Column<string>(nullable: true),
                    FormatNo = table.Column<string>(nullable: true),
                    Version = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    BalancedTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeighingMachineMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeighingMachineMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeighingMachineMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeighingMachineMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeighingMachineMaster_PlantMaster_SubPlantId",
                        column: x => x.SubPlantId,
                        principalTable: "PlantMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeighingMachineMaster_UnitOfMeasurementMaster_UnitOfMeasurem~",
                        column: x => x.UnitOfMeasurementId,
                        principalTable: "UnitOfMeasurementMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModuleSubModuleId = table.Column<int>(nullable: false),
                    PermissionId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    PermissionName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_ModuleSubModule_ModuleSubModuleId",
                        column: x => x.ModuleSubModuleId,
                        principalTable: "ModuleSubModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_PermissionMaster_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "PermissionMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CubicleAssignmentHeader",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    GroupId = table.Column<string>(maxLength: 100, nullable: false),
                    CubicleAssignmentDate = table.Column<DateTime>(nullable: false),
                    ProductCode = table.Column<string>(maxLength: 100, nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    GroupStatusId = table.Column<int>(nullable: true),
                    IsSampling = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CubicleAssignmentHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CubicleAssignmentHeader_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleAssignmentHeader_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleAssignmentHeader_StatusMaster_GroupStatusId",
                        column: x => x.GroupStatusId,
                        principalTable: "StatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleAssignmentHeader_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentCleaningStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CleaningDate = table.Column<DateTime>(nullable: false),
                    EquipmentId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    IsSampling = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCleaningStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentCleaningStatus_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentCleaningStatus_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentCleaningStatus_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentCleaningStatus_StatusMaster_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentCleaningTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CleaningDate = table.Column<DateTime>(nullable: false),
                    EquipmentId = table.Column<int>(nullable: false),
                    CubicleId = table.Column<int>(nullable: true),
                    AreaId = table.Column<int>(nullable: true),
                    CleaningTypeId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    StopTime = table.Column<DateTime>(nullable: true),
                    VerifiedTime = table.Column<DateTime>(nullable: true),
                    CleanerId = table.Column<int>(nullable: false),
                    VerifierId = table.Column<int>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    DoneBy = table.Column<string>(nullable: true),
                    IsSampling = table.Column<bool>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCleaningTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentCleaningTransactions_EquipmentCleaningTypeMaster_Cl~",
                        column: x => x.CleaningTypeId,
                        principalTable: "EquipmentCleaningTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentCleaningTransactions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentCleaningTransactions_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentCleaningTransactions_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentCleaningTransactions_StatusMaster_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnToVendorHeader",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MaterialDocumentNo = table.Column<string>(maxLength: 100, nullable: false),
                    MaterialCode = table.Column<string>(maxLength: 100, nullable: false),
                    SAPBatchNumber = table.Column<string>(maxLength: 100, nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    Qty = table.Column<float>(nullable: true),
                    ARNo = table.Column<string>(maxLength: 100, nullable: true),
                    UOM = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnToVendorHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnToVendorHeader_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReturnToVendorHeader_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReturnToVendorHeader_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReturnToVendorHeader_StatusMaster_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleInspectionHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    GateEntryId = table.Column<int>(nullable: true),
                    InspectionDate = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    InvoiceId = table.Column<int>(nullable: true),
                    ChecklistTypeId = table.Column<int>(nullable: true),
                    InspectionChecklistId = table.Column<int>(nullable: true),
                    TransactionStatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleInspectionHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleInspectionHeaders_ChecklistTypeMaster_ChecklistTypeId",
                        column: x => x.ChecklistTypeId,
                        principalTable: "ChecklistTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleInspectionHeaders_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleInspectionHeaders_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleInspectionHeaders_GateEntry_GateEntryId",
                        column: x => x.GateEntryId,
                        principalTable: "GateEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleInspectionHeaders_InspectionChecklistMaster_Inspectio~",
                        column: x => x.InspectionChecklistId,
                        principalTable: "InspectionChecklistMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleInspectionHeaders_InvoiceDetails_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "InvoiceDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleInspectionHeaders_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleInspectionHeaders_TransactionStatusMaster_Transaction~",
                        column: x => x.TransactionStatusId,
                        principalTable: "TransactionStatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AreaMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    SubPlantId = table.Column<int>(nullable: false),
                    DepartmentId = table.Column<int>(nullable: false),
                    AreaCode = table.Column<string>(maxLength: 50, nullable: false),
                    AreaName = table.Column<string>(maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Zone = table.Column<string>(maxLength: 100, nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AreaMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AreaMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AreaMaster_DepartmentMaster_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DepartmentMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AreaMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AreaMaster_PlantMaster_SubPlantId",
                        column: x => x.SubPlantId,
                        principalTable: "PlantMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    PlantId = table.Column<int>(nullable: false),
                    SLOCId = table.Column<int>(nullable: true),
                    EquipmentTypeId = table.Column<int>(nullable: true),
                    EquipmentCode = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Alias = table.Column<string>(maxLength: 100, nullable: true),
                    EquipmentModel = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsPortable = table.Column<bool>(nullable: true),
                    DateOfProcurement = table.Column<DateTime>(nullable: true),
                    DateOfInstallation = table.Column<DateTime>(nullable: true),
                    IsMaintenanceRequired = table.Column<bool>(nullable: true),
                    MaintenanceScheduleDays = table.Column<int>(nullable: true),
                    CommunicationType = table.Column<int>(nullable: true),
                    VendorName = table.Column<string>(maxLength: 100, nullable: true),
                    VendorDocumentNumber = table.Column<string>(maxLength: 100, nullable: true),
                    SupportExpiresOn = table.Column<DateTime>(nullable: true),
                    NetworkIPAddress = table.Column<string>(maxLength: 50, nullable: true),
                    NetworkIPPort = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    CleanHoldTime = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMaster_EquipmentTypeMaster_EquipmentTypeId",
                        column: x => x.EquipmentTypeId,
                        principalTable: "EquipmentTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentMaster_PlantMaster_PlantId",
                        column: x => x.PlantId,
                        principalTable: "PlantMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentMaster_DepartmentMaster_SLOCId",
                        column: x => x.SLOCId,
                        principalTable: "DepartmentMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Materials",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PurchaseOrderId = table.Column<int>(nullable: false),
                    PurchaseOrderNo = table.Column<string>(maxLength: 100, nullable: false),
                    ItemNo = table.Column<string>(maxLength: 100, nullable: false),
                    ItemCode = table.Column<string>(maxLength: 100, nullable: false),
                    ItemDescription = table.Column<string>(nullable: true),
                    OrderQuantity = table.Column<float>(nullable: false),
                    UnitOfMeasurement = table.Column<string>(maxLength: 100, nullable: true),
                    BalanceQuantity = table.Column<float>(nullable: true),
                    ManufacturerName = table.Column<string>(maxLength: 100, nullable: true),
                    ManufacturerCode = table.Column<string>(maxLength: 100, nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materials_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materials_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materials_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Materials_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalibrationFrequencyMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    WeighingMachineId = table.Column<int>(nullable: true),
                    FrequencyTypeId = table.Column<int>(nullable: true),
                    CalibrationLevel = table.Column<string>(maxLength: 100, nullable: true),
                    CalibrationCriteria = table.Column<string>(nullable: true),
                    StandardWeightValue = table.Column<float>(nullable: true),
                    MinimumValue = table.Column<float>(nullable: true),
                    MaximumValue = table.Column<float>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalibrationFrequencyMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalibrationFrequencyMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalibrationFrequencyMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalibrationFrequencyMaster_FrequencyTypeMaster_FrequencyType~",
                        column: x => x.FrequencyTypeId,
                        principalTable: "FrequencyTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalibrationFrequencyMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalibrationFrequencyMaster_WeighingMachineMaster_WeighingMac~",
                        column: x => x.WeighingMachineId,
                        principalTable: "WeighingMachineMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SampleDestructions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    InspectionLotId = table.Column<int>(nullable: false),
                    MaterialCode = table.Column<string>(maxLength: 100, nullable: false),
                    SAPBatchNumber = table.Column<string>(maxLength: 100, nullable: false),
                    UnitOfMeasurementId = table.Column<int>(nullable: true),
                    ContainerMaterialBarcode = table.Column<string>(maxLength: 100, nullable: false),
                    NoOfPacks = table.Column<int>(nullable: true),
                    GrossWeight = table.Column<float>(nullable: true),
                    TareWeight = table.Column<float>(nullable: true),
                    NetWeight = table.Column<float>(nullable: true),
                    WeighingMachineId = table.Column<int>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleDestructions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SampleDestructions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SampleDestructions_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SampleDestructions_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SampleDestructions_UnitOfMeasurementMaster_UnitOfMeasurement~",
                        column: x => x.UnitOfMeasurementId,
                        principalTable: "UnitOfMeasurementMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SampleDestructions_WeighingMachineMaster_WeighingMachineId",
                        column: x => x.WeighingMachineId,
                        principalTable: "WeighingMachineMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WeighingMachineTestConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    WeighingMachineId = table.Column<int>(nullable: true),
                    FrequencyTypeId = table.Column<int>(nullable: true),
                    IsEccentricityTestRequired = table.Column<bool>(nullable: true),
                    IsLinearityTestRequired = table.Column<bool>(nullable: true),
                    IsRepeatabilityTestRequired = table.Column<bool>(nullable: true),
                    IsUncertainityTestRequired = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeighingMachineTestConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeighingMachineTestConfigurations_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeighingMachineTestConfigurations_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeighingMachineTestConfigurations_FrequencyTypeMaster_Freque~",
                        column: x => x.FrequencyTypeId,
                        principalTable: "FrequencyTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeighingMachineTestConfigurations_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeighingMachineTestConfigurations_WeighingMachineMaster_Weig~",
                        column: x => x.WeighingMachineId,
                        principalTable: "WeighingMachineMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WMCalibrationHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    WeighingMachineId = table.Column<int>(nullable: true),
                    CalibrationFrequencyId = table.Column<int>(nullable: true),
                    ChecklistTypeId = table.Column<int>(nullable: true),
                    InspectionChecklistId = table.Column<int>(nullable: true),
                    CalibrationStatusId = table.Column<int>(nullable: true),
                    IsReCalibrated = table.Column<bool>(nullable: true),
                    InitialZeroReading = table.Column<string>(nullable: true),
                    ReCalibrationRemark = table.Column<string>(nullable: true),
                    CalibrationTestDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WMCalibrationHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WMCalibrationHeaders_FrequencyTypeMaster_CalibrationFrequenc~",
                        column: x => x.CalibrationFrequencyId,
                        principalTable: "FrequencyTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationHeaders_CalibrationStatusMaster_CalibrationStat~",
                        column: x => x.CalibrationStatusId,
                        principalTable: "CalibrationStatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationHeaders_ChecklistTypeMaster_ChecklistTypeId",
                        column: x => x.ChecklistTypeId,
                        principalTable: "ChecklistTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationHeaders_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationHeaders_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationHeaders_InspectionChecklistMaster_InspectionChe~",
                        column: x => x.InspectionChecklistId,
                        principalTable: "InspectionChecklistMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationHeaders_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationHeaders_WeighingMachineMaster_WeighingMachineId",
                        column: x => x.WeighingMachineId,
                        principalTable: "WeighingMachineMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentCleaningCheckpoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CheckPointId = table.Column<int>(nullable: false),
                    Observation = table.Column<string>(maxLength: 200, nullable: true),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    EquipmentCleaningTransactionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentCleaningCheckpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentCleaningCheckpoints_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentCleaningCheckpoints_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentCleaningCheckpoints_EquipmentCleaningTransactions_E~",
                        column: x => x.EquipmentCleaningTransactionId,
                        principalTable: "EquipmentCleaningTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquipmentCleaningCheckpoints_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ReturnToVendorDetail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ReturnToVendorHeaderId = table.Column<int>(nullable: true),
                    ContainerMaterialBarcode = table.Column<string>(maxLength: 100, nullable: false),
                    UOM = table.Column<string>(maxLength: 100, nullable: true),
                    Qty = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnToVendorDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnToVendorDetail_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReturnToVendorDetail_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReturnToVendorDetail_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReturnToVendorDetail_ReturnToVendorHeader_ReturnToVendorHead~",
                        column: x => x.ReturnToVendorHeaderId,
                        principalTable: "ReturnToVendorHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VehicleInspectionDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    VehicleInspectionHeaderId = table.Column<int>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    CheckpointId = table.Column<int>(nullable: true),
                    Observation = table.Column<string>(maxLength: 200, nullable: false),
                    DiscrepancyRemark = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleInspectionDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleInspectionDetails_CheckpointMaster_CheckpointId",
                        column: x => x.CheckpointId,
                        principalTable: "CheckpointMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleInspectionDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleInspectionDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleInspectionDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleInspectionDetails_VehicleInspectionHeaders_VehicleIns~",
                        column: x => x.VehicleInspectionHeaderId,
                        principalTable: "VehicleInspectionHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CubicleMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    PlantId = table.Column<int>(nullable: false),
                    CubicleCode = table.Column<string>(maxLength: 50, nullable: true),
                    AreaId = table.Column<int>(nullable: false),
                    SLOCId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CubicleMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CubicleMaster_AreaMaster_AreaId",
                        column: x => x.AreaId,
                        principalTable: "AreaMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CubicleMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleMaster_PlantMaster_PlantId",
                        column: x => x.PlantId,
                        principalTable: "PlantMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CubicleMaster_DepartmentMaster_SLOCId",
                        column: x => x.SLOCId,
                        principalTable: "DepartmentMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocationMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    LocationCode = table.Column<string>(maxLength: 50, nullable: true),
                    StorageLocationType = table.Column<string>(maxLength: 100, nullable: true),
                    PlantId = table.Column<int>(nullable: false),
                    DepartmentId = table.Column<int>(nullable: false),
                    AreaId = table.Column<int>(nullable: false),
                    Zone = table.Column<string>(maxLength: 100, nullable: true),
                    LocationTemperature = table.Column<decimal>(nullable: true),
                    LocationTemperatureUL = table.Column<decimal>(nullable: true),
                    TemperatureUnit = table.Column<int>(nullable: true),
                    SLOCType = table.Column<string>(maxLength: 100, nullable: true),
                    LevelId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationMaster_AreaMaster_AreaId",
                        column: x => x.AreaId,
                        principalTable: "AreaMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LocationMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocationMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocationMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocationMaster_PlantMaster_PlantId",
                        column: x => x.PlantId,
                        principalTable: "PlantMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StandardWeightBoxMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    SubPlantId = table.Column<int>(nullable: false),
                    StandardWeightBoxId = table.Column<string>(maxLength: 100, nullable: false),
                    AreaId = table.Column<int>(nullable: false),
                    DepartmentId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardWeightBoxMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StandardWeightBoxMaster_AreaMaster_AreaId",
                        column: x => x.AreaId,
                        principalTable: "AreaMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StandardWeightBoxMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StandardWeightBoxMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StandardWeightBoxMaster_DepartmentMaster_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DepartmentMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StandardWeightBoxMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StandardWeightBoxMaster_PlantMaster_SubPlantId",
                        column: x => x.SubPlantId,
                        principalTable: "PlantMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DispensingHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RLAFId = table.Column<int>(nullable: false),
                    ProcessOrderId = table.Column<int>(nullable: true),
                    InspectionLotId = table.Column<int>(nullable: true),
                    MaterialCodeId = table.Column<string>(maxLength: 100, nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    IsSampling = table.Column<bool>(nullable: false),
                    CheckedBy = table.Column<string>(nullable: true),
                    DoneBy = table.Column<int>(nullable: true),
                    CheckedById = table.Column<int>(nullable: true),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispensingHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispensingHeaders_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensingHeaders_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensingHeaders_InspectionLot_InspectionLotId",
                        column: x => x.InspectionLotId,
                        principalTable: "InspectionLot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensingHeaders_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensingHeaders_ProcessOrders_ProcessOrderId",
                        column: x => x.ProcessOrderId,
                        principalTable: "ProcessOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensingHeaders_EquipmentMaster_RLAFId",
                        column: x => x.RLAFId,
                        principalTable: "EquipmentMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DispensingHeaders_StatusMaster_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialInspectionRelationDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    MaterialHeaderId = table.Column<int>(nullable: false),
                    ChecklistTypeId = table.Column<int>(nullable: true),
                    InspectionChecklistId = table.Column<int>(nullable: true),
                    TransactionStatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialInspectionRelationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialInspectionRelationDetails_ChecklistTypeMaster_Checkl~",
                        column: x => x.ChecklistTypeId,
                        principalTable: "ChecklistTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialInspectionRelationDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialInspectionRelationDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialInspectionRelationDetails_InspectionChecklistMaster_~",
                        column: x => x.InspectionChecklistId,
                        principalTable: "InspectionChecklistMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialInspectionRelationDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialInspectionRelationDetails_MaterialInspectionHeaders_~",
                        column: x => x.MaterialHeaderId,
                        principalTable: "MaterialInspectionHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialInspectionRelationDetails_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialInspectionRelationDetails_TransactionStatusMaster_Tr~",
                        column: x => x.TransactionStatusId,
                        principalTable: "TransactionStatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WMCalibratedLatestMachineDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    WMCalibrationHeaderId = table.Column<int>(nullable: true),
                    WeighingMachineId = table.Column<int>(nullable: true),
                    LastCalibrationTestDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WMCalibratedLatestMachineDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WMCalibratedLatestMachineDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibratedLatestMachineDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibratedLatestMachineDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibratedLatestMachineDetails_WMCalibrationHeaders_WMCali~",
                        column: x => x.WMCalibrationHeaderId,
                        principalTable: "WMCalibrationHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibratedLatestMachineDetails_WeighingMachineMaster_Weigh~",
                        column: x => x.WeighingMachineId,
                        principalTable: "WeighingMachineMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WMCalibrationCheckpoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CheckPointId = table.Column<int>(nullable: true),
                    Observation = table.Column<string>(maxLength: 200, nullable: true),
                    DiscrepancyRemark = table.Column<string>(maxLength: 200, nullable: true),
                    WMCalibrationHeaderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WMCalibrationCheckpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WMCalibrationCheckpoints_CheckpointMaster_CheckPointId",
                        column: x => x.CheckPointId,
                        principalTable: "CheckpointMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationCheckpoints_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationCheckpoints_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationCheckpoints_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationCheckpoints_WMCalibrationHeaders_WMCalibrationH~",
                        column: x => x.WMCalibrationHeaderId,
                        principalTable: "WMCalibrationHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WMCalibrationUncertainityTests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    WMCalibrationHeaderId = table.Column<int>(nullable: true),
                    UncertainityValue = table.Column<double>(nullable: false),
                    TestResultId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WMCalibrationUncertainityTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WMCalibrationUncertainityTests_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationUncertainityTests_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationUncertainityTests_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationUncertainityTests_CalibrationTestStatusMaster_T~",
                        column: x => x.TestResultId,
                        principalTable: "CalibrationTestStatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationUncertainityTests_WMCalibrationHeaders_WMCalibr~",
                        column: x => x.WMCalibrationHeaderId,
                        principalTable: "WMCalibrationHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CubicalRecipeTranDetlMapping",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RecipeTransactiondetailId = table.Column<int>(nullable: false),
                    Operation = table.Column<string>(nullable: true),
                    CubicalId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CubicalRecipeTranDetlMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CubicalRecipeTranDetlMapping_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicalRecipeTranDetlMapping_CubicleMaster_CubicalId",
                        column: x => x.CubicalId,
                        principalTable: "CubicleMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CubicalRecipeTranDetlMapping_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicalRecipeTranDetlMapping_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicalRecipeTranDetlMapping_RecipeTransactionDetails_Recipe~",
                        column: x => x.RecipeTransactiondetailId,
                        principalTable: "RecipeTransactionDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CubicleAssignmentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CubicleAssignmentHeaderId = table.Column<int>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    ProcessOrderId = table.Column<int>(nullable: true),
                    ProcessOrderMaterialId = table.Column<int>(nullable: true),
                    CubicleId = table.Column<int>(nullable: true),
                    StatusId = table.Column<int>(nullable: true),
                    InspectionLotId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CubicleAssignmentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CubicleAssignmentDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleAssignmentDetails_CubicleAssignmentHeader_CubicleAssi~",
                        column: x => x.CubicleAssignmentHeaderId,
                        principalTable: "CubicleAssignmentHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleAssignmentDetails_CubicleMaster_CubicleId",
                        column: x => x.CubicleId,
                        principalTable: "CubicleMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleAssignmentDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleAssignmentDetails_InspectionLot_InspectionLotId",
                        column: x => x.InspectionLotId,
                        principalTable: "InspectionLot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleAssignmentDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleAssignmentDetails_ProcessOrders_ProcessOrderId",
                        column: x => x.ProcessOrderId,
                        principalTable: "ProcessOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleAssignmentDetails_ProcessOrderMaterials_ProcessOrderM~",
                        column: x => x.ProcessOrderMaterialId,
                        principalTable: "ProcessOrderMaterials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleAssignmentDetails_StatusMaster_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CubicleCleaningDailyStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CleaningDate = table.Column<DateTime>(nullable: false),
                    CubicleId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    IsSampling = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CubicleCleaningDailyStatus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningDailyStatus_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningDailyStatus_CubicleMaster_CubicleId",
                        column: x => x.CubicleId,
                        principalTable: "CubicleMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningDailyStatus_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningDailyStatus_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningDailyStatus_StatusMaster_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CubicleCleaningTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CleaningDate = table.Column<DateTime>(nullable: false),
                    CubicleId = table.Column<int>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    StopTime = table.Column<DateTime>(nullable: true),
                    VerifiedTime = table.Column<DateTime>(nullable: true),
                    CleanerId = table.Column<int>(nullable: false),
                    VerifierId = table.Column<int>(nullable: true),
                    DoneBy = table.Column<string>(nullable: true),
                    IsSampling = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CubicleCleaningTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningTransactions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningTransactions_CubicleMaster_CubicleId",
                        column: x => x.CubicleId,
                        principalTable: "CubicleMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningTransactions_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningTransactions_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningTransactions_StatusMaster_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningTransactions_CubicleCleaningTypeMaster_TypeId",
                        column: x => x.TypeId,
                        principalTable: "CubicleCleaningTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeviceMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    SubPlantId = table.Column<int>(nullable: false),
                    DeviceId = table.Column<string>(maxLength: 50, nullable: true),
                    DeviceTypeId = table.Column<int>(nullable: true),
                    Make = table.Column<string>(maxLength: 100, nullable: true),
                    Model = table.Column<string>(maxLength: 100, nullable: true),
                    SerialNo = table.Column<string>(maxLength: 100, nullable: true),
                    IpAddress = table.Column<string>(maxLength: 50, nullable: true),
                    Port = table.Column<int>(nullable: true),
                    DepartmentId = table.Column<int>(nullable: true),
                    AreaId = table.Column<int>(nullable: true),
                    CubicleId = table.Column<int>(nullable: true),
                    ModeId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceMaster_AreaMaster_AreaId",
                        column: x => x.AreaId,
                        principalTable: "AreaMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeviceMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeviceMaster_CubicleMaster_CubicleId",
                        column: x => x.CubicleId,
                        principalTable: "CubicleMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeviceMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeviceMaster_DepartmentMaster_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DepartmentMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeviceMaster_DeviceTypeMaster_DeviceTypeId",
                        column: x => x.DeviceTypeId,
                        principalTable: "DeviceTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeviceMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeviceMaster_ModeMaster_ModeId",
                        column: x => x.ModeId,
                        principalTable: "ModeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    EquipmentId = table.Column<int>(nullable: true),
                    Cubicleid = table.Column<int>(nullable: true),
                    CubicleAssignmentHeaderId = table.Column<int>(nullable: true),
                    GroupId = table.Column<string>(nullable: true),
                    IsSampling = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquipmentAssignments_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentAssignments_CubicleAssignmentHeader_CubicleAssignme~",
                        column: x => x.CubicleAssignmentHeaderId,
                        principalTable: "CubicleAssignmentHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentAssignments_CubicleMaster_Cubicleid",
                        column: x => x.Cubicleid,
                        principalTable: "CubicleMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentAssignments_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentAssignments_EquipmentMaster_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "EquipmentMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentAssignments_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LineClearanceTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ClearanceDate = table.Column<DateTime>(nullable: false),
                    CubicleId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    StopTime = table.Column<DateTime>(nullable: true),
                    VerifiedBy = table.Column<int>(nullable: true),
                    ApprovedBy = table.Column<int>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    IsSampling = table.Column<bool>(nullable: false),
                    ApprovedTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineClearanceTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineClearanceTransactions_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LineClearanceTransactions_CubicleMaster_CubicleId",
                        column: x => x.CubicleId,
                        principalTable: "CubicleMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineClearanceTransactions_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LineClearanceTransactions_CubicleAssignmentHeader_GroupId",
                        column: x => x.GroupId,
                        principalTable: "CubicleAssignmentHeader",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineClearanceTransactions_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LineClearanceTransactions_StatusMaster_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StageOutHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CubicleId = table.Column<int>(nullable: false),
                    GroupId = table.Column<string>(maxLength: 100, nullable: true),
                    InspectionLotId = table.Column<int>(maxLength: 100, nullable: true),
                    MaterialCode = table.Column<string>(maxLength: 50, nullable: true),
                    StatusId = table.Column<int>(nullable: false),
                    IsSampling = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageOutHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StageOutHeaders_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StageOutHeaders_CubicleMaster_CubicleId",
                        column: x => x.CubicleId,
                        principalTable: "CubicleMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StageOutHeaders_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StageOutHeaders_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StageOutHeaders_StatusMaster_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StandardWeightMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ApprovalStatusId = table.Column<int>(nullable: false),
                    ApprovalStatusDescription = table.Column<string>(nullable: true),
                    SubPlantId = table.Column<int>(nullable: false),
                    StandardWeightId = table.Column<string>(maxLength: 100, nullable: false),
                    Capacity = table.Column<float>(nullable: true),
                    CapacityinDecimal = table.Column<string>(nullable: true),
                    StampingDoneOn = table.Column<DateTime>(nullable: false),
                    StampingDueOn = table.Column<DateTime>(nullable: false),
                    AreaId = table.Column<int>(nullable: false),
                    DepartmentId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: true),
                    StandardWeightBoxMasterId = table.Column<int>(nullable: true),
                    UnitOfMeasurementId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StandardWeightMaster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StandardWeightMaster_AreaMaster_AreaId",
                        column: x => x.AreaId,
                        principalTable: "AreaMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StandardWeightMaster_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StandardWeightMaster_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StandardWeightMaster_DepartmentMaster_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "DepartmentMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StandardWeightMaster_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StandardWeightMaster_StandardWeightBoxMaster_StandardWeightB~",
                        column: x => x.StandardWeightBoxMasterId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StandardWeightMaster_PlantMaster_SubPlantId",
                        column: x => x.SubPlantId,
                        principalTable: "PlantMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StandardWeightMaster_UnitOfMeasurementMaster_UnitOfMeasureme~",
                        column: x => x.UnitOfMeasurementId,
                        principalTable: "UnitOfMeasurementMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WMCalibrationDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    WMCalibrationHeaderId = table.Column<int>(nullable: true),
                    CalibrationLevelId = table.Column<int>(nullable: true),
                    StandardWeightBoxId = table.Column<int>(nullable: true),
                    CapturedWeight = table.Column<double>(nullable: false),
                    Remark = table.Column<string>(maxLength: 100, nullable: true),
                    DoneBy = table.Column<string>(nullable: true),
                    CheckedBy = table.Column<string>(nullable: true),
                    SpriritLevelBubble = table.Column<string>(nullable: true),
                    CalibrationStatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WMCalibrationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WMCalibrationDetails_CalibrationFrequencyMaster_CalibrationL~",
                        column: x => x.CalibrationLevelId,
                        principalTable: "CalibrationFrequencyMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationDetails_CalibrationStatusMaster_CalibrationStat~",
                        column: x => x.CalibrationStatusId,
                        principalTable: "CalibrationStatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationDetails_StandardWeightBoxMaster_StandardWeightB~",
                        column: x => x.StandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationDetails_WMCalibrationHeaders_WMCalibrationHeade~",
                        column: x => x.WMCalibrationHeaderId,
                        principalTable: "WMCalibrationHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WMCalibrationEccentricityTests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    WMCalibrationHeaderId = table.Column<int>(nullable: true),
                    CalculatedCapacityWeight = table.Column<double>(nullable: false),
                    InitialZeroReading = table.Column<string>(nullable: true),
                    CValue = table.Column<double>(nullable: false),
                    LFValue = table.Column<double>(nullable: false),
                    RFValue = table.Column<double>(nullable: false),
                    LBValue = table.Column<double>(nullable: false),
                    RBValue = table.Column<double>(nullable: false),
                    DoneBy = table.Column<string>(nullable: true),
                    CheckedBy = table.Column<string>(nullable: true),
                    SpriritLevelBubble = table.Column<string>(nullable: true),
                    CValueStandardWeightBoxId = table.Column<int>(nullable: true),
                    LFValueStandardWeightBoxId = table.Column<int>(nullable: true),
                    RFValueStandardWeightBoxId = table.Column<int>(nullable: true),
                    LBValueStandardWeightBoxId = table.Column<int>(nullable: true),
                    RBValueStandardWeightBoxId = table.Column<int>(nullable: true),
                    MeanValue = table.Column<double>(nullable: false),
                    StandardDeviationValue = table.Column<double>(nullable: false),
                    PRSDValue = table.Column<double>(nullable: false),
                    TestResultId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WMCalibrationEccentricityTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WMCalibrationEccentricityTests_StandardWeightBoxMaster_CValu~",
                        column: x => x.CValueStandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationEccentricityTests_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationEccentricityTests_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationEccentricityTests_StandardWeightBoxMaster_LBVal~",
                        column: x => x.LBValueStandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationEccentricityTests_StandardWeightBoxMaster_LFVal~",
                        column: x => x.LFValueStandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationEccentricityTests_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationEccentricityTests_StandardWeightBoxMaster_RBVal~",
                        column: x => x.RBValueStandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationEccentricityTests_StandardWeightBoxMaster_RFVal~",
                        column: x => x.RFValueStandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationEccentricityTests_CalibrationTestStatusMaster_T~",
                        column: x => x.TestResultId,
                        principalTable: "CalibrationTestStatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationEccentricityTests_WMCalibrationHeaders_WMCalibr~",
                        column: x => x.WMCalibrationHeaderId,
                        principalTable: "WMCalibrationHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WMCalibrationLinearityTests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    WMCalibrationHeaderId = table.Column<int>(nullable: true),
                    InitialZeroReading = table.Column<string>(nullable: true),
                    WeightValue1 = table.Column<double>(nullable: false),
                    WeightValue2 = table.Column<double>(nullable: false),
                    WeightValue3 = table.Column<double>(nullable: false),
                    WeightValue4 = table.Column<double>(nullable: false),
                    WeightValue5 = table.Column<double>(nullable: false),
                    DoneBy = table.Column<string>(nullable: true),
                    CheckedBy = table.Column<string>(nullable: true),
                    SpriritLevelBubble = table.Column<string>(nullable: true),
                    WeightValue1StandardWeightBoxId = table.Column<int>(nullable: true),
                    WeightValue2StandardWeightBoxId = table.Column<int>(nullable: true),
                    WeightValue3StandardWeightBoxId = table.Column<int>(nullable: true),
                    WeightValue4StandardWeightBoxId = table.Column<int>(nullable: true),
                    WeightValue5StandardWeightBoxId = table.Column<int>(nullable: true),
                    MeanValue = table.Column<double>(nullable: false),
                    StandardDeviationValue = table.Column<double>(nullable: false),
                    PRSDValue = table.Column<double>(nullable: false),
                    TestResultId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WMCalibrationLinearityTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WMCalibrationLinearityTests_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationLinearityTests_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationLinearityTests_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationLinearityTests_CalibrationTestStatusMaster_Test~",
                        column: x => x.TestResultId,
                        principalTable: "CalibrationTestStatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationLinearityTests_WMCalibrationHeaders_WMCalibrati~",
                        column: x => x.WMCalibrationHeaderId,
                        principalTable: "WMCalibrationHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationLinearityTests_StandardWeightBoxMaster_WeightVa~",
                        column: x => x.WeightValue1StandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationLinearityTests_StandardWeightBoxMaster_WeightV~1",
                        column: x => x.WeightValue2StandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationLinearityTests_StandardWeightBoxMaster_WeightV~2",
                        column: x => x.WeightValue3StandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationLinearityTests_StandardWeightBoxMaster_WeightV~3",
                        column: x => x.WeightValue4StandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationLinearityTests_StandardWeightBoxMaster_WeightV~4",
                        column: x => x.WeightValue5StandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WMCalibrationRepeatabilityTests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    WMCalibrationHeaderId = table.Column<int>(nullable: true),
                    InitialZeroReading = table.Column<string>(nullable: true),
                    CalculatedCapacityWeight = table.Column<double>(nullable: false),
                    WeightValue1 = table.Column<double>(nullable: false),
                    WeightValue2 = table.Column<double>(nullable: false),
                    WeightValue3 = table.Column<double>(nullable: false),
                    WeightValue4 = table.Column<double>(nullable: false),
                    WeightValue5 = table.Column<double>(nullable: false),
                    WeightValue6 = table.Column<double>(nullable: false),
                    WeightValue7 = table.Column<double>(nullable: false),
                    WeightValue8 = table.Column<double>(nullable: false),
                    WeightValue9 = table.Column<double>(nullable: false),
                    WeightValue10 = table.Column<double>(nullable: false),
                    DoneBy = table.Column<string>(nullable: true),
                    CheckedBy = table.Column<string>(nullable: true),
                    SpriritLevelBubble = table.Column<string>(nullable: true),
                    WeightValue1StandardWeightBoxId = table.Column<int>(nullable: true),
                    WeightValue2StandardWeightBoxId = table.Column<int>(nullable: true),
                    WeightValue3StandardWeightBoxId = table.Column<int>(nullable: true),
                    WeightValue4StandardWeightBoxId = table.Column<int>(nullable: true),
                    WeightValue5StandardWeightBoxId = table.Column<int>(nullable: true),
                    WeightValue6StandardWeightBoxId = table.Column<int>(nullable: true),
                    WeightValue7StandardWeightBoxId = table.Column<int>(nullable: true),
                    WeightValue8StandardWeightBoxId = table.Column<int>(nullable: true),
                    WeightValue9StandardWeightBoxId = table.Column<int>(nullable: true),
                    WeightValue10StandardWeightBoxId = table.Column<int>(nullable: true),
                    MeanValue = table.Column<double>(nullable: false),
                    StandardDeviationValue = table.Column<double>(nullable: false),
                    PRSDValue = table.Column<double>(nullable: false),
                    TestResultId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WMCalibrationRepeatabilityTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WMCalibrationRepeatabilityTests_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationRepeatabilityTests_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationRepeatabilityTests_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationRepeatabilityTests_CalibrationTestStatusMaster_~",
                        column: x => x.TestResultId,
                        principalTable: "CalibrationTestStatusMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationRepeatabilityTests_WMCalibrationHeaders_WMCalib~",
                        column: x => x.WMCalibrationHeaderId,
                        principalTable: "WMCalibrationHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationRepeatabilityTests_StandardWeightBoxMaster_Weig~",
                        column: x => x.WeightValue10StandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationRepeatabilityTests_StandardWeightBoxMaster_Wei~1",
                        column: x => x.WeightValue1StandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationRepeatabilityTests_StandardWeightBoxMaster_Wei~2",
                        column: x => x.WeightValue2StandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationRepeatabilityTests_StandardWeightBoxMaster_Wei~3",
                        column: x => x.WeightValue3StandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationRepeatabilityTests_StandardWeightBoxMaster_Wei~4",
                        column: x => x.WeightValue4StandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationRepeatabilityTests_StandardWeightBoxMaster_Wei~5",
                        column: x => x.WeightValue5StandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationRepeatabilityTests_StandardWeightBoxMaster_Wei~6",
                        column: x => x.WeightValue6StandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationRepeatabilityTests_StandardWeightBoxMaster_Wei~7",
                        column: x => x.WeightValue7StandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationRepeatabilityTests_StandardWeightBoxMaster_Wei~8",
                        column: x => x.WeightValue8StandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationRepeatabilityTests_StandardWeightBoxMaster_Wei~9",
                        column: x => x.WeightValue9StandardWeightBoxId,
                        principalTable: "StandardWeightBoxMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DispensingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DispensingHeaderId = table.Column<int>(nullable: true),
                    SAPBatchNumber = table.Column<string>(maxLength: 100, nullable: false),
                    ContainerMaterialBarcode = table.Column<string>(maxLength: 100, nullable: false),
                    DispenseBarcode = table.Column<string>(maxLength: 100, nullable: false),
                    UnitOfMeasurementId = table.Column<int>(nullable: true),
                    NoOfPacks = table.Column<int>(nullable: true),
                    IsGrossWeight = table.Column<bool>(nullable: false),
                    GrossWeight = table.Column<float>(nullable: true),
                    TareWeight = table.Column<float>(nullable: true),
                    NetWeight = table.Column<float>(nullable: true),
                    WeighingMachineId = table.Column<int>(nullable: true),
                    SamplingTypeId = table.Column<int>(nullable: true),
                    DoneBy = table.Column<int>(nullable: true),
                    CheckedById = table.Column<int>(nullable: true),
                    Printed = table.Column<bool>(nullable: false),
                    NoOfContainers = table.Column<int>(nullable: false),
                    ContainerNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispensingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispensingDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensingDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensingDetails_DispensingHeaders_DispensingHeaderId",
                        column: x => x.DispensingHeaderId,
                        principalTable: "DispensingHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensingDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensingDetails_SamplingTypeMaster_SamplingTypeId",
                        column: x => x.SamplingTypeId,
                        principalTable: "SamplingTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensingDetails_UnitOfMeasurementMaster_UnitOfMeasurementId",
                        column: x => x.UnitOfMeasurementId,
                        principalTable: "UnitOfMeasurementMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensingDetails_WeighingMachineMaster_WeighingMachineId",
                        column: x => x.WeighingMachineId,
                        principalTable: "WeighingMachineMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialChecklistDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CheckPointId = table.Column<int>(nullable: true),
                    Observation = table.Column<string>(maxLength: 200, nullable: true),
                    DiscrepancyRemark = table.Column<string>(maxLength: 200, nullable: true),
                    MaterialRelationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialChecklistDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialChecklistDetails_CheckpointMaster_CheckPointId",
                        column: x => x.CheckPointId,
                        principalTable: "CheckpointMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialChecklistDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialChecklistDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialChecklistDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialChecklistDetails_MaterialInspectionRelationDetails_M~",
                        column: x => x.MaterialRelationId,
                        principalTable: "MaterialInspectionRelationDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialConsignmentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ManufacturedBatchNo = table.Column<string>(maxLength: 100, nullable: true),
                    ManufacturedDate = table.Column<DateTime>(nullable: true),
                    ExpiryDate = table.Column<DateTime>(nullable: true),
                    RetestDate = table.Column<DateTime>(nullable: true),
                    QtyAsPerInvoice = table.Column<float>(nullable: true),
                    QtyAsPerInvoiceInDecimal = table.Column<string>(nullable: true),
                    UnitofMeasurementId = table.Column<int>(nullable: true),
                    SequenceId = table.Column<int>(nullable: true),
                    MaterialRelationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialConsignmentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialConsignmentDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialConsignmentDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialConsignmentDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialConsignmentDetails_MaterialInspectionRelationDetails~",
                        column: x => x.MaterialRelationId,
                        principalTable: "MaterialInspectionRelationDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CubicleCleaningCheckpoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CheckPointId = table.Column<int>(nullable: false),
                    Observation = table.Column<string>(maxLength: 200, nullable: true),
                    Remark = table.Column<string>(maxLength: 200, nullable: true),
                    CubicleCleaningTransactionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CubicleCleaningCheckpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningCheckpoints_CheckpointMaster_CheckPointId",
                        column: x => x.CheckPointId,
                        principalTable: "CheckpointMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningCheckpoints_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningCheckpoints_CubicleCleaningTransactions_Cubic~",
                        column: x => x.CubicleCleaningTransactionId,
                        principalTable: "CubicleCleaningTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningCheckpoints_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CubicleCleaningCheckpoints_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LineClearanceCheckpoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LineClearanceTransactionId = table.Column<int>(nullable: false),
                    CheckPointId = table.Column<int>(nullable: false),
                    Observation = table.Column<string>(maxLength: 200, nullable: true),
                    Remark = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineClearanceCheckpoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineClearanceCheckpoints_CheckpointMaster_CheckPointId",
                        column: x => x.CheckPointId,
                        principalTable: "CheckpointMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineClearanceCheckpoints_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LineClearanceCheckpoints_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LineClearanceCheckpoints_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LineClearanceCheckpoints_LineClearanceTransactions_LineClear~",
                        column: x => x.LineClearanceTransactionId,
                        principalTable: "LineClearanceTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StageOutDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    StageOutHeaderId = table.Column<int>(nullable: false),
                    SAPBatchNo = table.Column<string>(maxLength: 100, nullable: true),
                    MaterialContainerBarcode = table.Column<string>(maxLength: 100, nullable: true),
                    BalanceQuantity = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageOutDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StageOutDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StageOutDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StageOutDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StageOutDetails_StageOutHeaders_StageOutHeaderId",
                        column: x => x.StageOutHeaderId,
                        principalTable: "StageOutHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WMCalibrationDetailWeights",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    KeyTypeId = table.Column<int>(nullable: true),
                    CapturedWeightKeyTypeId = table.Column<int>(nullable: true),
                    WMCalibrationDetailId = table.Column<int>(nullable: true),
                    WMCalibrationEccentricityTestId = table.Column<int>(nullable: true),
                    WMCalibrationLinearityTestId = table.Column<int>(nullable: true),
                    WMCalibrationRepeatabilityTestId = table.Column<int>(nullable: true),
                    StandardWeightId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WMCalibrationDetailWeights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WMCalibrationDetailWeights_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationDetailWeights_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationDetailWeights_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationDetailWeights_StandardWeightMaster_StandardWeig~",
                        column: x => x.StandardWeightId,
                        principalTable: "StandardWeightMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationDetailWeights_WMCalibrationDetails_WMCalibratio~",
                        column: x => x.WMCalibrationDetailId,
                        principalTable: "WMCalibrationDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationDetailWeights_WMCalibrationEccentricityTests_WM~",
                        column: x => x.WMCalibrationEccentricityTestId,
                        principalTable: "WMCalibrationEccentricityTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationDetailWeights_WMCalibrationLinearityTests_WMCal~",
                        column: x => x.WMCalibrationLinearityTestId,
                        principalTable: "WMCalibrationLinearityTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WMCalibrationDetailWeights_WMCalibrationRepeatabilityTests_W~",
                        column: x => x.WMCalibrationRepeatabilityTestId,
                        principalTable: "WMCalibrationRepeatabilityTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DispensingPrintDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DispensingDetailId = table.Column<int>(nullable: true),
                    DeviceId = table.Column<int>(nullable: true),
                    IsController = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispensingPrintDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DispensingPrintDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensingPrintDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensingPrintDetails_DeviceMaster_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "DeviceMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensingPrintDetails_DispensingDetails_DispensingDetailId",
                        column: x => x.DispensingDetailId,
                        principalTable: "DispensingDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispensingPrintDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GRNDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    GRNHeaderId = table.Column<int>(nullable: true),
                    SAPBatchNumber = table.Column<string>(maxLength: 50, nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    MaterialId = table.Column<int>(nullable: true),
                    MfgBatchNoId = table.Column<int>(nullable: true),
                    InvoiceId = table.Column<int>(nullable: true),
                    TotalQty = table.Column<float>(nullable: false),
                    NoOfContainer = table.Column<float>(nullable: false),
                    QtyPerContainer = table.Column<float>(nullable: false),
                    TotalQtyInDecimal = table.Column<string>(nullable: true),
                    QtyPerContainerInDecimal = table.Column<string>(nullable: true),
                    DiscrepancyRemark = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRNDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GRNDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNDetails_GRNHeaders_GRNHeaderId",
                        column: x => x.GRNHeaderId,
                        principalTable: "GRNHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNDetails_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNDetails_MaterialConsignmentDetails_MfgBatchNoId",
                        column: x => x.MfgBatchNoId,
                        principalTable: "MaterialConsignmentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialDamageDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SequenceId = table.Column<int>(nullable: true),
                    ContainerNo = table.Column<string>(maxLength: 100, nullable: true),
                    Remark = table.Column<string>(maxLength: 100, nullable: true),
                    Quantity = table.Column<float>(nullable: true),
                    QuantityInDecimal = table.Column<string>(nullable: true),
                    UnitofMeasurementId = table.Column<int>(nullable: true),
                    MaterialRelationId = table.Column<int>(nullable: true),
                    MaterialConsignmentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialDamageDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialDamageDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialDamageDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialDamageDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialDamageDetails_MaterialConsignmentDetails_MaterialCon~",
                        column: x => x.MaterialConsignmentId,
                        principalTable: "MaterialConsignmentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaterialDamageDetails_MaterialInspectionRelationDetails_Mate~",
                        column: x => x.MaterialRelationId,
                        principalTable: "MaterialInspectionRelationDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WeightCaptureHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    InvoiceId = table.Column<int>(nullable: false),
                    PurchaseOrderId = table.Column<int>(nullable: false),
                    MaterialId = table.Column<int>(nullable: false),
                    MfgBatchNoId = table.Column<int>(nullable: false),
                    UnitofMeasurementId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightCaptureHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightCaptureHeaders_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeightCaptureHeaders_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeightCaptureHeaders_InvoiceDetails_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "InvoiceDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeightCaptureHeaders_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeightCaptureHeaders_MaterialConsignmentDetails_MfgBatchNoId",
                        column: x => x.MfgBatchNoId,
                        principalTable: "MaterialConsignmentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeightCaptureHeaders_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GRNMaterialLabelPrintingHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    GRNDetailId = table.Column<int>(nullable: true),
                    PackDetails = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRNMaterialLabelPrintingHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GRNMaterialLabelPrintingHeaders_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNMaterialLabelPrintingHeaders_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNMaterialLabelPrintingHeaders_GRNDetails_GRNDetailId",
                        column: x => x.GRNDetailId,
                        principalTable: "GRNDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNMaterialLabelPrintingHeaders_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GRNQtyDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    GRNDetailId = table.Column<int>(nullable: true),
                    TotalQty = table.Column<float>(nullable: false),
                    NoOfContainer = table.Column<float>(nullable: false),
                    QtyPerContainer = table.Column<float>(nullable: false),
                    DiscrepancyRemark = table.Column<string>(maxLength: 200, nullable: true),
                    IsDamaged = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRNQtyDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GRNQtyDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNQtyDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNQtyDetails_GRNDetails_GRNDetailId",
                        column: x => x.GRNDetailId,
                        principalTable: "GRNDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNQtyDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WeightCaptureDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    WeightCaptureHeaderId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    ScanBalanceId = table.Column<int>(nullable: false),
                    GrossWeight = table.Column<float>(nullable: true),
                    NetWeight = table.Column<float>(nullable: true),
                    TareWeight = table.Column<float>(nullable: true),
                    NoOfPacks = table.Column<int>(nullable: true),
                    ContainerNo = table.Column<string>(maxLength: 100, nullable: true),
                    WeighingMachineId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeightCaptureDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeightCaptureDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeightCaptureDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeightCaptureDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeightCaptureDetails_WeighingMachineMaster_WeighingMachineId",
                        column: x => x.WeighingMachineId,
                        principalTable: "WeighingMachineMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeightCaptureDetails_WeightCaptureHeaders_WeightCaptureHeade~",
                        column: x => x.WeightCaptureHeaderId,
                        principalTable: "WeightCaptureHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GRNMaterialLabelPrintingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    GRNMaterialLabelPrintingHeaderId = table.Column<int>(nullable: true),
                    PrinterId = table.Column<int>(nullable: true),
                    IsController = table.Column<bool>(nullable: false),
                    RangePrint = table.Column<string>(maxLength: 50, nullable: true),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRNMaterialLabelPrintingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GRNMaterialLabelPrintingDetails_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNMaterialLabelPrintingDetails_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNMaterialLabelPrintingDetails_GRNMaterialLabelPrintingHead~",
                        column: x => x.GRNMaterialLabelPrintingHeaderId,
                        principalTable: "GRNMaterialLabelPrintingHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNMaterialLabelPrintingDetails_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNMaterialLabelPrintingDetails_DeviceMaster_PrinterId",
                        column: x => x.PrinterId,
                        principalTable: "DeviceMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GRNMaterialLabelPrintingContainerBarcodes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    GRNMaterialLabelPrintingHeaderId = table.Column<int>(nullable: true),
                    ContainerNo = table.Column<int>(nullable: false),
                    MaterialLabelContainerBarCode = table.Column<string>(maxLength: 50, nullable: false),
                    GRNDetailId = table.Column<int>(nullable: true),
                    GRNQtyDetailId = table.Column<int>(nullable: true),
                    Quantity = table.Column<float>(nullable: true),
                    BalanceQuantity = table.Column<float>(nullable: true),
                    IsLoosedContainer = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GRNMaterialLabelPrintingContainerBarcodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GRNMaterialLabelPrintingContainerBarcodes_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNMaterialLabelPrintingContainerBarcodes_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNMaterialLabelPrintingContainerBarcodes_GRNDetails_GRNDeta~",
                        column: x => x.GRNDetailId,
                        principalTable: "GRNDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNMaterialLabelPrintingContainerBarcodes_GRNMaterialLabelPr~",
                        column: x => x.GRNMaterialLabelPrintingHeaderId,
                        principalTable: "GRNMaterialLabelPrintingHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNMaterialLabelPrintingContainerBarcodes_GRNQtyDetails_GRNQ~",
                        column: x => x.GRNQtyDetailId,
                        principalTable: "GRNQtyDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GRNMaterialLabelPrintingContainerBarcodes_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Palletizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    PalletId = table.Column<int>(nullable: true),
                    MaterialId = table.Column<int>(nullable: true),
                    GRNDetailId = table.Column<int>(nullable: true),
                    ContainerId = table.Column<int>(nullable: true),
                    TransactionId = table.Column<Guid>(nullable: false),
                    SAPBatchNumber = table.Column<string>(nullable: true),
                    ContainerNo = table.Column<int>(nullable: false),
                    ContainerBarCode = table.Column<string>(nullable: true),
                    IsUnloaded = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true),
                    ProductBatchNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Palletizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Palletizations_GRNMaterialLabelPrintingContainerBarcodes_Con~",
                        column: x => x.ContainerId,
                        principalTable: "GRNMaterialLabelPrintingContainerBarcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Palletizations_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Palletizations_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Palletizations_GRNDetails_GRNDetailId",
                        column: x => x.GRNDetailId,
                        principalTable: "GRNDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Palletizations_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Palletizations_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Palletizations_HandlingUnitMaster_PalletId",
                        column: x => x.PalletId,
                        principalTable: "HandlingUnitMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PutAwayBinToBinTransfer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<long>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    DeletedBy = table.Column<long>(nullable: true),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LocationId = table.Column<int>(nullable: true),
                    PalletId = table.Column<int>(nullable: true),
                    MaterialId = table.Column<int>(nullable: true),
                    ContainerId = table.Column<int>(nullable: true),
                    MaterialTransferTypeId = table.Column<int>(nullable: true),
                    TransactionId = table.Column<Guid>(nullable: false),
                    SAPBatchNumber = table.Column<string>(nullable: true),
                    ContainerNo = table.Column<int>(nullable: false),
                    IsUnloaded = table.Column<bool>(nullable: false),
                    TenantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PutAwayBinToBinTransfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PutAwayBinToBinTransfer_GRNMaterialLabelPrintingContainerBar~",
                        column: x => x.ContainerId,
                        principalTable: "GRNMaterialLabelPrintingContainerBarcodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PutAwayBinToBinTransfer_Users_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PutAwayBinToBinTransfer_Users_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PutAwayBinToBinTransfer_Users_ModifiedBy",
                        column: x => x.ModifiedBy,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PutAwayBinToBinTransfer_LocationMaster_LocationId",
                        column: x => x.LocationId,
                        principalTable: "LocationMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PutAwayBinToBinTransfer_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PutAwayBinToBinTransfer_MaterialTransferTypeMaster_MaterialT~",
                        column: x => x.MaterialTransferTypeId,
                        principalTable: "MaterialTransferTypeMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PutAwayBinToBinTransfer_HandlingUnitMaster_PalletId",
                        column: x => x.PalletId,
                        principalTable: "HandlingUnitMaster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityMaster_CreatedBy",
                table: "ActivityMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityMaster_DeletedBy",
                table: "ActivityMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityMaster_ModifiedBy",
                table: "ActivityMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalLevelMaster_CreatedBy",
                table: "ApprovalLevelMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalLevelMaster_DeletedBy",
                table: "ApprovalLevelMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalLevelMaster_ModifiedBy",
                table: "ApprovalLevelMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalStatusMaster_ApprovalStatus",
                table: "ApprovalStatusMaster",
                column: "ApprovalStatus",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalStatusMaster_CreatedBy",
                table: "ApprovalStatusMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalStatusMaster_DeletedBy",
                table: "ApprovalStatusMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalStatusMaster_ModifiedBy",
                table: "ApprovalStatusMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalUserModuleMappingMaster_CreatedBy",
                table: "ApprovalUserModuleMappingMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalUserModuleMappingMaster_DeletedBy",
                table: "ApprovalUserModuleMappingMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalUserModuleMappingMaster_ModifiedBy",
                table: "ApprovalUserModuleMappingMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AreaMaster_CreatedBy",
                table: "AreaMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AreaMaster_DeletedBy",
                table: "AreaMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AreaMaster_DepartmentId",
                table: "AreaMaster",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaMaster_ModifiedBy",
                table: "AreaMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AreaMaster_SubPlantId",
                table: "AreaMaster",
                column: "SubPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaUsageListLog_AreaUsageHeaderId",
                table: "AreaUsageListLog",
                column: "AreaUsageHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaUsageListLog_CheckpointId",
                table: "AreaUsageListLog",
                column: "CheckpointId");

            migrationBuilder.CreateIndex(
                name: "IX_AreaUsageListLog_CreatedBy",
                table: "AreaUsageListLog",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AreaUsageListLog_DeletedBy",
                table: "AreaUsageListLog",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AreaUsageListLog_ModifiedBy",
                table: "AreaUsageListLog",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AreaUsageLog_CreatedBy",
                table: "AreaUsageLog",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AreaUsageLog_DeletedBy",
                table: "AreaUsageLog",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AreaUsageLog_ModifiedBy",
                table: "AreaUsageLog",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_TenantId_ExecutionDuration",
                table: "AuditLogs",
                columns: new[] { "TenantId", "ExecutionDuration" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_TenantId_ExecutionTime",
                table: "AuditLogs",
                columns: new[] { "TenantId", "ExecutionTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_TenantId_UserId",
                table: "AuditLogs",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_BackgroundJobs_IsAbandoned_NextTryTime",
                table: "BackgroundJobs",
                columns: new[] { "IsAbandoned", "NextTryTime" });

            migrationBuilder.CreateIndex(
                name: "IX_CageLabelPrinting_CreatedBy",
                table: "CageLabelPrinting",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CageLabelPrinting_DeletedBy",
                table: "CageLabelPrinting",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CageLabelPrinting_ModifiedBy",
                table: "CageLabelPrinting",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CalenderMaster_CreatedBy",
                table: "CalenderMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CalenderMaster_DeletedBy",
                table: "CalenderMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CalenderMaster_HolidayTypeId",
                table: "CalenderMaster",
                column: "HolidayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CalenderMaster_ModifiedBy",
                table: "CalenderMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationFrequencyMaster_CreatedBy",
                table: "CalibrationFrequencyMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationFrequencyMaster_DeletedBy",
                table: "CalibrationFrequencyMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationFrequencyMaster_FrequencyTypeId",
                table: "CalibrationFrequencyMaster",
                column: "FrequencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationFrequencyMaster_ModifiedBy",
                table: "CalibrationFrequencyMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationFrequencyMaster_WeighingMachineId",
                table: "CalibrationFrequencyMaster",
                column: "WeighingMachineId");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationStatusMaster_CreatedBy",
                table: "CalibrationStatusMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationStatusMaster_DeletedBy",
                table: "CalibrationStatusMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationStatusMaster_ModifiedBy",
                table: "CalibrationStatusMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationTestStatusMaster_CreatedBy",
                table: "CalibrationTestStatusMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationTestStatusMaster_DeletedBy",
                table: "CalibrationTestStatusMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CalibrationTestStatusMaster_ModifiedBy",
                table: "CalibrationTestStatusMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistTypeMaster_CreatedBy",
                table: "ChecklistTypeMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistTypeMaster_DeletedBy",
                table: "ChecklistTypeMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistTypeMaster_ModifiedBy",
                table: "ChecklistTypeMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistTypeMaster_SubModuleId",
                table: "ChecklistTypeMaster",
                column: "SubModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ChecklistTypeMaster_SubPlantId",
                table: "ChecklistTypeMaster",
                column: "SubPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckpointMaster_CreatedBy",
                table: "CheckpointMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CheckpointMaster_DeletedBy",
                table: "CheckpointMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CheckpointMaster_InspectionChecklistId",
                table: "CheckpointMaster",
                column: "InspectionChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckpointMaster_ModifiedBy",
                table: "CheckpointMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CheckpointTypeMaster_CreatedBy",
                table: "CheckpointTypeMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CheckpointTypeMaster_DeletedBy",
                table: "CheckpointTypeMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CheckpointTypeMaster_ModifiedBy",
                table: "CheckpointTypeMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CompRecipeTransDetlMapping_ComponentId",
                table: "CompRecipeTransDetlMapping",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_CompRecipeTransDetlMapping_CreatedBy",
                table: "CompRecipeTransDetlMapping",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CompRecipeTransDetlMapping_DeletedBy",
                table: "CompRecipeTransDetlMapping",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CompRecipeTransDetlMapping_ModifiedBy",
                table: "CompRecipeTransDetlMapping",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CompRecipeTransDetlMapping_RecipeTransactiondetailId",
                table: "CompRecipeTransDetlMapping",
                column: "RecipeTransactiondetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Consumption_CreatedBy",
                table: "Consumption",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Consumption_DeletedBy",
                table: "Consumption",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Consumption_ModifiedBy",
                table: "Consumption",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumptionDetails_ConsumptionId",
                table: "ConsumptionDetails",
                column: "ConsumptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumptionDetails_CreatedBy",
                table: "ConsumptionDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumptionDetails_DeletedBy",
                table: "ConsumptionDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ConsumptionDetails_ModifiedBy",
                table: "ConsumptionDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CountryMaster_CountryName",
                table: "CountryMaster",
                column: "CountryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CountryMaster_CreatedBy",
                table: "CountryMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CountryMaster_DeletedBy",
                table: "CountryMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CountryMaster_ModifiedBy",
                table: "CountryMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicalRecipeTranDetlMapping_CreatedBy",
                table: "CubicalRecipeTranDetlMapping",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicalRecipeTranDetlMapping_CubicalId",
                table: "CubicalRecipeTranDetlMapping",
                column: "CubicalId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicalRecipeTranDetlMapping_DeletedBy",
                table: "CubicalRecipeTranDetlMapping",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicalRecipeTranDetlMapping_ModifiedBy",
                table: "CubicalRecipeTranDetlMapping",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicalRecipeTranDetlMapping_RecipeTransactiondetailId",
                table: "CubicalRecipeTranDetlMapping",
                column: "RecipeTransactiondetailId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleAssignmentDetails_CreatedBy",
                table: "CubicleAssignmentDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleAssignmentDetails_CubicleAssignmentHeaderId",
                table: "CubicleAssignmentDetails",
                column: "CubicleAssignmentHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleAssignmentDetails_CubicleId",
                table: "CubicleAssignmentDetails",
                column: "CubicleId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleAssignmentDetails_DeletedBy",
                table: "CubicleAssignmentDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleAssignmentDetails_InspectionLotId",
                table: "CubicleAssignmentDetails",
                column: "InspectionLotId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleAssignmentDetails_ModifiedBy",
                table: "CubicleAssignmentDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleAssignmentDetails_ProcessOrderId",
                table: "CubicleAssignmentDetails",
                column: "ProcessOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleAssignmentDetails_ProcessOrderMaterialId",
                table: "CubicleAssignmentDetails",
                column: "ProcessOrderMaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleAssignmentDetails_StatusId",
                table: "CubicleAssignmentDetails",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleAssignmentHeader_CreatedBy",
                table: "CubicleAssignmentHeader",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleAssignmentHeader_DeletedBy",
                table: "CubicleAssignmentHeader",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleAssignmentHeader_GroupStatusId",
                table: "CubicleAssignmentHeader",
                column: "GroupStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleAssignmentHeader_ModifiedBy",
                table: "CubicleAssignmentHeader",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleAssignmentWIP_CreatedBy",
                table: "CubicleAssignmentWIP",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleAssignmentWIP_DeletedBy",
                table: "CubicleAssignmentWIP",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleAssignmentWIP_ModifiedBy",
                table: "CubicleAssignmentWIP",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningCheckpoints_CheckPointId",
                table: "CubicleCleaningCheckpoints",
                column: "CheckPointId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningCheckpoints_CreatedBy",
                table: "CubicleCleaningCheckpoints",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningCheckpoints_CubicleCleaningTransactionId",
                table: "CubicleCleaningCheckpoints",
                column: "CubicleCleaningTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningCheckpoints_DeletedBy",
                table: "CubicleCleaningCheckpoints",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningCheckpoints_ModifiedBy",
                table: "CubicleCleaningCheckpoints",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningDailyStatus_CreatedBy",
                table: "CubicleCleaningDailyStatus",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningDailyStatus_CubicleId",
                table: "CubicleCleaningDailyStatus",
                column: "CubicleId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningDailyStatus_DeletedBy",
                table: "CubicleCleaningDailyStatus",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningDailyStatus_ModifiedBy",
                table: "CubicleCleaningDailyStatus",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningDailyStatus_StatusId",
                table: "CubicleCleaningDailyStatus",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningTransactions_CreatedBy",
                table: "CubicleCleaningTransactions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningTransactions_CubicleId",
                table: "CubicleCleaningTransactions",
                column: "CubicleId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningTransactions_DeletedBy",
                table: "CubicleCleaningTransactions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningTransactions_ModifiedBy",
                table: "CubicleCleaningTransactions",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningTransactions_StatusId",
                table: "CubicleCleaningTransactions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningTransactions_TypeId",
                table: "CubicleCleaningTransactions",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningTypeMaster_CreatedBy",
                table: "CubicleCleaningTypeMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningTypeMaster_DeletedBy",
                table: "CubicleCleaningTypeMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleCleaningTypeMaster_ModifiedBy",
                table: "CubicleCleaningTypeMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleMaster_AreaId",
                table: "CubicleMaster",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleMaster_CreatedBy",
                table: "CubicleMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleMaster_DeletedBy",
                table: "CubicleMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleMaster_ModifiedBy",
                table: "CubicleMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleMaster_PlantId",
                table: "CubicleMaster",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_CubicleMaster_SLOCId",
                table: "CubicleMaster",
                column: "SLOCId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentMaster_CreatedBy",
                table: "DepartmentMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentMaster_DeletedBy",
                table: "DepartmentMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentMaster_ModifiedBy",
                table: "DepartmentMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentMaster_SubPlantId",
                table: "DepartmentMaster",
                column: "SubPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_DesignationMaster_CreatedBy",
                table: "DesignationMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DesignationMaster_DeletedBy",
                table: "DesignationMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DesignationMaster_ModifiedBy",
                table: "DesignationMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMaster_AreaId",
                table: "DeviceMaster",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMaster_CreatedBy",
                table: "DeviceMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMaster_CubicleId",
                table: "DeviceMaster",
                column: "CubicleId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMaster_DeletedBy",
                table: "DeviceMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMaster_DepartmentId",
                table: "DeviceMaster",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMaster_DeviceTypeId",
                table: "DeviceMaster",
                column: "DeviceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMaster_ModifiedBy",
                table: "DeviceMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceMaster_ModeId",
                table: "DeviceMaster",
                column: "ModeId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceTypeMaster_CreatedBy",
                table: "DeviceTypeMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceTypeMaster_DeletedBy",
                table: "DeviceTypeMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceTypeMaster_ModifiedBy",
                table: "DeviceTypeMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchDetails_CreatedBy",
                table: "DispatchDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchDetails_DeletedBy",
                table: "DispatchDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchDetails_ModifiedBy",
                table: "DispatchDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingDetails_CreatedBy",
                table: "DispensingDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingDetails_DeletedBy",
                table: "DispensingDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingDetails_DispensingHeaderId",
                table: "DispensingDetails",
                column: "DispensingHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingDetails_ModifiedBy",
                table: "DispensingDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingDetails_SamplingTypeId",
                table: "DispensingDetails",
                column: "SamplingTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingDetails_UnitOfMeasurementId",
                table: "DispensingDetails",
                column: "UnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingDetails_WeighingMachineId",
                table: "DispensingDetails",
                column: "WeighingMachineId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingHeaders_CreatedBy",
                table: "DispensingHeaders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingHeaders_DeletedBy",
                table: "DispensingHeaders",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingHeaders_InspectionLotId",
                table: "DispensingHeaders",
                column: "InspectionLotId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingHeaders_ModifiedBy",
                table: "DispensingHeaders",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingHeaders_ProcessOrderId",
                table: "DispensingHeaders",
                column: "ProcessOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingHeaders_RLAFId",
                table: "DispensingHeaders",
                column: "RLAFId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingHeaders_StatusId",
                table: "DispensingHeaders",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingPrintDetails_CreatedBy",
                table: "DispensingPrintDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingPrintDetails_DeletedBy",
                table: "DispensingPrintDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingPrintDetails_DeviceId",
                table: "DispensingPrintDetails",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingPrintDetails_DispensingDetailId",
                table: "DispensingPrintDetails",
                column: "DispensingDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_DispensingPrintDetails_ModifiedBy",
                table: "DispensingPrintDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicEntityProperties_DynamicPropertyId",
                table: "DynamicEntityProperties",
                column: "DynamicPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicEntityProperties_EntityFullName_DynamicPropertyId_Ten~",
                table: "DynamicEntityProperties",
                columns: new[] { "EntityFullName", "DynamicPropertyId", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DynamicEntityPropertyValues_DynamicEntityPropertyId",
                table: "DynamicEntityPropertyValues",
                column: "DynamicEntityPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicProperties_PropertyName_TenantId",
                table: "DynamicProperties",
                columns: new[] { "PropertyName", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DynamicPropertyValues_DynamicPropertyId",
                table: "DynamicPropertyValues",
                column: "DynamicPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityChanges_EntityChangeSetId",
                table: "EntityChanges",
                column: "EntityChangeSetId");

            migrationBuilder.CreateIndex(
                name: "IX_EntityChanges_EntityTypeFullName_EntityId",
                table: "EntityChanges",
                columns: new[] { "EntityTypeFullName", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_EntityChangeSets_TenantId_CreationTime",
                table: "EntityChangeSets",
                columns: new[] { "TenantId", "CreationTime" });

            migrationBuilder.CreateIndex(
                name: "IX_EntityChangeSets_TenantId_Reason",
                table: "EntityChangeSets",
                columns: new[] { "TenantId", "Reason" });

            migrationBuilder.CreateIndex(
                name: "IX_EntityChangeSets_TenantId_UserId",
                table: "EntityChangeSets",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_EntityPropertyChanges_EntityChangeId",
                table: "EntityPropertyChanges",
                column: "EntityChangeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssignments_CreatedBy",
                table: "EquipmentAssignments",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssignments_CubicleAssignmentHeaderId",
                table: "EquipmentAssignments",
                column: "CubicleAssignmentHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssignments_Cubicleid",
                table: "EquipmentAssignments",
                column: "Cubicleid");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssignments_DeletedBy",
                table: "EquipmentAssignments",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssignments_EquipmentId",
                table: "EquipmentAssignments",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentAssignments_ModifiedBy",
                table: "EquipmentAssignments",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCleaningCheckpoints_CreatedBy",
                table: "EquipmentCleaningCheckpoints",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCleaningCheckpoints_DeletedBy",
                table: "EquipmentCleaningCheckpoints",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCleaningCheckpoints_EquipmentCleaningTransactionId",
                table: "EquipmentCleaningCheckpoints",
                column: "EquipmentCleaningTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCleaningCheckpoints_ModifiedBy",
                table: "EquipmentCleaningCheckpoints",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCleaningStatus_CreatedBy",
                table: "EquipmentCleaningStatus",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCleaningStatus_DeletedBy",
                table: "EquipmentCleaningStatus",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCleaningStatus_ModifiedBy",
                table: "EquipmentCleaningStatus",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCleaningStatus_StatusId",
                table: "EquipmentCleaningStatus",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCleaningTransactions_CleaningTypeId",
                table: "EquipmentCleaningTransactions",
                column: "CleaningTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCleaningTransactions_CreatedBy",
                table: "EquipmentCleaningTransactions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCleaningTransactions_DeletedBy",
                table: "EquipmentCleaningTransactions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCleaningTransactions_ModifiedBy",
                table: "EquipmentCleaningTransactions",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCleaningTransactions_StatusId",
                table: "EquipmentCleaningTransactions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCleaningTypeMaster_CreatedBy",
                table: "EquipmentCleaningTypeMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCleaningTypeMaster_DeletedBy",
                table: "EquipmentCleaningTypeMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCleaningTypeMaster_ModifiedBy",
                table: "EquipmentCleaningTypeMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaster_CreatedBy",
                table: "EquipmentMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaster_DeletedBy",
                table: "EquipmentMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaster_EquipmentTypeId",
                table: "EquipmentMaster",
                column: "EquipmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaster_ModifiedBy",
                table: "EquipmentMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaster_PlantId",
                table: "EquipmentMaster",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentMaster_SLOCId",
                table: "EquipmentMaster",
                column: "SLOCId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentTypeMaster_CreatedBy",
                table: "EquipmentTypeMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentTypeMaster_DeletedBy",
                table: "EquipmentTypeMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentTypeMaster_ModifiedBy",
                table: "EquipmentTypeMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUsageLog_CreatedBy",
                table: "EquipmentUsageLog",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUsageLog_DeletedBy",
                table: "EquipmentUsageLog",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUsageLog_ModifiedBy",
                table: "EquipmentUsageLog",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUsageLogList_CheckpointId",
                table: "EquipmentUsageLogList",
                column: "CheckpointId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUsageLogList_CreatedBy",
                table: "EquipmentUsageLogList",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUsageLogList_DeletedBy",
                table: "EquipmentUsageLogList",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUsageLogList_EquipmentUsageHeaderId",
                table: "EquipmentUsageLogList",
                column: "EquipmentUsageHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentUsageLogList_ModifiedBy",
                table: "EquipmentUsageLogList",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Features_EditionId_Name",
                table: "Features",
                columns: new[] { "EditionId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Features_TenantId_Name",
                table: "Features",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_FgPicking_CreatedBy",
                table: "FgPicking",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FgPicking_DeletedBy",
                table: "FgPicking",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FgPicking_ModifiedBy",
                table: "FgPicking",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FgPutAway_CreatedBy",
                table: "FgPutAway",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FgPutAway_DeletedBy",
                table: "FgPutAway",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FgPutAway_ModifiedBy",
                table: "FgPutAway",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FrequencyTypeMaster_CreatedBy",
                table: "FrequencyTypeMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FrequencyTypeMaster_DeletedBy",
                table: "FrequencyTypeMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_FrequencyTypeMaster_ModifiedBy",
                table: "FrequencyTypeMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GateEntry_CreatedBy",
                table: "GateEntry",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GateEntry_DeletedBy",
                table: "GateEntry",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GateEntry_InvoiceId",
                table: "GateEntry",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_GateEntry_ModifiedBy",
                table: "GateEntry",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GateMaster_CreatedBy",
                table: "GateMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GateMaster_DeletedBy",
                table: "GateMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GateMaster_ModifiedBy",
                table: "GateMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GateMaster_PlantId",
                table: "GateMaster",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNDetails_CreatedBy",
                table: "GRNDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNDetails_DeletedBy",
                table: "GRNDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNDetails_GRNHeaderId",
                table: "GRNDetails",
                column: "GRNHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNDetails_ModifiedBy",
                table: "GRNDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNDetails_MaterialId",
                table: "GRNDetails",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNDetails_MfgBatchNoId",
                table: "GRNDetails",
                column: "MfgBatchNoId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNHeaders_CreatedBy",
                table: "GRNHeaders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNHeaders_DeletedBy",
                table: "GRNHeaders",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNHeaders_ModifiedBy",
                table: "GRNHeaders",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaterialLabelPrintingContainerBarcodes_CreatedBy",
                table: "GRNMaterialLabelPrintingContainerBarcodes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaterialLabelPrintingContainerBarcodes_DeletedBy",
                table: "GRNMaterialLabelPrintingContainerBarcodes",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaterialLabelPrintingContainerBarcodes_GRNDetailId",
                table: "GRNMaterialLabelPrintingContainerBarcodes",
                column: "GRNDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaterialLabelPrintingContainerBarcodes_GRNMaterialLabelPr~",
                table: "GRNMaterialLabelPrintingContainerBarcodes",
                column: "GRNMaterialLabelPrintingHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaterialLabelPrintingContainerBarcodes_GRNQtyDetailId",
                table: "GRNMaterialLabelPrintingContainerBarcodes",
                column: "GRNQtyDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaterialLabelPrintingContainerBarcodes_ModifiedBy",
                table: "GRNMaterialLabelPrintingContainerBarcodes",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaterialLabelPrintingDetails_CreatedBy",
                table: "GRNMaterialLabelPrintingDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaterialLabelPrintingDetails_DeletedBy",
                table: "GRNMaterialLabelPrintingDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaterialLabelPrintingDetails_GRNMaterialLabelPrintingHead~",
                table: "GRNMaterialLabelPrintingDetails",
                column: "GRNMaterialLabelPrintingHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaterialLabelPrintingDetails_ModifiedBy",
                table: "GRNMaterialLabelPrintingDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaterialLabelPrintingDetails_PrinterId",
                table: "GRNMaterialLabelPrintingDetails",
                column: "PrinterId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaterialLabelPrintingHeaders_CreatedBy",
                table: "GRNMaterialLabelPrintingHeaders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaterialLabelPrintingHeaders_DeletedBy",
                table: "GRNMaterialLabelPrintingHeaders",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaterialLabelPrintingHeaders_GRNDetailId",
                table: "GRNMaterialLabelPrintingHeaders",
                column: "GRNDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNMaterialLabelPrintingHeaders_ModifiedBy",
                table: "GRNMaterialLabelPrintingHeaders",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNQtyDetails_CreatedBy",
                table: "GRNQtyDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNQtyDetails_DeletedBy",
                table: "GRNQtyDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_GRNQtyDetails_GRNDetailId",
                table: "GRNQtyDetails",
                column: "GRNDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_GRNQtyDetails_ModifiedBy",
                table: "GRNQtyDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingUnitMaster_CreatedBy",
                table: "HandlingUnitMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingUnitMaster_DeletedBy",
                table: "HandlingUnitMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingUnitMaster_HandlingUnitTypeId",
                table: "HandlingUnitMaster",
                column: "HandlingUnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingUnitMaster_ModifiedBy",
                table: "HandlingUnitMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingUnitMaster_PlantId",
                table: "HandlingUnitMaster",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingUnitTypeMaster_CreatedBy",
                table: "HandlingUnitTypeMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingUnitTypeMaster_DeletedBy",
                table: "HandlingUnitTypeMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_HandlingUnitTypeMaster_ModifiedBy",
                table: "HandlingUnitTypeMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_HolidayTypeMaster_CreatedBy",
                table: "HolidayTypeMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_HolidayTypeMaster_DeletedBy",
                table: "HolidayTypeMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_HolidayTypeMaster_ModifiedBy",
                table: "HolidayTypeMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InProcessLabelDetails_CreatedBy",
                table: "InProcessLabelDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InProcessLabelDetails_DeletedBy",
                table: "InProcessLabelDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InProcessLabelDetails_ModifiedBy",
                table: "InProcessLabelDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionChecklistMaster_CreatedBy",
                table: "InspectionChecklistMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionChecklistMaster_DeletedBy",
                table: "InspectionChecklistMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionChecklistMaster_ModifiedBy",
                table: "InspectionChecklistMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionLot_CreatedBy",
                table: "InspectionLot",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionLot_DeletedBy",
                table: "InspectionLot",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InspectionLot_ModifiedBy",
                table: "InspectionLot",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_CreatedBy",
                table: "InvoiceDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_DeletedBy",
                table: "InvoiceDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_ModifiedBy",
                table: "InvoiceDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_IssueToProductions_CreatedBy",
                table: "IssueToProductions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_IssueToProductions_DeletedBy",
                table: "IssueToProductions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_IssueToProductions_ModifiedBy",
                table: "IssueToProductions",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LabelPrintPacking_CreatedBy",
                table: "LabelPrintPacking",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LabelPrintPacking_DeletedBy",
                table: "LabelPrintPacking",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LabelPrintPacking_ModifiedBy",
                table: "LabelPrintPacking",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_TenantId_Name",
                table: "Languages",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_LanguageTexts_TenantId_Source_LanguageName_Key",
                table: "LanguageTexts",
                columns: new[] { "TenantId", "Source", "LanguageName", "Key" });

            migrationBuilder.CreateIndex(
                name: "IX_LineClearanceCheckpoints_CheckPointId",
                table: "LineClearanceCheckpoints",
                column: "CheckPointId");

            migrationBuilder.CreateIndex(
                name: "IX_LineClearanceCheckpoints_CreatedBy",
                table: "LineClearanceCheckpoints",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LineClearanceCheckpoints_DeletedBy",
                table: "LineClearanceCheckpoints",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LineClearanceCheckpoints_ModifiedBy",
                table: "LineClearanceCheckpoints",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LineClearanceCheckpoints_LineClearanceTransactionId",
                table: "LineClearanceCheckpoints",
                column: "LineClearanceTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_LineClearanceTransactions_CreatedBy",
                table: "LineClearanceTransactions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LineClearanceTransactions_CubicleId",
                table: "LineClearanceTransactions",
                column: "CubicleId");

            migrationBuilder.CreateIndex(
                name: "IX_LineClearanceTransactions_DeletedBy",
                table: "LineClearanceTransactions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LineClearanceTransactions_GroupId",
                table: "LineClearanceTransactions",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_LineClearanceTransactions_ModifiedBy",
                table: "LineClearanceTransactions",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LineClearanceTransactions_StatusId",
                table: "LineClearanceTransactions",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Loading_CreatedBy",
                table: "Loading",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Loading_DeletedBy",
                table: "Loading",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Loading_ModifiedBy",
                table: "Loading",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LocationMaster_AreaId",
                table: "LocationMaster",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationMaster_CreatedBy",
                table: "LocationMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LocationMaster_DeletedBy",
                table: "LocationMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LocationMaster_ModifiedBy",
                table: "LocationMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LocationMaster_PlantId",
                table: "LocationMaster",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_LogFormHistory_CreatedBy",
                table: "LogFormHistory",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LogFormHistory_DeletedBy",
                table: "LogFormHistory",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LogFormHistory_ModifiedBy",
                table: "LogFormHistory",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LogoMaster_CreatedBy",
                table: "LogoMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LogoMaster_DeletedBy",
                table: "LogoMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LogoMaster_ModifiedBy",
                table: "LogoMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBatchDispensingContainerDetails_CreatedBy",
                table: "MaterialBatchDispensingContainerDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBatchDispensingContainerDetails_DeletedBy",
                table: "MaterialBatchDispensingContainerDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBatchDispensingContainerDetails_ModifiedBy",
                table: "MaterialBatchDispensingContainerDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBatchDispensingContainerDetails_MaterialBatchDispens~",
                table: "MaterialBatchDispensingContainerDetails",
                column: "MaterialBatchDispensingHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBatchDispensingHeaders_CreatedBy",
                table: "MaterialBatchDispensingHeaders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBatchDispensingHeaders_DeletedBy",
                table: "MaterialBatchDispensingHeaders",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialBatchDispensingHeaders_ModifiedBy",
                table: "MaterialBatchDispensingHeaders",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialChecklistDetails_CheckPointId",
                table: "MaterialChecklistDetails",
                column: "CheckPointId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialChecklistDetails_CreatedBy",
                table: "MaterialChecklistDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialChecklistDetails_DeletedBy",
                table: "MaterialChecklistDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialChecklistDetails_ModifiedBy",
                table: "MaterialChecklistDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialChecklistDetails_MaterialRelationId",
                table: "MaterialChecklistDetails",
                column: "MaterialRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialConsignmentDetails_CreatedBy",
                table: "MaterialConsignmentDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialConsignmentDetails_DeletedBy",
                table: "MaterialConsignmentDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialConsignmentDetails_ModifiedBy",
                table: "MaterialConsignmentDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialConsignmentDetails_MaterialRelationId",
                table: "MaterialConsignmentDetails",
                column: "MaterialRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDamageDetails_CreatedBy",
                table: "MaterialDamageDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDamageDetails_DeletedBy",
                table: "MaterialDamageDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDamageDetails_ModifiedBy",
                table: "MaterialDamageDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDamageDetails_MaterialConsignmentId",
                table: "MaterialDamageDetails",
                column: "MaterialConsignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDamageDetails_MaterialRelationId",
                table: "MaterialDamageDetails",
                column: "MaterialRelationId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDestructions_CreatedBy",
                table: "MaterialDestructions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDestructions_DeletedBy",
                table: "MaterialDestructions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDestructions_ModifiedBy",
                table: "MaterialDestructions",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialInspectionHeaders_CreatedBy",
                table: "MaterialInspectionHeaders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialInspectionHeaders_DeletedBy",
                table: "MaterialInspectionHeaders",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialInspectionHeaders_GateEntryId",
                table: "MaterialInspectionHeaders",
                column: "GateEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialInspectionHeaders_InvoiceId",
                table: "MaterialInspectionHeaders",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialInspectionHeaders_ModifiedBy",
                table: "MaterialInspectionHeaders",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialInspectionHeaders_TransactionStatusId",
                table: "MaterialInspectionHeaders",
                column: "TransactionStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialInspectionRelationDetails_ChecklistTypeId",
                table: "MaterialInspectionRelationDetails",
                column: "ChecklistTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialInspectionRelationDetails_CreatedBy",
                table: "MaterialInspectionRelationDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialInspectionRelationDetails_DeletedBy",
                table: "MaterialInspectionRelationDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialInspectionRelationDetails_InspectionChecklistId",
                table: "MaterialInspectionRelationDetails",
                column: "InspectionChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialInspectionRelationDetails_ModifiedBy",
                table: "MaterialInspectionRelationDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialInspectionRelationDetails_MaterialHeaderId",
                table: "MaterialInspectionRelationDetails",
                column: "MaterialHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialInspectionRelationDetails_MaterialId",
                table: "MaterialInspectionRelationDetails",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialInspectionRelationDetails_TransactionStatusId",
                table: "MaterialInspectionRelationDetails",
                column: "TransactionStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialMaster_CreatedBy",
                table: "MaterialMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialMaster_DeletedBy",
                table: "MaterialMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialMaster_ModifiedBy",
                table: "MaterialMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialReturnDetails_CreatedBy",
                table: "MaterialReturnDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialReturnDetails_DeletedBy",
                table: "MaterialReturnDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialReturnDetails_ModifiedBy",
                table: "MaterialReturnDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialReturnDetailsSAP_CreatedBy",
                table: "MaterialReturnDetailsSAP",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialReturnDetailsSAP_DeletedBy",
                table: "MaterialReturnDetailsSAP",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialReturnDetailsSAP_ModifiedBy",
                table: "MaterialReturnDetailsSAP",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_CreatedBy",
                table: "Materials",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_DeletedBy",
                table: "Materials",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_ModifiedBy",
                table: "Materials",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_PurchaseOrderId",
                table: "Materials",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTransferTypeMaster_CreatedBy",
                table: "MaterialTransferTypeMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTransferTypeMaster_DeletedBy",
                table: "MaterialTransferTypeMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTransferTypeMaster_ModifiedBy",
                table: "MaterialTransferTypeMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ModeMaster_CreatedBy",
                table: "ModeMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ModeMaster_DeletedBy",
                table: "ModeMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ModeMaster_ModifiedBy",
                table: "ModeMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleMaster_CreatedBy",
                table: "ModuleMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleMaster_DeletedBy",
                table: "ModuleMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleMaster_ModifiedBy",
                table: "ModuleMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleSubModule_CreatedBy",
                table: "ModuleSubModule",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleSubModule_DeletedBy",
                table: "ModuleSubModule",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleSubModule_ModifiedBy",
                table: "ModuleSubModule",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleSubModule_ModuleId",
                table: "ModuleSubModule",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleSubModule_SubModuleId",
                table: "ModuleSubModule",
                column: "SubModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSubscriptions_NotificationName_EntityTypeName_En~",
                table: "NotificationSubscriptions",
                columns: new[] { "NotificationName", "EntityTypeName", "EntityId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSubscriptions_TenantId_NotificationName_EntityTy~",
                table: "NotificationSubscriptions",
                columns: new[] { "TenantId", "NotificationName", "EntityTypeName", "EntityId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_OBDDetails_CreatedBy",
                table: "OBDDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OBDDetails_DeletedBy",
                table: "OBDDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OBDDetails_ModifiedBy",
                table: "OBDDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUnitRoles_TenantId_OrganizationUnitId",
                table: "OrganizationUnitRoles",
                columns: new[] { "TenantId", "OrganizationUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUnitRoles_TenantId_RoleId",
                table: "OrganizationUnitRoles",
                columns: new[] { "TenantId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUnits_ParentId",
                table: "OrganizationUnits",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationUnits_TenantId_Code",
                table: "OrganizationUnits",
                columns: new[] { "TenantId", "Code" });

            migrationBuilder.CreateIndex(
                name: "IX_PackingMaster_CreatedBy",
                table: "PackingMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PackingMaster_DeletedBy",
                table: "PackingMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PackingMaster_ModifiedBy",
                table: "PackingMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Palletizations_ContainerId",
                table: "Palletizations",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Palletizations_CreatedBy",
                table: "Palletizations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Palletizations_DeletedBy",
                table: "Palletizations",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Palletizations_GRNDetailId",
                table: "Palletizations",
                column: "GRNDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_Palletizations_ModifiedBy",
                table: "Palletizations",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Palletizations_MaterialId",
                table: "Palletizations",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Palletizations_PalletId",
                table: "Palletizations",
                column: "PalletId");

            migrationBuilder.CreateIndex(
                name: "IX_PalletMaster_CreatedBy",
                table: "PalletMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PalletMaster_DeletedBy",
                table: "PalletMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PalletMaster_ModifiedBy",
                table: "PalletMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionMaster_CreatedBy",
                table: "PermissionMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionMaster_DeletedBy",
                table: "PermissionMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionMaster_ModifiedBy",
                table: "PermissionMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_TenantId_Name",
                table: "Permissions",
                columns: new[] { "TenantId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_RoleId",
                table: "Permissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserId",
                table: "Permissions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PickingMaster_CreatedBy",
                table: "PickingMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PickingMaster_DeletedBy",
                table: "PickingMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PickingMaster_ModifiedBy",
                table: "PickingMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlantMaster_CountryId",
                table: "PlantMaster",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantMaster_CreatedBy",
                table: "PlantMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlantMaster_DeletedBy",
                table: "PlantMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlantMaster_ModifiedBy",
                table: "PlantMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlantMaster_MasterPlantId",
                table: "PlantMaster",
                column: "MasterPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantMaster_StateId",
                table: "PlantMaster",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_PostWIPDataToSAP_CreatedBy",
                table: "PostWIPDataToSAP",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PostWIPDataToSAP_DeletedBy",
                table: "PostWIPDataToSAP",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PostWIPDataToSAP_ModifiedBy",
                table: "PostWIPDataToSAP",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PRNEntryMaster_CreatedBy",
                table: "PRNEntryMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PRNEntryMaster_DeletedBy",
                table: "PRNEntryMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PRNEntryMaster_ModifiedBy",
                table: "PRNEntryMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessOrderAfterRelease_CreatedBy",
                table: "ProcessOrderAfterRelease",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessOrderAfterRelease_DeletedBy",
                table: "ProcessOrderAfterRelease",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessOrderAfterRelease_ModifiedBy",
                table: "ProcessOrderAfterRelease",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessOrderMaterialAfterRelease_CreatedBy",
                table: "ProcessOrderMaterialAfterRelease",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessOrderMaterialAfterRelease_DeletedBy",
                table: "ProcessOrderMaterialAfterRelease",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessOrderMaterialAfterRelease_ModifiedBy",
                table: "ProcessOrderMaterialAfterRelease",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessOrderMaterials_CreatedBy",
                table: "ProcessOrderMaterials",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessOrderMaterials_DeletedBy",
                table: "ProcessOrderMaterials",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessOrderMaterials_InspectionLotId",
                table: "ProcessOrderMaterials",
                column: "InspectionLotId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessOrderMaterials_ModifiedBy",
                table: "ProcessOrderMaterials",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessOrderMaterials_ProcessOrderId",
                table: "ProcessOrderMaterials",
                column: "ProcessOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessOrders_CreatedBy",
                table: "ProcessOrders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessOrders_DeletedBy",
                table: "ProcessOrders",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessOrders_ModifiedBy",
                table: "ProcessOrders",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_CreatedBy",
                table: "PurchaseOrders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_DeletedBy",
                table: "PurchaseOrders",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ModifiedBy",
                table: "PurchaseOrders",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PlantId",
                table: "PurchaseOrders",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Putaway_CreatedBy",
                table: "Putaway",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Putaway_DeletedBy",
                table: "Putaway",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Putaway_ModifiedBy",
                table: "Putaway",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PutAwayBinToBinTransfer_ContainerId",
                table: "PutAwayBinToBinTransfer",
                column: "ContainerId");

            migrationBuilder.CreateIndex(
                name: "IX_PutAwayBinToBinTransfer_CreatedBy",
                table: "PutAwayBinToBinTransfer",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PutAwayBinToBinTransfer_DeletedBy",
                table: "PutAwayBinToBinTransfer",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PutAwayBinToBinTransfer_ModifiedBy",
                table: "PutAwayBinToBinTransfer",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PutAwayBinToBinTransfer_LocationId",
                table: "PutAwayBinToBinTransfer",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PutAwayBinToBinTransfer_MaterialId",
                table: "PutAwayBinToBinTransfer",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_PutAwayBinToBinTransfer_MaterialTransferTypeId",
                table: "PutAwayBinToBinTransfer",
                column: "MaterialTransferTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PutAwayBinToBinTransfer_PalletId",
                table: "PutAwayBinToBinTransfer",
                column: "PalletId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeMaster_CreatedBy",
                table: "RecipeMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeMaster_DeletedBy",
                table: "RecipeMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeMaster_ModifiedBy",
                table: "RecipeMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTransactionDetails_CreatedBy",
                table: "RecipeTransactionDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTransactionDetails_DeletedBy",
                table: "RecipeTransactionDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTransactionDetails_ModifiedBy",
                table: "RecipeTransactionDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTransactionDetails_RecipeTransactionHeaderId",
                table: "RecipeTransactionDetails",
                column: "RecipeTransactionHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTransactionHeader_CreatedBy",
                table: "RecipeTransactionHeader",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTransactionHeader_DeletedBy",
                table: "RecipeTransactionHeader",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTransactionHeader_ModifiedBy",
                table: "RecipeTransactionHeader",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeWiseProcessOrderMapping_CreatedBy",
                table: "RecipeWiseProcessOrderMapping",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeWiseProcessOrderMapping_DeletedBy",
                table: "RecipeWiseProcessOrderMapping",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeWiseProcessOrderMapping_ModifiedBy",
                table: "RecipeWiseProcessOrderMapping",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportConfiguration_CreatedBy",
                table: "ReportConfiguration",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportConfiguration_DeletedBy",
                table: "ReportConfiguration",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportConfiguration_ModifiedBy",
                table: "ReportConfiguration",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReportConfiguration_SubModuleId",
                table: "ReportConfiguration",
                column: "SubModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnToVendorDetail_CreatedBy",
                table: "ReturnToVendorDetail",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnToVendorDetail_DeletedBy",
                table: "ReturnToVendorDetail",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnToVendorDetail_ModifiedBy",
                table: "ReturnToVendorDetail",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnToVendorDetail_ReturnToVendorHeaderId",
                table: "ReturnToVendorDetail",
                column: "ReturnToVendorHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnToVendorHeader_CreatedBy",
                table: "ReturnToVendorHeader",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnToVendorHeader_DeletedBy",
                table: "ReturnToVendorHeader",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnToVendorHeader_ModifiedBy",
                table: "ReturnToVendorHeader",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnToVendorHeader_StatusId",
                table: "ReturnToVendorHeader",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_TenantId_ClaimType",
                table: "RoleClaims",
                columns: new[] { "TenantId", "ClaimType" });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_CreatedBy",
                table: "RolePermissions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_DeletedBy",
                table: "RolePermissions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_ModifiedBy",
                table: "RolePermissions",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_ModuleSubModuleId",
                table: "RolePermissions",
                column: "ModuleSubModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ApprovalStatusId",
                table: "Roles",
                column: "ApprovalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedBy",
                table: "Roles",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_DeletedBy",
                table: "Roles",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ModifiedBy",
                table: "Roles",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_TenantId_NormalizedName",
                table: "Roles",
                columns: new[] { "TenantId", "NormalizedName" });

            migrationBuilder.CreateIndex(
                name: "IX_SampleDestructions_CreatedBy",
                table: "SampleDestructions",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SampleDestructions_DeletedBy",
                table: "SampleDestructions",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SampleDestructions_ModifiedBy",
                table: "SampleDestructions",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SampleDestructions_UnitOfMeasurementId",
                table: "SampleDestructions",
                column: "UnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_SampleDestructions_WeighingMachineId",
                table: "SampleDestructions",
                column: "WeighingMachineId");

            migrationBuilder.CreateIndex(
                name: "IX_SamplingTypeMaster_CreatedBy",
                table: "SamplingTypeMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SamplingTypeMaster_DeletedBy",
                table: "SamplingTypeMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SamplingTypeMaster_ModifiedBy",
                table: "SamplingTypeMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPGRNPosting_CreatedBy",
                table: "SAPGRNPosting",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPGRNPosting_DeletedBy",
                table: "SAPGRNPosting",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPGRNPosting_ModifiedBy",
                table: "SAPGRNPosting",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPPlantMaster_CreatedBy",
                table: "SAPPlantMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPPlantMaster_DeletedBy",
                table: "SAPPlantMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPPlantMaster_ModifiedBy",
                table: "SAPPlantMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPProcessOrderReceivedMaterials_CreatedBy",
                table: "SAPProcessOrderReceivedMaterials",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPProcessOrderReceivedMaterials_DeletedBy",
                table: "SAPProcessOrderReceivedMaterials",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPProcessOrderReceivedMaterials_ModifiedBy",
                table: "SAPProcessOrderReceivedMaterials",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPProcessOrders_CreatedBy",
                table: "SAPProcessOrders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPProcessOrders_DeletedBy",
                table: "SAPProcessOrders",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPProcessOrders_ModifiedBy",
                table: "SAPProcessOrders",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPQualityControlDetails_CreatedBy",
                table: "SAPQualityControlDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPQualityControlDetails_DeletedBy",
                table: "SAPQualityControlDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPQualityControlDetails_ModifiedBy",
                table: "SAPQualityControlDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPReturntoMaterial_CreatedBy",
                table: "SAPReturntoMaterial",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPReturntoMaterial_DeletedBy",
                table: "SAPReturntoMaterial",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPReturntoMaterial_ModifiedBy",
                table: "SAPReturntoMaterial",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPUOMMaster_CreatedBy",
                table: "SAPUOMMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPUOMMaster_DeletedBy",
                table: "SAPUOMMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SAPUOMMaster_ModifiedBy",
                table: "SAPUOMMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_UserId",
                table: "Settings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_TenantId_Name_UserId",
                table: "Settings",
                columns: new[] { "TenantId", "Name", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StageOutDetails_CreatedBy",
                table: "StageOutDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StageOutDetails_DeletedBy",
                table: "StageOutDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StageOutDetails_ModifiedBy",
                table: "StageOutDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StageOutDetails_StageOutHeaderId",
                table: "StageOutDetails",
                column: "StageOutHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_StageOutHeaders_CreatedBy",
                table: "StageOutHeaders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StageOutHeaders_CubicleId",
                table: "StageOutHeaders",
                column: "CubicleId");

            migrationBuilder.CreateIndex(
                name: "IX_StageOutHeaders_DeletedBy",
                table: "StageOutHeaders",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StageOutHeaders_ModifiedBy",
                table: "StageOutHeaders",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StageOutHeaders_StatusId",
                table: "StageOutHeaders",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardWeightBoxMaster_AreaId",
                table: "StandardWeightBoxMaster",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardWeightBoxMaster_CreatedBy",
                table: "StandardWeightBoxMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StandardWeightBoxMaster_DeletedBy",
                table: "StandardWeightBoxMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StandardWeightBoxMaster_DepartmentId",
                table: "StandardWeightBoxMaster",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardWeightBoxMaster_ModifiedBy",
                table: "StandardWeightBoxMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StandardWeightBoxMaster_SubPlantId",
                table: "StandardWeightBoxMaster",
                column: "SubPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardWeightMaster_AreaId",
                table: "StandardWeightMaster",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardWeightMaster_CreatedBy",
                table: "StandardWeightMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StandardWeightMaster_DeletedBy",
                table: "StandardWeightMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StandardWeightMaster_DepartmentId",
                table: "StandardWeightMaster",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardWeightMaster_ModifiedBy",
                table: "StandardWeightMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StandardWeightMaster_StandardWeightBoxMasterId",
                table: "StandardWeightMaster",
                column: "StandardWeightBoxMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardWeightMaster_SubPlantId",
                table: "StandardWeightMaster",
                column: "SubPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_StandardWeightMaster_UnitOfMeasurementId",
                table: "StandardWeightMaster",
                column: "UnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_StateMaster_CountryId",
                table: "StateMaster",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_StateMaster_CreatedBy",
                table: "StateMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StateMaster_DeletedBy",
                table: "StateMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StateMaster_ModifiedBy",
                table: "StateMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StatusMaster_CreatedBy",
                table: "StatusMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StatusMaster_DeletedBy",
                table: "StatusMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StatusMaster_ModifiedBy",
                table: "StatusMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_StatusMaster_ModuleId",
                table: "StatusMaster",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusMaster_SubModuleId",
                table: "StatusMaster",
                column: "SubModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_SubModuleMaster_CreatedBy",
                table: "SubModuleMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubModuleMaster_DeletedBy",
                table: "SubModuleMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubModuleMaster_ModifiedBy",
                table: "SubModuleMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubModuleMaster_SubModuleTypeId",
                table: "SubModuleMaster",
                column: "SubModuleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubModuleTypeMaster_CreatedBy",
                table: "SubModuleTypeMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubModuleTypeMaster_DeletedBy",
                table: "SubModuleTypeMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SubModuleTypeMaster_ModifiedBy",
                table: "SubModuleTypeMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TenantNotifications_TenantId",
                table: "TenantNotifications",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_CreatedBy",
                table: "Tenants",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_DeletedBy",
                table: "Tenants",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_EditionId",
                table: "Tenants",
                column: "EditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_ModifiedBy",
                table: "Tenants",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_TenancyName",
                table: "Tenants",
                column: "TenancyName");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStatusMaster_CreatedBy",
                table: "TransactionStatusMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStatusMaster_DeletedBy",
                table: "TransactionStatusMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionStatusMaster_ModifiedBy",
                table: "TransactionStatusMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurementMaster_CreatedBy",
                table: "UnitOfMeasurementMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurementMaster_DeletedBy",
                table: "UnitOfMeasurementMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurementMaster_ModifiedBy",
                table: "UnitOfMeasurementMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurementMaster_UnitOfMeasurementTypeId",
                table: "UnitOfMeasurementMaster",
                column: "UnitOfMeasurementTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurementTypeMaster_CreatedBy",
                table: "UnitOfMeasurementTypeMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurementTypeMaster_DeletedBy",
                table: "UnitOfMeasurementTypeMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UnitOfMeasurementTypeMaster_ModifiedBy",
                table: "UnitOfMeasurementTypeMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_EmailAddress",
                table: "UserAccounts",
                column: "EmailAddress");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_UserName",
                table: "UserAccounts",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_TenantId_EmailAddress",
                table: "UserAccounts",
                columns: new[] { "TenantId", "EmailAddress" });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_TenantId_UserId",
                table: "UserAccounts",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_TenantId_UserName",
                table: "UserAccounts",
                columns: new[] { "TenantId", "UserName" });

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_TenantId_ClaimType",
                table: "UserClaims",
                columns: new[] { "TenantId", "ClaimType" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginAttempts_UserId_TenantId",
                table: "UserLoginAttempts",
                columns: new[] { "UserId", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginAttempts_TenancyName_UserNameOrEmailAddress_Result",
                table: "UserLoginAttempts",
                columns: new[] { "TenancyName", "UserNameOrEmailAddress", "Result" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_TenantId_UserId",
                table: "UserLogins",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_TenantId_LoginProvider_ProviderKey",
                table: "UserLogins",
                columns: new[] { "TenantId", "LoginProvider", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserId_State_CreationTime",
                table: "UserNotifications",
                columns: new[] { "UserId", "State", "CreationTime" });

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganizationUnits_TenantId_OrganizationUnitId",
                table: "UserOrganizationUnits",
                columns: new[] { "TenantId", "OrganizationUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganizationUnits_TenantId_UserId",
                table: "UserOrganizationUnits",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserPlants_PlantId",
                table: "UserPlants",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlants_UserId",
                table: "UserPlants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_TenantId_RoleId",
                table: "UserRoles",
                columns: new[] { "TenantId", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_TenantId_UserId",
                table: "UserRoles",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_ApprovalStatusId",
                table: "Users",
                column: "ApprovalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedBy",
                table: "Users",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DeletedBy",
                table: "Users",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DesignationId",
                table: "Users",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ModifiedBy",
                table: "Users",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ModeId",
                table: "Users",
                column: "ModeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PlantId",
                table: "Users",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReportingManagerId",
                table: "Users",
                column: "ReportingManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId_NormalizedEmailAddress",
                table: "Users",
                columns: new[] { "TenantId", "NormalizedEmailAddress" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId_NormalizedUserName",
                table: "Users",
                columns: new[] { "TenantId", "NormalizedUserName" });

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId",
                table: "UserTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_TenantId_UserId",
                table: "UserTokens",
                columns: new[] { "TenantId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspectionDetails_CheckpointId",
                table: "VehicleInspectionDetails",
                column: "CheckpointId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspectionDetails_CreatedBy",
                table: "VehicleInspectionDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspectionDetails_DeletedBy",
                table: "VehicleInspectionDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspectionDetails_ModifiedBy",
                table: "VehicleInspectionDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspectionDetails_VehicleInspectionHeaderId",
                table: "VehicleInspectionDetails",
                column: "VehicleInspectionHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspectionHeaders_ChecklistTypeId",
                table: "VehicleInspectionHeaders",
                column: "ChecklistTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspectionHeaders_CreatedBy",
                table: "VehicleInspectionHeaders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspectionHeaders_DeletedBy",
                table: "VehicleInspectionHeaders",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspectionHeaders_GateEntryId",
                table: "VehicleInspectionHeaders",
                column: "GateEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspectionHeaders_InspectionChecklistId",
                table: "VehicleInspectionHeaders",
                column: "InspectionChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspectionHeaders_InvoiceId",
                table: "VehicleInspectionHeaders",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspectionHeaders_ModifiedBy",
                table: "VehicleInspectionHeaders",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleInspectionHeaders_TransactionStatusId",
                table: "VehicleInspectionHeaders",
                column: "TransactionStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_WebhookSendAttempts_WebhookEventId",
                table: "WebhookSendAttempts",
                column: "WebhookEventId");

            migrationBuilder.CreateIndex(
                name: "IX_WeighingMachineMaster_CreatedBy",
                table: "WeighingMachineMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WeighingMachineMaster_DeletedBy",
                table: "WeighingMachineMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WeighingMachineMaster_ModifiedBy",
                table: "WeighingMachineMaster",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WeighingMachineMaster_SubPlantId",
                table: "WeighingMachineMaster",
                column: "SubPlantId");

            migrationBuilder.CreateIndex(
                name: "IX_WeighingMachineMaster_UnitOfMeasurementId",
                table: "WeighingMachineMaster",
                column: "UnitOfMeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_WeighingMachineTestConfigurations_CreatedBy",
                table: "WeighingMachineTestConfigurations",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WeighingMachineTestConfigurations_DeletedBy",
                table: "WeighingMachineTestConfigurations",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WeighingMachineTestConfigurations_FrequencyTypeId",
                table: "WeighingMachineTestConfigurations",
                column: "FrequencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WeighingMachineTestConfigurations_ModifiedBy",
                table: "WeighingMachineTestConfigurations",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WeighingMachineTestConfigurations_WeighingMachineId",
                table: "WeighingMachineTestConfigurations",
                column: "WeighingMachineId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightCaptureDetails_CreatedBy",
                table: "WeightCaptureDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WeightCaptureDetails_DeletedBy",
                table: "WeightCaptureDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WeightCaptureDetails_ModifiedBy",
                table: "WeightCaptureDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WeightCaptureDetails_WeighingMachineId",
                table: "WeightCaptureDetails",
                column: "WeighingMachineId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightCaptureDetails_WeightCaptureHeaderId",
                table: "WeightCaptureDetails",
                column: "WeightCaptureHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightCaptureHeaders_CreatedBy",
                table: "WeightCaptureHeaders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WeightCaptureHeaders_DeletedBy",
                table: "WeightCaptureHeaders",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WeightCaptureHeaders_InvoiceId",
                table: "WeightCaptureHeaders",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightCaptureHeaders_ModifiedBy",
                table: "WeightCaptureHeaders",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WeightCaptureHeaders_MfgBatchNoId",
                table: "WeightCaptureHeaders",
                column: "MfgBatchNoId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightCaptureHeaders_PurchaseOrderId",
                table: "WeightCaptureHeaders",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_WeightVerificationHeader_CreatedBy",
                table: "WeightVerificationHeader",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WeightVerificationHeader_DeletedBy",
                table: "WeightVerificationHeader",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WeightVerificationHeader_ModifiedBy",
                table: "WeightVerificationHeader",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WIPLineClearanceCheckpoints_CreatedBy",
                table: "WIPLineClearanceCheckpoints",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WIPLineClearanceCheckpoints_DeletedBy",
                table: "WIPLineClearanceCheckpoints",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WIPLineClearanceCheckpoints_ModifiedBy",
                table: "WIPLineClearanceCheckpoints",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WIPLineClearanceCheckpoints_LineClearanceTransactionId",
                table: "WIPLineClearanceCheckpoints",
                column: "LineClearanceTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_WIPLineClearanceTransaction_CreatedBy",
                table: "WIPLineClearanceTransaction",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WIPLineClearanceTransaction_DeletedBy",
                table: "WIPLineClearanceTransaction",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WIPLineClearanceTransaction_ModifiedBy",
                table: "WIPLineClearanceTransaction",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WIPMaterialVerification_CreatedBy",
                table: "WIPMaterialVerification",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WIPMaterialVerification_DeletedBy",
                table: "WIPMaterialVerification",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WIPMaterialVerification_ModifiedBy",
                table: "WIPMaterialVerification",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibratedLatestMachineDetails_CreatedBy",
                table: "WMCalibratedLatestMachineDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibratedLatestMachineDetails_DeletedBy",
                table: "WMCalibratedLatestMachineDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibratedLatestMachineDetails_ModifiedBy",
                table: "WMCalibratedLatestMachineDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibratedLatestMachineDetails_WMCalibrationHeaderId",
                table: "WMCalibratedLatestMachineDetails",
                column: "WMCalibrationHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibratedLatestMachineDetails_WeighingMachineId",
                table: "WMCalibratedLatestMachineDetails",
                column: "WeighingMachineId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationCheckpoints_CheckPointId",
                table: "WMCalibrationCheckpoints",
                column: "CheckPointId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationCheckpoints_CreatedBy",
                table: "WMCalibrationCheckpoints",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationCheckpoints_DeletedBy",
                table: "WMCalibrationCheckpoints",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationCheckpoints_ModifiedBy",
                table: "WMCalibrationCheckpoints",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationCheckpoints_WMCalibrationHeaderId",
                table: "WMCalibrationCheckpoints",
                column: "WMCalibrationHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationDetails_CalibrationLevelId",
                table: "WMCalibrationDetails",
                column: "CalibrationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationDetails_CalibrationStatusId",
                table: "WMCalibrationDetails",
                column: "CalibrationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationDetails_CreatedBy",
                table: "WMCalibrationDetails",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationDetails_DeletedBy",
                table: "WMCalibrationDetails",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationDetails_ModifiedBy",
                table: "WMCalibrationDetails",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationDetails_StandardWeightBoxId",
                table: "WMCalibrationDetails",
                column: "StandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationDetails_WMCalibrationHeaderId",
                table: "WMCalibrationDetails",
                column: "WMCalibrationHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationDetailWeights_CreatedBy",
                table: "WMCalibrationDetailWeights",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationDetailWeights_DeletedBy",
                table: "WMCalibrationDetailWeights",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationDetailWeights_ModifiedBy",
                table: "WMCalibrationDetailWeights",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationDetailWeights_StandardWeightId",
                table: "WMCalibrationDetailWeights",
                column: "StandardWeightId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationDetailWeights_WMCalibrationDetailId",
                table: "WMCalibrationDetailWeights",
                column: "WMCalibrationDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationDetailWeights_WMCalibrationEccentricityTestId",
                table: "WMCalibrationDetailWeights",
                column: "WMCalibrationEccentricityTestId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationDetailWeights_WMCalibrationLinearityTestId",
                table: "WMCalibrationDetailWeights",
                column: "WMCalibrationLinearityTestId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationDetailWeights_WMCalibrationRepeatabilityTestId",
                table: "WMCalibrationDetailWeights",
                column: "WMCalibrationRepeatabilityTestId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationEccentricityTests_CValueStandardWeightBoxId",
                table: "WMCalibrationEccentricityTests",
                column: "CValueStandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationEccentricityTests_CreatedBy",
                table: "WMCalibrationEccentricityTests",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationEccentricityTests_DeletedBy",
                table: "WMCalibrationEccentricityTests",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationEccentricityTests_LBValueStandardWeightBoxId",
                table: "WMCalibrationEccentricityTests",
                column: "LBValueStandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationEccentricityTests_LFValueStandardWeightBoxId",
                table: "WMCalibrationEccentricityTests",
                column: "LFValueStandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationEccentricityTests_ModifiedBy",
                table: "WMCalibrationEccentricityTests",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationEccentricityTests_RBValueStandardWeightBoxId",
                table: "WMCalibrationEccentricityTests",
                column: "RBValueStandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationEccentricityTests_RFValueStandardWeightBoxId",
                table: "WMCalibrationEccentricityTests",
                column: "RFValueStandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationEccentricityTests_TestResultId",
                table: "WMCalibrationEccentricityTests",
                column: "TestResultId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationEccentricityTests_WMCalibrationHeaderId",
                table: "WMCalibrationEccentricityTests",
                column: "WMCalibrationHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationHeaders_CalibrationFrequencyId",
                table: "WMCalibrationHeaders",
                column: "CalibrationFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationHeaders_CalibrationStatusId",
                table: "WMCalibrationHeaders",
                column: "CalibrationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationHeaders_ChecklistTypeId",
                table: "WMCalibrationHeaders",
                column: "ChecklistTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationHeaders_CreatedBy",
                table: "WMCalibrationHeaders",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationHeaders_DeletedBy",
                table: "WMCalibrationHeaders",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationHeaders_InspectionChecklistId",
                table: "WMCalibrationHeaders",
                column: "InspectionChecklistId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationHeaders_ModifiedBy",
                table: "WMCalibrationHeaders",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationHeaders_WeighingMachineId",
                table: "WMCalibrationHeaders",
                column: "WeighingMachineId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationLinearityTests_CreatedBy",
                table: "WMCalibrationLinearityTests",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationLinearityTests_DeletedBy",
                table: "WMCalibrationLinearityTests",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationLinearityTests_ModifiedBy",
                table: "WMCalibrationLinearityTests",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationLinearityTests_TestResultId",
                table: "WMCalibrationLinearityTests",
                column: "TestResultId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationLinearityTests_WMCalibrationHeaderId",
                table: "WMCalibrationLinearityTests",
                column: "WMCalibrationHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationLinearityTests_WeightValue1StandardWeightBoxId",
                table: "WMCalibrationLinearityTests",
                column: "WeightValue1StandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationLinearityTests_WeightValue2StandardWeightBoxId",
                table: "WMCalibrationLinearityTests",
                column: "WeightValue2StandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationLinearityTests_WeightValue3StandardWeightBoxId",
                table: "WMCalibrationLinearityTests",
                column: "WeightValue3StandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationLinearityTests_WeightValue4StandardWeightBoxId",
                table: "WMCalibrationLinearityTests",
                column: "WeightValue4StandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationLinearityTests_WeightValue5StandardWeightBoxId",
                table: "WMCalibrationLinearityTests",
                column: "WeightValue5StandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationRepeatabilityTests_CreatedBy",
                table: "WMCalibrationRepeatabilityTests",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationRepeatabilityTests_DeletedBy",
                table: "WMCalibrationRepeatabilityTests",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationRepeatabilityTests_ModifiedBy",
                table: "WMCalibrationRepeatabilityTests",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationRepeatabilityTests_TestResultId",
                table: "WMCalibrationRepeatabilityTests",
                column: "TestResultId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationRepeatabilityTests_WMCalibrationHeaderId",
                table: "WMCalibrationRepeatabilityTests",
                column: "WMCalibrationHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationRepeatabilityTests_WeightValue10StandardWeightB~",
                table: "WMCalibrationRepeatabilityTests",
                column: "WeightValue10StandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationRepeatabilityTests_WeightValue1StandardWeightBo~",
                table: "WMCalibrationRepeatabilityTests",
                column: "WeightValue1StandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationRepeatabilityTests_WeightValue2StandardWeightBo~",
                table: "WMCalibrationRepeatabilityTests",
                column: "WeightValue2StandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationRepeatabilityTests_WeightValue3StandardWeightBo~",
                table: "WMCalibrationRepeatabilityTests",
                column: "WeightValue3StandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationRepeatabilityTests_WeightValue4StandardWeightBo~",
                table: "WMCalibrationRepeatabilityTests",
                column: "WeightValue4StandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationRepeatabilityTests_WeightValue5StandardWeightBo~",
                table: "WMCalibrationRepeatabilityTests",
                column: "WeightValue5StandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationRepeatabilityTests_WeightValue6StandardWeightBo~",
                table: "WMCalibrationRepeatabilityTests",
                column: "WeightValue6StandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationRepeatabilityTests_WeightValue7StandardWeightBo~",
                table: "WMCalibrationRepeatabilityTests",
                column: "WeightValue7StandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationRepeatabilityTests_WeightValue8StandardWeightBo~",
                table: "WMCalibrationRepeatabilityTests",
                column: "WeightValue8StandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationRepeatabilityTests_WeightValue9StandardWeightBo~",
                table: "WMCalibrationRepeatabilityTests",
                column: "WeightValue9StandardWeightBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationUncertainityTests_CreatedBy",
                table: "WMCalibrationUncertainityTests",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationUncertainityTests_DeletedBy",
                table: "WMCalibrationUncertainityTests",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationUncertainityTests_ModifiedBy",
                table: "WMCalibrationUncertainityTests",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationUncertainityTests_TestResultId",
                table: "WMCalibrationUncertainityTests",
                column: "TestResultId");

            migrationBuilder.CreateIndex(
                name: "IX_WMCalibrationUncertainityTests_WMCalibrationHeaderId",
                table: "WMCalibrationUncertainityTests",
                column: "WMCalibrationHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_WMSPasswordManager_CreatedBy",
                table: "WMSPasswordManager",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMSPasswordManager_DeletedBy",
                table: "WMSPasswordManager",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WMSPasswordManager_ModifiedBy",
                table: "WMSPasswordManager",
                column: "ModifiedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ZMaster_CreatedBy",
                table: "ZMaster",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ZMaster_DeletedBy",
                table: "ZMaster",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ZMaster_ModifiedBy",
                table: "ZMaster",
                column: "ModifiedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_CreatedBy",
                table: "Roles",
                column: "CreatedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_DeletedBy",
                table: "Roles",
                column: "DeletedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_ModifiedBy",
                table: "Roles",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_ApprovalStatusMaster_ApprovalStatusId",
                table: "Roles",
                column: "ApprovalStatusId",
                principalTable: "ApprovalStatusMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_PlantMaster_PlantId",
                table: "Users",
                column: "PlantId",
                principalTable: "PlantMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ModeMaster_ModeId",
                table: "Users",
                column: "ModeId",
                principalTable: "ModeMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_ApprovalStatusMaster_ApprovalStatusId",
                table: "Users",
                column: "ApprovalStatusId",
                principalTable: "ApprovalStatusMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_DesignationMaster_DesignationId",
                table: "Users",
                column: "DesignationId",
                principalTable: "DesignationMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalStatusMaster_Users_CreatedBy",
                table: "ApprovalStatusMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalStatusMaster_Users_DeletedBy",
                table: "ApprovalStatusMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalStatusMaster_Users_ModifiedBy",
                table: "ApprovalStatusMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_CountryMaster_Users_CreatedBy",
                table: "CountryMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_CountryMaster_Users_DeletedBy",
                table: "CountryMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_CountryMaster_Users_ModifiedBy",
                table: "CountryMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_DesignationMaster_Users_CreatedBy",
                table: "DesignationMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_DesignationMaster_Users_DeletedBy",
                table: "DesignationMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_DesignationMaster_Users_ModifiedBy",
                table: "DesignationMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_ModeMaster_Users_CreatedBy",
                table: "ModeMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_ModeMaster_Users_DeletedBy",
                table: "ModeMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_ModeMaster_Users_ModifiedBy",
                table: "ModeMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantMaster_Users_CreatedBy",
                table: "PlantMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantMaster_Users_DeletedBy",
                table: "PlantMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_PlantMaster_Users_ModifiedBy",
                table: "PlantMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_StateMaster_Users_CreatedBy",
                table: "StateMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_StateMaster_Users_DeletedBy",
                table: "StateMaster");

            migrationBuilder.DropForeignKey(
                name: "FK_StateMaster_Users_ModifiedBy",
                table: "StateMaster");

            migrationBuilder.DropTable(
                name: "ActivityMaster");

            migrationBuilder.DropTable(
                name: "ApprovalLevelMaster");

            migrationBuilder.DropTable(
                name: "ApprovalUserModuleMappingMaster");

            migrationBuilder.DropTable(
                name: "AreaUsageListLog");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "AuditSQLResult");

            migrationBuilder.DropTable(
                name: "BackgroundJobs");

            migrationBuilder.DropTable(
                name: "CageLabelPrinting");

            migrationBuilder.DropTable(
                name: "CalenderMaster");

            migrationBuilder.DropTable(
                name: "CheckpointTypeMaster");

            migrationBuilder.DropTable(
                name: "CompRecipeTransDetlMapping");

            migrationBuilder.DropTable(
                name: "ConsumptionDetails");

            migrationBuilder.DropTable(
                name: "CubicalRecipeTranDetlMapping");

            migrationBuilder.DropTable(
                name: "CubicleAssignmentDetails");

            migrationBuilder.DropTable(
                name: "CubicleAssignmentWIP");

            migrationBuilder.DropTable(
                name: "CubicleCleaningCheckpoints");

            migrationBuilder.DropTable(
                name: "CubicleCleaningDailyStatus");

            migrationBuilder.DropTable(
                name: "DispatchDetails");

            migrationBuilder.DropTable(
                name: "DispensingPrintDetails");

            migrationBuilder.DropTable(
                name: "DynamicEntityPropertyValues");

            migrationBuilder.DropTable(
                name: "DynamicPropertyValues");

            migrationBuilder.DropTable(
                name: "Elog_ClientForms");

            migrationBuilder.DropTable(
                name: "Elog_ElogControls");

            migrationBuilder.DropTable(
                name: "EntityPropertyChanges");

            migrationBuilder.DropTable(
                name: "EquipmentAssignments");

            migrationBuilder.DropTable(
                name: "EquipmentCleaningCheckpoints");

            migrationBuilder.DropTable(
                name: "EquipmentCleaningStatus");

            migrationBuilder.DropTable(
                name: "EquipmentUsageLogList");

            migrationBuilder.DropTable(
                name: "Features");

            migrationBuilder.DropTable(
                name: "FgPicking");

            migrationBuilder.DropTable(
                name: "FgPutAway");

            migrationBuilder.DropTable(
                name: "FormApprovalData");

            migrationBuilder.DropTable(
                name: "GateMaster");

            migrationBuilder.DropTable(
                name: "GRNMaterialLabelPrintingDetails");

            migrationBuilder.DropTable(
                name: "InProcessLabelDetails");

            migrationBuilder.DropTable(
                name: "IssueToProductions");

            migrationBuilder.DropTable(
                name: "LabelPrintPacking");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "LanguageTexts");

            migrationBuilder.DropTable(
                name: "LineClearanceCheckpoints");

            migrationBuilder.DropTable(
                name: "Loading");

            migrationBuilder.DropTable(
                name: "LogFormHistory");

            migrationBuilder.DropTable(
                name: "LogoMaster");

            migrationBuilder.DropTable(
                name: "MaterialBatchDispensingContainerDetails");

            migrationBuilder.DropTable(
                name: "MaterialChecklistDetails");

            migrationBuilder.DropTable(
                name: "MaterialDamageDetails");

            migrationBuilder.DropTable(
                name: "MaterialDestructions");

            migrationBuilder.DropTable(
                name: "MaterialReturnDetails");

            migrationBuilder.DropTable(
                name: "MaterialReturnDetailsSAP");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "NotificationSubscriptions");

            migrationBuilder.DropTable(
                name: "OBDDetails");

            migrationBuilder.DropTable(
                name: "OrganizationUnitRoles");

            migrationBuilder.DropTable(
                name: "OrganizationUnits");

            migrationBuilder.DropTable(
                name: "PackingMaster");

            migrationBuilder.DropTable(
                name: "Palletizations");

            migrationBuilder.DropTable(
                name: "PalletMaster");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "PickingMaster");

            migrationBuilder.DropTable(
                name: "PostWIPDataToSAP");

            migrationBuilder.DropTable(
                name: "PRNEntryMaster");

            migrationBuilder.DropTable(
                name: "ProcessOrderAfterRelease");

            migrationBuilder.DropTable(
                name: "ProcessOrderMaterialAfterRelease");

            migrationBuilder.DropTable(
                name: "Putaway");

            migrationBuilder.DropTable(
                name: "PutAwayBinToBinTransfer");

            migrationBuilder.DropTable(
                name: "RecipeMaster");

            migrationBuilder.DropTable(
                name: "RecipeWiseProcessOrderMapping");

            migrationBuilder.DropTable(
                name: "ReportConfiguration");

            migrationBuilder.DropTable(
                name: "ReturnToVendorDetail");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "SampleDestructions");

            migrationBuilder.DropTable(
                name: "SAPGRNPosting");

            migrationBuilder.DropTable(
                name: "SAPPlantMaster");

            migrationBuilder.DropTable(
                name: "SAPProcessOrderReceivedMaterials");

            migrationBuilder.DropTable(
                name: "SAPProcessOrders");

            migrationBuilder.DropTable(
                name: "SAPQualityControlDetails");

            migrationBuilder.DropTable(
                name: "SAPReturntoMaterial");

            migrationBuilder.DropTable(
                name: "SAPUOMMaster");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "StageOutDetails");

            migrationBuilder.DropTable(
                name: "TenantNotifications");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLoginAttempts");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserNotifications");

            migrationBuilder.DropTable(
                name: "UserOrganizationUnits");

            migrationBuilder.DropTable(
                name: "UserPlants");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "VehicleInspectionDetails");

            migrationBuilder.DropTable(
                name: "WebhookSendAttempts");

            migrationBuilder.DropTable(
                name: "WebhookSubscriptions");

            migrationBuilder.DropTable(
                name: "WeighingMachineTestConfigurations");

            migrationBuilder.DropTable(
                name: "WeightCaptureDetails");

            migrationBuilder.DropTable(
                name: "WeightVerificationHeader");

            migrationBuilder.DropTable(
                name: "WIPLineClearanceCheckpoints");

            migrationBuilder.DropTable(
                name: "WIPMaterialVerification");

            migrationBuilder.DropTable(
                name: "WMCalibratedLatestMachineDetails");

            migrationBuilder.DropTable(
                name: "WMCalibrationCheckpoints");

            migrationBuilder.DropTable(
                name: "WMCalibrationDetailWeights");

            migrationBuilder.DropTable(
                name: "WMCalibrationUncertainityTests");

            migrationBuilder.DropTable(
                name: "WMSPasswordManager");

            migrationBuilder.DropTable(
                name: "ZMaster");

            migrationBuilder.DropTable(
                name: "AreaUsageLog");

            migrationBuilder.DropTable(
                name: "HolidayTypeMaster");

            migrationBuilder.DropTable(
                name: "MaterialMaster");

            migrationBuilder.DropTable(
                name: "Consumption");

            migrationBuilder.DropTable(
                name: "RecipeTransactionDetails");

            migrationBuilder.DropTable(
                name: "ProcessOrderMaterials");

            migrationBuilder.DropTable(
                name: "CubicleCleaningTransactions");

            migrationBuilder.DropTable(
                name: "DispensingDetails");

            migrationBuilder.DropTable(
                name: "DynamicEntityProperties");

            migrationBuilder.DropTable(
                name: "EntityChanges");

            migrationBuilder.DropTable(
                name: "EquipmentCleaningTransactions");

            migrationBuilder.DropTable(
                name: "EquipmentUsageLog");

            migrationBuilder.DropTable(
                name: "DeviceMaster");

            migrationBuilder.DropTable(
                name: "LineClearanceTransactions");

            migrationBuilder.DropTable(
                name: "MaterialBatchDispensingHeaders");

            migrationBuilder.DropTable(
                name: "GRNMaterialLabelPrintingContainerBarcodes");

            migrationBuilder.DropTable(
                name: "LocationMaster");

            migrationBuilder.DropTable(
                name: "MaterialTransferTypeMaster");

            migrationBuilder.DropTable(
                name: "HandlingUnitMaster");

            migrationBuilder.DropTable(
                name: "ReturnToVendorHeader");

            migrationBuilder.DropTable(
                name: "ModuleSubModule");

            migrationBuilder.DropTable(
                name: "PermissionMaster");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "StageOutHeaders");

            migrationBuilder.DropTable(
                name: "Editions");

            migrationBuilder.DropTable(
                name: "VehicleInspectionHeaders");

            migrationBuilder.DropTable(
                name: "WebhookEvents");

            migrationBuilder.DropTable(
                name: "WeightCaptureHeaders");

            migrationBuilder.DropTable(
                name: "WIPLineClearanceTransaction");

            migrationBuilder.DropTable(
                name: "CheckpointMaster");

            migrationBuilder.DropTable(
                name: "StandardWeightMaster");

            migrationBuilder.DropTable(
                name: "WMCalibrationDetails");

            migrationBuilder.DropTable(
                name: "WMCalibrationEccentricityTests");

            migrationBuilder.DropTable(
                name: "WMCalibrationLinearityTests");

            migrationBuilder.DropTable(
                name: "WMCalibrationRepeatabilityTests");

            migrationBuilder.DropTable(
                name: "RecipeTransactionHeader");

            migrationBuilder.DropTable(
                name: "CubicleCleaningTypeMaster");

            migrationBuilder.DropTable(
                name: "DispensingHeaders");

            migrationBuilder.DropTable(
                name: "SamplingTypeMaster");

            migrationBuilder.DropTable(
                name: "DynamicProperties");

            migrationBuilder.DropTable(
                name: "EntityChangeSets");

            migrationBuilder.DropTable(
                name: "EquipmentCleaningTypeMaster");

            migrationBuilder.DropTable(
                name: "DeviceTypeMaster");

            migrationBuilder.DropTable(
                name: "CubicleAssignmentHeader");

            migrationBuilder.DropTable(
                name: "GRNMaterialLabelPrintingHeaders");

            migrationBuilder.DropTable(
                name: "GRNQtyDetails");

            migrationBuilder.DropTable(
                name: "HandlingUnitTypeMaster");

            migrationBuilder.DropTable(
                name: "CubicleMaster");

            migrationBuilder.DropTable(
                name: "CalibrationFrequencyMaster");

            migrationBuilder.DropTable(
                name: "CalibrationTestStatusMaster");

            migrationBuilder.DropTable(
                name: "WMCalibrationHeaders");

            migrationBuilder.DropTable(
                name: "StandardWeightBoxMaster");

            migrationBuilder.DropTable(
                name: "InspectionLot");

            migrationBuilder.DropTable(
                name: "ProcessOrders");

            migrationBuilder.DropTable(
                name: "EquipmentMaster");

            migrationBuilder.DropTable(
                name: "StatusMaster");

            migrationBuilder.DropTable(
                name: "GRNDetails");

            migrationBuilder.DropTable(
                name: "FrequencyTypeMaster");

            migrationBuilder.DropTable(
                name: "CalibrationStatusMaster");

            migrationBuilder.DropTable(
                name: "WeighingMachineMaster");

            migrationBuilder.DropTable(
                name: "AreaMaster");

            migrationBuilder.DropTable(
                name: "EquipmentTypeMaster");

            migrationBuilder.DropTable(
                name: "ModuleMaster");

            migrationBuilder.DropTable(
                name: "GRNHeaders");

            migrationBuilder.DropTable(
                name: "MaterialConsignmentDetails");

            migrationBuilder.DropTable(
                name: "UnitOfMeasurementMaster");

            migrationBuilder.DropTable(
                name: "DepartmentMaster");

            migrationBuilder.DropTable(
                name: "MaterialInspectionRelationDetails");

            migrationBuilder.DropTable(
                name: "UnitOfMeasurementTypeMaster");

            migrationBuilder.DropTable(
                name: "ChecklistTypeMaster");

            migrationBuilder.DropTable(
                name: "InspectionChecklistMaster");

            migrationBuilder.DropTable(
                name: "MaterialInspectionHeaders");

            migrationBuilder.DropTable(
                name: "Materials");

            migrationBuilder.DropTable(
                name: "SubModuleMaster");

            migrationBuilder.DropTable(
                name: "GateEntry");

            migrationBuilder.DropTable(
                name: "TransactionStatusMaster");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "SubModuleTypeMaster");

            migrationBuilder.DropTable(
                name: "InvoiceDetails");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ApprovalStatusMaster");

            migrationBuilder.DropTable(
                name: "DesignationMaster");

            migrationBuilder.DropTable(
                name: "ModeMaster");

            migrationBuilder.DropTable(
                name: "PlantMaster");

            migrationBuilder.DropTable(
                name: "StateMaster");

            migrationBuilder.DropTable(
                name: "CountryMaster");
        }
    }
}
