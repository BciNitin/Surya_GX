namespace ELog.Core
{
    public static class PMMSConsts
    {
        public const string LocalizationSourceName = "ELog";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;
        public const string issueIndicator = "i";
        public const string RejectedStatus = "Rejected";
        public const string DefaultFromAddressValue = "admin@mydomain.com";
        public const string DefaultFromDisplayNameValue = "mydomain.com mailer";
        public const string AdminHostEmailAddress = "superadmin@bci.com";
        public const string SuperAdminUserName = "superadmin";
        public const string IsPrinterEnabledValue = "IsPrinterEnabled";
        public const string IsWeighigMachineEnabled = "IsWeighigMachineEnabled";
        public const string TenantAdminEmailAddress = "admin@bci.com";
        public const string ModuleSeedFilePath = "Module.Json";
        public const string ActionsSeedFilePath = "ActionMaster.Json";
        public const string ApprovalStatusSeedFilePath = "ApprovalStatusMaster.Json";
        public const string SubModuleTypeSeedFilePath = "SubModuleTypeMaster.Json";
        public const string SubModuleSeedFilePath = "SubModule.Json";
        public const string ModuleSubModuleSeedFilePath = "ModuleSubModule.Json";
        public const string CountryStateSeedFilePath = "Country_State.json";
        public const string EquipmentTypeMasterSeedFilePath = "EquipmentTypeMaster.Json";
        public const string DeviceTypeMasterSeedFilePath = "DeviceTypeMaster.Json";
        public const string UnitOfMeasurementTypeMaasterSeedFilePath = "UnitOfMeasurementTypeMaster.Json";
        public const string ModeMasterSeedFilePath = "ModeMaster.json";
        public const string SerilogSelfLogPath = "App_Data\\Logs\\SeriLogSelf.log";
        public const string CheckpointTypeMasterSeedFilePath = "CheckpointTypeMaster.Json";
        public const string TransactionStatusSeedFilePath = "TransactionStatusMaster.Json";
        public const string HolidayTypeMasterSeedFilePath = "HolidayTypeMaster.Json";
        public const string MaterialTransferTypeSeedFilePath = "MaterialTransferTypeMaster.Json";
        public const string FrequencyTypeMasterSeedFilePath = "FrequencyTypeMaster.Json";
        public const string CalibrationStatusMasterSeedFilePath = "CalibrationStatusMaster.Json";
        public const string CalibrationTestStatusMasterSeedFilePath = "CalibrationTestStatusMaster.Json";
        public const string CubicleCleaningTypeMasterSeedFilePath = "CubicleCleaningTypeMaster.json";
        public const string StatusSeedFilePath = "StatusMaster.Json";
        public const string EquipmentCleaningTypeMasterSeedFilePath = "EquipmentCleaningTypeMaster.json";
        public const string SamplingTypeMasterSeedFilePath = "SamplingTypeMaster.Json";
        public const int TotalStampingDays = 60;

        #region Log Property constants

        public const string LogPropertyBrowserInfo = "BrowserInfo";
        public const string LogPropertyClientIpAddress = "ClientIpAddress";
        public const string LogPropertyClientName = "ClientName";
        public const string LogPropertyCustomData = "CustomData";
        public const string LogPropertyException = "Exception";
        public const string LogPropertyExecutionDuration = "ExecutionDuration";
        public const string LogPropertyExecutionTime = "ExecutionTime";
        public const string LogPropertyImpersonatorTenantId = "ImpersonatorTenantId";
        public const string LogPropertyImpersonatorUserId = "ImpersonatorUserId";
        public const string LogPropertyMethodName = "MethodName";
        public const string LogPropertyParameters = "Parameters";
        public const string LogPropertyServiceName = "ServiceName";
        public const string LogPropertyTenantId = "TenantId";
        public const string LogPropertyUserId = "UserId";
        public const string LogPropertyReturnValue = "ReturnValue";
        public const string LogPropertyActionId = "ActionId";
        public const string LogPropertyActionName = "ActionName";
        public const string LogPropertyRequestPath = "RequestPath";
        public const string LogPropertyRequestId = "RequestId";
        public const string LogPropertySpanId = "SpanId";
        public const string LogPropertyParentId = "ParentId";
        public const string LogPropertyTraceId = "TraceId";
        public const string LogPropertyConnectionId = "ConnectionId";
        public const string LogPropertySourceContext = "SourceContext";
        public const string LogPropertyTimeStamp = "TimeStamp";

        #endregion Log Property constants

        #region Module SubModule

        public const string MasterModule = "Master";
        public const string AdminModule = "Admin";
        public const string InwardSubModule = "Inward";
        public const string DispensingSubModule = "Dispensing";
        public const string GateEntrySubModule = "GateEntry";
        public const string WeighingMachineDevice = "WeighingMachine";
        public const string CubicleAssignmentSubModule = "CubicleAssignment";
        public const string CubicleCleaningSubModule = "CubicleCleaning";
        public const string EquipmentCleaningSubModule = "EquipmentCleaning";
        public const string LineClearanceSubModule = "LineClearance";
        public const string AreaUsageLogSubModule = "AreaUsageLog";
        public const string PickingSubModule = "Picking";
        public const string PreStageSubModule = "Prestage";
        public const string StagingSubModule = "Staging";
        public const string SamplingSubModule = "Sampling";
        public const string StageOutSubModule = "StageOut";
        public const string ReturnToVendorSubModule = "ReturntoVendor";

        public const string WIPModule = "WIP";

        public const string ModuleName = "Module";
        public const string SubModuleName = "SubModule";
        public const string AddPermission = "Add";
        public const string DeletePermission = "Delete";
        public const string PrinterDevice = "Printer";

        public const string UsersMode = "Users";
        public const string IsCommonCreatorSetting = "IsCommonCreator";
        public const string IsCommonCreatorSettingValue = "false";
        public const string UserCreationLimitSetting = "UserCreationLimitSetting";
        public const string UserCreationLimitSettingValue = "50";
        public const string IsLoginWithAd = "IsLoginWithAd";
        public const string IsLoginWithAdValue = "false";
        public const string UserLockoutEnabledByDefault = "UserLockoutEnabledByDefault";
        public const string DefaultAccountLockoutTimeSpan = "DefaultAccountLockoutTimeSpan";
        public const string MaxFailedAccessAttemptsBeforeLockout = "MaxFailedAccessAttemptsBeforeLockout";
        public const string UserLockoutEnabledByDefaultValue = "true";
        public const string DefaultAccountLockoutTimeSpanValue = "7300";
        public const string MaxFailedAccessAttemptsBeforeLockoutValue = "3";
        public const string ActivitySubModuleNameForArea = "Area Usage Log";
        public const string ActivitySubModuleNameForEquipment = "Equipment Cleaning(WIP)";
        public const string SubModuleNameForRecipe = "Recipe Approval";



        #endregion Module SubModule

        #region Module SubModule Validations

        public const int MaxStandardWeightBoxIdLength = 64;
        public const int MaxStandardWeightIdLength = 64;
        public const int MaxInspectionChecklistCodeLength = 64;
        public const int MaxDepartmentIdLength = 64;
        public const int MaxAreaNameLength = 64;
        public const int MaxDepartmentNameLength = 64;
        public const int MaxInspectionChecklistNameLength = 64;
        public const int MaxAreaIdLength = 64;
        public const int Small = 50;
        public const int Medium = 100;
        public const int Large = 200;
        public const int Max = 500;

        #endregion Module SubModule Validations

        #region Inspection Checklist

        public const string PropertyInputValueRequired = "InputValueRequired";
        public const string PropertyAcceptanceValue = "AcceptanceValue";
        public const string PropertyValueTag = "ValueTag";
        public const string PropertyDateOfSupportExpiresOn = "SupportExpiresOn";
        public const string PropertyDateOfInstallation = "DateOfInstallation";
        public const string PropertyDateOfProcurement = "DateOfProcurement";
        public const string Quality = "Quality";

        #endregion Inspection Checklist

        #region Destruction Constants

        public const string MovementType_Approved = "551";
        public const string MovementType_Rejected = "555";

        #endregion Destruction Constants

        #region Equipment Types

        public const string Portable = "Portable";
        public const string Fixed = "Fixed";

        #endregion Equipment Types

        #region User Modes

        public const string QAMode = "Quality";
        public const string StoreMode = "Store";

        #endregion User Modes

        #region Stock Status

        public const string Available = "Available";
        public const string NotAvailable = "Not-Available";

        #endregion Stock Status
        #region statusColorCode
        public const string ExpiredColorCode = "Red";
        public const string RejectedColorCode = "Red";
        public const string ReleasedColorCode = "Green";
        public const string SampledColorCode = "Blue";
        public const string RetestColorCode = "Yellow";
        public const string UnderTestColorCode = "Yellow";
        #endregion
        #region DamgedStatus
        public const string Yes = "Yes";
        public const string No = "No";
        #endregion

    }
}