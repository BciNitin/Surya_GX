namespace ELog.Core
{
    public static class PMMSPermissionConst
    {
        #region Masters

        public const string User_SubModule = "User";
        public const string Role_SubModule = "Role";
        public const string Password_SubModule = "Password";
        public const string Plant_SubModule = "Plant";
        public const string Location_SubModule = "Location";
        public const string Gate_SubModule = "Gate";
        public const string ChecklistType_SubModule = "ChecklistType";
        public const string Department_SubModule = "Department";
        public const string Area_SubModule = "Area";
        public const string StandardWeight_SubModule = "StandardWeight";
        public const string StandardWeightBox_SubModule = "StandardWeightBox";
        public const string InspectionChecklist_SubModule = "InspectionChecklist";
        public const string Cubicle_SubModule = "Cubicle";
        public const string Equipment_SubModule = "Equipment";
        public const string HandlingUnit_SubModule = "HandlingUnit";
        public const string WeighingMachine_SubModule = "WeighingMachine";
        public const string UnitOfMeasurement_SubModule = "UnitOfMeasurement";
        public const string Device_SubModule = "Device";
        public const string Activity_SubModule = "ActivityMaster";
        public const string PrnEntry_SubModule = "PRNFileMaster";


        #endregion Masters

        #region Permissions

        public const string View = "View";
        public const string Add = "Add";
        public const string Edit = "Edit";
        public const string Delete = "Delete";
        public const string Print = "Print";
        public const string Approver = "Approver";

        #endregion Permissions

        #region Inward

        public const string GateEntry_SubModule = "GateEntry";
        public const string VehicleInspection_SubModule = "VehicleInspection";
        public const string WeightCapture_SubModule = "WeightCapture";
        public const string MaterialInspection_SubModule = "MaterialInspection";
        public const string GRNPosting_SubModule = "GRNPosting";
        public const string Calendar_SubModule = "Calendar";
        public const string GRNMaterialLabelPrinting_SubModule = "MaterialLabelPrinting";
        public const string Palletization_SubModule = "Palletization";
        public const string PutAway_SubModule = "PutAway";
        public const string BinToBinTransfer_SubModule = "BintoBinTransfer";
        public const string WeighingCalibration_SubModule = "WeighingCalibration";
        public const string MaterialStatusLabel_Submodule = "MaterialStatusLabel";
        #endregion Inward


        #region Dispensing

        public const string CubicleAssignment_SubModule = "CubicleAssignment";
        public const string CubicleCleaning_SubModule = "CubicleCleaning";
        public const string SamplingCubicleCleaning_SubModule = "CubicleCleaning(Sampling)";
        public const string EquipmentCleaning_SubModule = "EquipmentCleaning";
        public const string SamplingEquipmentCleaning_SubModule = "EquipmentCleaning(Sampling)";
        public const string SamplingCubicleAssignment_SubModule = "CubicleAssignment(Sampling)";
        public const string LineClearance_SubModule = "LineClearance";
        public const string SamplingLineClearance_SubModule = "LineClearance(Sampling)";
        public const string EquipmentAssignment_SubModule = "EquipmentAssignment";
        public const string SamplingEquipmentAssignment_SubModule = "EquipmentAssignment(Sampling)";
        public const string Picking_SubModule = "Picking";
        public const string SamplingPicking_SubModule = "Picking(Sampling)";
        public const string Prestage_SubModule = "Prestage";
        public const string SamplingPrestage_SubModule = "Prestage(Sampling)";
        public const string Staging_SubModule = "Staging";
        public const string SamplingStaging_SubModule = "Staging(Sampling)";
        public const string Dispensing_SubModule = "Dispensing";
        public const string Sampling_SubModule = "Sampling";
        public const string ReturnToVendor_SubModule = "ReturntoVendor";
        public const string SampleDestruction_SubModule = "Destruction(Sampling)";
        public const string IssueToProduction_SubModule = "IssueToProduction";

        #endregion Dispensing

        #region Report
        public const string VehicleInspectionReport_SubModule = "VehicleInspectionReport";
        public const string MaterialInspectionReport_SubModule = "MaterialInspectionReport";
        public const string AllocationReport_SubModule = "AllocationReport";
        public const string CubicleAssignmentReport_SubModule = "CubicleAssignmentReport";
        public const string PickingReport_SubModule = "PickingReport";
        public const string LineClearanceReport_SubModule = "LineClearanceReport";
        public const string DispensingReport_SubModule = "DispensingReport";
        public const string CubicleCleaningReport_SubModule = "CubicleCleaningReport";
        public const string EquipmentCleaningReport_SubModule = "EquipmentCleaningReport";
        public const string WeighingCalibrationReport_SubModule = "WeighingCalibrationReport";
        public const string DispatchReport_SubModule = "DispatchReport";
        #endregion Report

        #region ModuleSubmodule

        public const string Module = "Module";
        public const string SubModule = "SubModule";

        #endregion ModuleSubmodule

        #region Recipe

        public const string RecipeApproval = "RecipeApproval";

        #endregion Recipe
    }
}