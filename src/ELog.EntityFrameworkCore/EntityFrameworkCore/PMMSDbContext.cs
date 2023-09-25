using Abp.Application.Editions;
using Abp.Authorization;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.BackgroundJobs;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Localization;
using Abp.Organizations;
using Abp.Zero.EntityFrameworkCore;
using Elog.Core.Entities;
using ELog.Core;
using ELog.Core.Authorization.Roles;
using ELog.Core.Authorization.Users;
using ELog.Core.Entities;
using ELog.Core.MultiTenancy;
using ELog.Core.SQLDtoEntities;
using ELog.EntityFrameworkCore.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ELog.EntityFrameworkCore.EntityFrameworkCore
{
    [ExcludeFromCodeCoverage]
    [AutoRepositoryTypes(
    typeof(IRepository<>),
    typeof(IRepository<,>),
    typeof(PMMSRepositoryBase<>),
    typeof(PMMSRepositoryBase<,>)
)]
    public class PMMSDbContext : AbpZeroDbContext<Tenant, Role, User, PMMSDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<ApprovalStatusMaster> ApprovalStatusMaster { get; set; }
        public DbSet<PlantMaster> PlantMaster { get; set; }

        public DbSet<RolePermissions> RolePermissions { get; set; }
        public DbSet<ModeMaster> ModeMaster { get; set; }
        public DbSet<Menu> Menu { get; set; }
        public DbSet<DesignationMaster> DesignationMaster { get; set; }
        public DbSet<ModuleMaster> ModuleMaster { get; set; }
        public DbSet<SubModuleMaster> SubModuleMaster { get; set; }
        public DbSet<PermissionMaster> PermissionMaster { get; set; }
        public DbSet<ModuleSubModule> ModuleSubModule { get; set; }

        public DbSet<AreaMaster> AreaMaster { get; set; }
        public DbSet<CalenderMaster> CalenderMaster { get; set; }
        public DbSet<CubicleMaster> CubicleMaster { get; set; }
        public DbSet<DepartmentMaster> DepartmentMaster { get; set; }
        public DbSet<EquipmentMaster> EquipmentMaster { get; set; }
        public DbSet<GateMaster> GateMaster { get; set; }
        public DbSet<HandlingUnitMaster> HandlingUnitMaster { get; set; }
        public DbSet<InspectionChecklistMaster> InspectionChecklistMaster { get; set; }
        public DbSet<LocationMaster> LocationMaster { get; set; }
        public DbSet<StandardWeightBoxMaster> StandardWeightBoxMaster { get; set; }
        public DbSet<StandardWeightMaster> StandardWeightMaster { get; set; }
        public DbSet<UnitOfMeasurementMaster> UnitOfMeasurementMaster { get; set; }
        public DbSet<WeighingMachineMaster> WeighingMachineMaster { get; set; }
        public DbSet<CountryMaster> CountryMaster { get; set; }
        public DbSet<StateMaster> StateMaster { get; set; }
        public DbSet<EquipmentTypeMaster> EquipmentTypeMaster { get; set; }
        public DbSet<HandlingUnitTypeMaster> HandlingTypeMaster { get; set; }
        public DbSet<SubModuleTypeMaster> SubModuleTypeMaster { get; set; }
        public DbSet<UnitOfMeasurementTypeMaster> UnitOfMeasurementTypeMaster { get; set; }
        public DbSet<DeviceMaster> DeviceMaster { get; set; }
        public DbSet<DeviceTypeMaster> DeviceTypeMaster { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<CalibrationFrequencyMaster> CalibrationFrequencyMaster { get; set; }
        public DbSet<ChecklistTypeMaster> ChecklistTypeMaster { get; set; }
        public DbSet<UserPlants> UserPlants { get; set; }
        public DbSet<CheckpointTypeMaster> CheckpointTypeMaster { get; set; }
        public DbSet<CheckpointMaster> CheckpointMaster { get; set; }
        public DbSet<GateEntry> GateEntries { get; set; }
        public DbSet<VehicleInspectionHeader> VehicleInspectionHeaders { get; set; }
        public DbSet<VehicleInspectionDetail> VehicleInspectionDetails { get; set; }

        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<MaterialInspectionHeader> MaterialInspectionHeaders { get; set; }
        public DbSet<MaterialConsignmentDetail> MaterialConsignmentDetails { get; set; }

        public DbSet<MaterialChecklistDetail> MaterialChecklistDetails { get; set; }
        public DbSet<MaterialDamageDetail> MaterialDamageDetails { get; set; }

        public DbSet<TransactionStatusMaster> TransactionStatusMaster { get; set; }
        public DbSet<WeightCaptureHeader> WeightCaptureHeaders { get; set; }
        public DbSet<WeightCaptureDetail> WeightCaptureDetails { get; set; }
        public DbSet<GRNHeader> GRNHeader { get; set; }
        public DbSet<GRNDetail> GRNDetail { get; set; }
        public DbSet<GRNQtyDetail> GRNQtyDetail { get; set; }
        public DbSet<MaterialInspectionRelationDetail> MaterialInspectionRelationDetails { get; set; }
        public DbSet<Palletization> Palletizations { get; set; }
        public DbSet<HolidayTypeMaster> HolidayTypeMaster { get; set; }
        public DbSet<RecipeMaster> RecipeMaster { get; set; }

        public DbSet<ApprovalLevelMaster> ApprovalLevelMaster { get; set; }
        public DbSet<ApprovalUserModuleMappingMaster> ApprovalUserModuleMappingMaster { get; set; }

        public DbSet<ActivityMaster> ActivityMaster { get; set; }
        public DbSet<RecipeTransactionHeader> RecipeTransactionHeaders { get; set; }
        public DbSet<RecipeTransactionDetails> RecipeTransactionDetails { get; set; }
        public DbSet<CubicalRecipeTranDetlMapping> CubicalRecipeTranDetlMapping { get; set; }
        public DbSet<RecipeWiseProcessOrderMapping> RecipeWiseProcessOrderMapping { get; set; }
        public DbSet<GRNMaterialLabelPrintingHeader> GRNMaterialLabelPrintingHeaders { get; set; }
        public DbSet<GRNMaterialLabelPrintingDetail> GRNMaterialLabelPrintingDetails { get; set; }
        public DbSet<GRNMaterialLabelPrintingContainerBarcode> GRNMaterialLabelPrintingContainerBarcodes { get; set; }
        public DbSet<MaterialTransferTypeMaster> MaterialTransferTypeMaster { get; set; }
        public DbSet<PutAwayBinToBinTransfer> PutAwayBinToBinTransfer { get; set; }
        public DbSet<MaterialMaster> MaterialMaster { get; set; }

        public DbSet<FrequencyTypeMaster> FrequencyTypeMaster { get; set; }
        public DbSet<CalibrationStatusMaster> CalibrationStatusMaster { get; set; }
        public DbSet<CalibrationTestStatusMaster> CalibrationTestStatusMaster { get; set; }
        public DbSet<WMCalibrationHeader> WMCalibrationHeaders { get; set; }
        public DbSet<WMCalibrationCheckpoint> WMCalibrationCheckpoints { get; set; }
        public DbSet<WMCalibrationDetail> WMCalibrationDetails { get; set; }
        public DbSet<WMCalibrationEccentricityTest> WMCalibrationEccentricityTests { get; set; }

        public DbSet<AuditReportDetailsDto> AuditSQLResult { get; set; }



        public DbSet<WMCalibrationLinearityTest> WMCalibrationLinearityTests { get; set; }
        public DbSet<WMCalibrationRepeatabilityTest> WMCalibrationRepeatabilityTests { get; set; }
        public DbSet<WMCalibrationDetailWeight> WMCalibrationDetailWeights { get; set; }
        public DbSet<WMCalibrationUncertainityTest> WMCalibrationUncertainityTests { get; set; }

        public DbSet<WMCalibratedLatestMachineDetail> WMCalibratedLatestMachineDetails { get; set; }
        public DbSet<WeighingMachineTestConfiguration> WeighingMachineTestConfigurations { get; set; }
        public DbSet<ProcessOrder> ProcessOrder { get; set; }
        public DbSet<ProcessOrderMaterial> ProcessOrderMaterial { get; set; }
        public DbSet<CubicleAssignmentHeader> CubicleAssignmentHeaders { get; set; }
        public DbSet<CubicleAssignmentDetail> CubicleAssignmentDetails { get; set; }
        public DbSet<StatusMaster> StatusMaster { get; set; }

        public DbSet<CubicleCleaningTypeMaster> CubicleCleaningTypeMaster { get; set; }
        public DbSet<CubicleCleaningTransaction> CubicleCleaningTransactions { get; set; }
        public DbSet<CubicleCleaningDailyStatus> CubicleCleaningDailyStatuses { get; set; }
        public DbSet<CubicleCleaningCheckpoint> CubicleCleaningCheckpoints { get; set; }

        public DbSet<EquipmentCleaningTypeMaster> EquipmentCleaningTypeMaster { get; set; }
        public DbSet<EquipmentCleaningTransaction> EquipmentCleaningTransactions { get; set; }
        public DbSet<EquipmentCleaningStatus> EquipmentCleaningStatuses { get; set; }
        public DbSet<EquipmentCleaningCheckpoint> EquipmentCleaningCheckpoints { get; set; }
        public DbSet<EquipmentAssignment> EquipmentAssignments { get; set; }
        public DbSet<SAPProcessOrder> SAPProcessOrders { get; set; }
        public DbSet<SAPReturntoMaterial> SAPReturntoMaterials { get; set; }
        public DbSet<SAPProcessOrderReceivedMaterial> SAPProcessOrderReceivedMaterials { get; set; }
        public DbSet<SAPQualityControlDetail> SAPQualityControlDetails { get; set; }
        public DbSet<LineClearanceTransaction> LineClearanceTransactions { get; set; }
        public DbSet<LineClearanceCheckpoint> LineClearanceCheckpoints { get; set; }

        public DbSet<SAPPlantMaster> SAPPlantMasters { get; set; }
        public DbSet<SAPUOMMaster> SAPUOMMasters { get; set; }
        public DbSet<SAPGRNPosting> SAPGRNPostings { get; set; }
        public DbSet<InspectionLot> InspectionLot { get; set; }
        public DbSet<MaterialBatchDispensingHeader> MaterialBatchDispensingHeaders { get; set; }
        public DbSet<MaterialBatchDispensingContainerDetail> MaterialBatchDispensingContainerDetails { get; set; }
        public DbSet<DispensingHeader> DispensingHeader { get; set; }
        public DbSet<DispensingDetail> DispensingDetail { get; set; }

        public DbSet<DispatchDetail> DispatchDetails { get; set; }
        public DbSet<DispensingPrintDetail> DispensingPrintDetail { get; set; }
        public DbSet<SamplingTypeMaster> SamplingTypeMaster { get; set; }
        public DbSet<StageOutHeader> StageOutHeaders { get; set; }
        public DbSet<StageOutDetail> StageOutDetails { get; set; }
        public DbSet<ReturnToVendorHeader> ReturnToVendorHeaders { get; set; }
        public DbSet<ReturnToVendorDetail> ReturnToVendorDetail { get; set; }
        public DbSet<MaterialDestruction> MaterialDestructions { get; set; }
        public DbSet<SampleDestruction> SampleDestruction { get; set; }
        public DbSet<PickingReportDto> PickingReportSQLResult { get; set; }
        public DbSet<ReportConfiguration> reportConfigurations { get; set; }
        public DbSet<IssueToProduction> IssueToProductions { get; set; }

        public DbSet<AreaUsageLog> AreaUsageLog { get; set; }

        public DbSet<AreaUsageListLog> AreaUsageListLog { get; set; }

        public DbSet<EquipmentUsageLog> EquipmentUsageLog { get; set; }

        public DbSet<EquipmentUsageLogList> EquipmentUsageLogList { get; set; }
        public DbSet<WeightVerificationHeader> WeightVerificationHeader { get; set; }
        //public DbSet<WeightVerificationDetail> WeightVerificationDetail { get; set; }

        public DbSet<CubicleAssignmentWIP> CubicleAssignmentWIP { get; set; }

        public DbSet<InProcessLabelDetails> InProcessLabelDetails { get; set; }
        public DbSet<Consumption> Consumptions { get; set; }
        public DbSet<ConsumptionDetails> ConsumptionDetails { get; set; }
        public DbSet<Putaway> Putaways { get; set; }

        public DbSet<ProcessOrderAfterRelease> ProcessOrderAfterRelease { get; set; }
        public DbSet<PickingMaster> PickingMaster { get; set; }

        public DbSet<PackingMaster> PackingMaster { get; set; }
        public DbSet<CompRecipeTransDetlMapping> CompRecipeTransDetlMapping { get; set; }

        public DbSet<ProcessOrderMaterialAfterRelease> ProcessOrderMaterialAfterRelease { get; set; }

        public DbSet<LabelPrintPacking> LabelPrintPacking { get; set; }

        public DbSet<PostWIPDataToSAP> PostWIPDataToSAP { get; set; }
        public DbSet<PalletMaster> PalletMaster { get; set; }
        public DbSet<MaterialReturn> MaterialReturn { get; set; }
        public DbSet<OBDDetails> OBDDetails { get; set; }

        public DbSet<FgPutAway> FgPutAway { get; set; }

        public DbSet<WMSPasswordManager> WMSPasswordManager { get; set; }

        public DbSet<WIPLineClearanceTransaction> WIPLineClearanceTransaction { get; set; }
        public DbSet<WIPLineClearanceCheckpoints> WIPLineClearanceCheckpoints { get; set; }

        public DbSet<LogoMaster> LogoMasters { get; set; }
        public DbSet<FgPicking> FgPicking { get; set; }
        public DbSet<Loading> Loading { get; set; }
        public DbSet<MaterialRteturnDetailsSAP> MaterialRteturnDetailsSAP { get; set; }
        public DbSet<CageLabelPrinting> CageLabelPrinting { get; set; }

        public DbSet<WIPMaterialVerification> WIPMaterialVerification { get; set; }

        public DbSet<PRNEntryMaster> PRNEntryMaster { get; set; }
        public DbSet<ClientForm> ClientForm { get; set; }
        public DbSet<ElogControl> ElogControl { get; set; }
        public DbSet<ZMaster> ZMaster { get; set; }

        public DbSet<FormApproval> FormApproval { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<LogFormHistory> LogFormHistory { get; set; }
        public PMMSDbContext(DbContextOptions<PMMSDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ChangeAbpTablePrefix<Tenant, Role, User>(""); //Removes table prefixes. You can specify another prefix.
            modelBuilder.ApplyUtcDateTimeConverter();
            ApplyUniqueConstraintColumns(modelBuilder);
            ApplyColumnNotNullables(modelBuilder);
            ApplyColumnDefaultValues(modelBuilder);
            RenameColumns(modelBuilder);
           // ApplySequence(modelBuilder);
            modelBuilder.Entity<UserPlants>().Ignore(x => x.DeleterUser);
            modelBuilder.Entity<UserPlants>().Ignore(x => x.CreatorUser);
            modelBuilder.Entity<UserPlants>().Ignore(x => x.LastModifierUser);
            modelBuilder.Entity<PickingReportDto>().HasNoKey().ToView(null);

            modelBuilder.Entity<UserPlants>()
                .HasOne<PlantMaster>(sc => sc.PlantMaster)
                .WithMany(s => s.UserPlants)
                .HasForeignKey(sc => sc.PlantId);

            modelBuilder.Entity<UserPlants>()
                .HasOne<User>(sc => sc.User)
                .WithMany(s => s.UserPlants)
                .HasForeignKey(sc => sc.UserId);
        }

        //public int GetNextUOMSequenceValue()
        //{
        //    if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
        //    {
        //        var p = new MySqlParameter("@result", System.Data.SqlDbType.Int)
        //        {
        //            Direction = System.Data.ParameterDirection.Output
        //        };

        //        Database.ExecuteSqlRaw("set @result = next value for UOMSequence", p);

        //        return (int)p.Value;
        //    }
        //    return 0;
        //}

        //public int GetNextGRNSequenceValue()
        //{
        //    if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
        //    {
        //        var p = new MySqlParameter("@result", System.Data.SqlDbType.Int)
        //        {
        //            Direction = System.Data.ParameterDirection.Output
        //        };

        //        Database.ExecuteSqlRaw("set @result = next value for GRNSequence", p);

        //        return (int)p.Value;
        //    }
        //    return 0;
        //}

        //public int GetNextGRNSAPBatchNumberSequenceValue()
        //{
        //    if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
        //    {
        //        var p = new MySqlParameter("@result", System.Data.SqlDbType.Int)
        //        {
        //            Direction = System.Data.ParameterDirection.Output
        //        };

        //        Database.ExecuteSqlRaw("set @result = next value for SAPBatchNumberSequence", p);

        //        return (int)p.Value;
        //    }
        //    return 0;
        //}
        //public int GetNextGRNLineItemSequenceValue()
        //{
        //    if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
        //    {
        //        var p = new MySqlParameter("@result", System.Data.SqlDbType.Int)
        //        {
        //            Direction = System.Data.ParameterDirection.Output
        //        };

        //        Database.ExecuteSqlRaw("set @result = next value for LineItemNumberSequence", p);

        //        return (int)p.Value;
        //    }
        //    return 0;
        //}
        //public void ResetLineItemNumberSequenceValue()
        //{
        //    if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
        //    {
        //        Database.ExecuteSqlRaw("ALTER SEQUENCE LineItemNumberSequence RESTART WITH 1");
        //    }
        //}

        //public object ExecuteStoreQuery<T>(string v, object product_id)
        //{
        //    throw new NotImplementedException();
        //}

        //public long GetNextGateEntryBarcodeSequenceValue()
        //{
        //    if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
        //    {
        //        var p = new MySqlParameter("@result", System.Data.SqlDbType.BigInt)
        //        {
        //            Direction = System.Data.ParameterDirection.Output
        //        };
        //        Database.ExecuteSqlRaw("set @result = next value for GateEntryBarcodeSequence", p);

        //        if ((long)p.Value == PMMSNumConsts.GateEntryBarcodeSequenceMaxValue)
        //        {
        //            Database.ExecuteSqlRaw("ALTER SEQUENCE GateEntryBarcodeSequence RESTART WITH 1");
        //            Database.ExecuteSqlRaw("set @result = next value for GateEntryBarcodeSequence", p);
        //        }
        //        return (long)p.Value;
        //    }
        //    return 0;
        //}

        //public long GetNextMaterialLabelBarcodeSequenceValue()
        //{
        //    if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
        //    {
        //        var p = new MySqlParameter("@result", System.Data.SqlDbType.BigInt)
        //        {
        //            Direction = System.Data.ParameterDirection.Output
        //        };
        //        Database.ExecuteSqlRaw("set @result = next value for MaterialLabelSequence", p);

        //        if ((long)p.Value == PMMSNumConsts.MaterialLabelBarcodeSequenceMaxValue)
        //        {
        //            Database.ExecuteSqlRaw("ALTER SEQUENCE MaterialLabelSequence RESTART WITH 1");
        //            Database.ExecuteSqlRaw("set @result = next value for MaterialLabelSequence", p);
        //        }
        //        return (long)p.Value;
        //    }
        //    return 0;
        //}

        //public long GetNextDispensingLabelBarcodeSequenceValue()
        //{
        //    if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
        //    {
        //        var p = new MySqlParameter("@result", System.Data.SqlDbType.BigInt)
        //        {
        //            Direction = System.Data.ParameterDirection.Output
        //        };
        //        Database.ExecuteSqlRaw("set @result = next value for DispensingLabelSequence", p);

        //        if ((long)p.Value == PMMSNumConsts.MaterialLabelBarcodeSequenceMaxValue)
        //        {
        //            Database.ExecuteSqlRaw("ALTER SEQUENCE DispensingLabelSequence RESTART WITH 1");
        //            Database.ExecuteSqlRaw("set @result = next value for DispensingLabelSequence", p);
        //        }
        //        return (long)p.Value;
        //    }
        //    return 0;
        //}

        //public long GetNextSamplingLabelBarcodeSequenceValue()
        //{
        //    if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
        //    {
        //        var p = new MySqlParameter("@result", System.Data.SqlDbType.BigInt)
        //        {
        //            Direction = System.Data.ParameterDirection.Output
        //        };
        //        Database.ExecuteSqlRaw("set @result = next value for SamplingLabelSequence", p);

        //        if ((long)p.Value == PMMSNumConsts.MaterialLabelBarcodeSequenceMaxValue)
        //        {
        //            Database.ExecuteSqlRaw("ALTER SEQUENCE SamplingLabelSequence RESTART WITH 1");
        //            Database.ExecuteSqlRaw("set @result = next value for SamplingLabelSequence", p);
        //        }
        //        return (long)p.Value;
        //    }
        //    return 0;
        //}


        //public long GetNextWIPProcessLabelBarcodeSequenceValue()
        //{
        //    if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
        //    {
        //        var p = new MySqlParameter("@result", System.Data.SqlDbType.BigInt)
        //        {
        //            Direction = System.Data.ParameterDirection.Output
        //        };
        //        Database.ExecuteSqlRaw("set @result = next value for ProcessLabelSequence", p);

        //        if ((long)p.Value == PMMSNumConsts.MaterialLabelBarcodeSequenceMaxValue)
        //        {
        //            Database.ExecuteSqlRaw("ALTER SEQUENCE ProcessLabelSequence RESTART WITH 1");
        //            Database.ExecuteSqlRaw("set @result = next value for ProcessLabelSequence", p);
        //        }
        //        return (long)p.Value;
        //    }
        //    return 0;
        //}


        //public long GetNextCageLabelPrintBarcodeSequenceValue()
        //{
        //    if (Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
        //    {
        //        var p = new MySqlParameter("@result", System.Data.SqlDbType.BigInt)
        //        {
        //            Direction = System.Data.ParameterDirection.Output
        //        };
        //        Database.ExecuteSqlRaw("set @result = next value for CageLabelSequence", p);

        //        if ((long)p.Value == PMMSNumConsts.GateEntryBarcodeSequenceMaxValue)
        //        {
        //            Database.ExecuteSqlRaw("ALTER SEQUENCE CageLabelSequence RESTART WITH 1");
        //            Database.ExecuteSqlRaw("set @result = next value for CageLabelSequence", p);
        //        }
        //        return (long)p.Value;
        //    }
        //    return 0;
        //}
        //private void ApplySequence(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.HasSequence<int>("UOMSequence");
        //    modelBuilder.HasSequence<long>("GateEntryBarcodeSequence");
        //    modelBuilder.HasSequence<long>("GRNSequence");
        //    modelBuilder.HasSequence<long>("SAPBatchNumberSequence");
        //    modelBuilder.HasSequence<long>("LineItemNumberSequence");
        //    modelBuilder.HasSequence<long>("MaterialLabelSequence");
        //    modelBuilder.HasSequence<long>("DispensingLabelSequence");
        //    modelBuilder.HasSequence<long>("SamplingLabelSequence");
        //    modelBuilder.HasSequence<long>("CageLabelSequence");

        //}

        private void ApplyColumnDefaultValues(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(p => p.IsActive).HasDefaultValue(true);
        }

        private void ApplyUniqueConstraintColumns(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApprovalStatusMaster>(x => x.HasIndex(x => x.ApprovalStatus).IsUnique(true));
            modelBuilder.Entity<CountryMaster>().HasIndex(u => u.CountryName).IsUnique();
        }

        private void ApplyColumnNotNullables(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApprovalStatusMaster>().Property(p => p.ApprovalStatus).IsRequired(true);
            modelBuilder.Entity<PlantMaster>().Property(p => p.PlantName).IsRequired(true);
            modelBuilder.Entity<RolePermissions>().Property(p => p.PermissionName).IsRequired(true);
            modelBuilder.Entity<Role>().Property(p => p.ApprovalStatusId).IsRequired(true);
            modelBuilder.Entity<ModuleMaster>().Property(p => p.Description).IsRequired(true);
            modelBuilder.Entity<SubModuleMaster>().Property(p => p.Description).IsRequired(true);
        }

        private void RenameColumns(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BackgroundJobInfo>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<BackgroundJobInfo>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");

            modelBuilder.Entity<Edition>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<Edition>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");
            modelBuilder.Entity<Edition>().Property(p => p.LastModificationTime).HasColumnName("ModifiedOn");
            modelBuilder.Entity<Edition>().Property(p => p.LastModifierUserId).HasColumnName("ModifiedBy");
            modelBuilder.Entity<Edition>().Property(p => p.DeletionTime).HasColumnName("DeletedOn");
            modelBuilder.Entity<Edition>().Property(p => p.DeleterUserId).HasColumnName("DeletedBy");

            modelBuilder.Entity<ApplicationLanguage>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<ApplicationLanguage>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");
            modelBuilder.Entity<ApplicationLanguage>().Property(p => p.LastModificationTime).HasColumnName("ModifiedOn");
            modelBuilder.Entity<ApplicationLanguage>().Property(p => p.LastModifierUserId).HasColumnName("ModifiedBy");
            modelBuilder.Entity<ApplicationLanguage>().Property(p => p.DeletionTime).HasColumnName("DeletedOn");
            modelBuilder.Entity<ApplicationLanguage>().Property(p => p.DeleterUserId).HasColumnName("DeletedBy");

            modelBuilder.Entity<ApplicationLanguageText>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<ApplicationLanguageText>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");
            modelBuilder.Entity<ApplicationLanguageText>().Property(p => p.LastModificationTime).HasColumnName("ModifiedOn");
            modelBuilder.Entity<ApplicationLanguageText>().Property(p => p.LastModifierUserId).HasColumnName("ModifiedBy");

            modelBuilder.Entity<OrganizationUnitRole>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<OrganizationUnitRole>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");

            modelBuilder.Entity<PermissionSetting>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<PermissionSetting>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");

            modelBuilder.Entity<RoleClaim>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<RoleClaim>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");

            modelBuilder.Entity<Setting>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<Setting>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");
            modelBuilder.Entity<Setting>().Property(p => p.LastModificationTime).HasColumnName("ModifiedOn");
            modelBuilder.Entity<Setting>().Property(p => p.LastModifierUserId).HasColumnName("ModifiedBy");

            modelBuilder.Entity<UserAccount>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<UserAccount>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");
            modelBuilder.Entity<UserAccount>().Property(p => p.LastModificationTime).HasColumnName("ModifiedOn");
            modelBuilder.Entity<UserAccount>().Property(p => p.LastModifierUserId).HasColumnName("ModifiedBy");
            modelBuilder.Entity<UserAccount>().Property(p => p.DeletionTime).HasColumnName("DeletedOn");
            modelBuilder.Entity<UserAccount>().Property(p => p.DeleterUserId).HasColumnName("DeletedBy");

            modelBuilder.Entity<UserClaim>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<UserClaim>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");

            modelBuilder.Entity<UserOrganizationUnit>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<UserOrganizationUnit>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");

            modelBuilder.Entity<UserRole>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
            modelBuilder.Entity<UserRole>().Property(p => p.CreatorUserId).HasColumnName("CreatedBy");

            modelBuilder.Entity<UserLoginAttempt>().Property(p => p.CreationTime).HasColumnName("CreatedOn");
        }
    }
}