using System.ComponentModel.DataAnnotations;

namespace ELog.Core
{
    public static class PMMSEnums
    {
        public enum ApprovalStatus
        {
            [Display(Name = "Submitted")]
            Submitted = 1,

            [Display(Name = "Approved")]
            Approved = 2,

            [Display(Name = "Rejected")]
            Rejected = 3,
        }

        public enum TransactionStatus
        {
            [Display(Name = "OnHold")]
            OnHold = 1,

            [Display(Name = "Accepted")]
            Accepted = 2,

            [Display(Name = "Rejected")]
            Rejected = 3,

            [Display(Name = "Saved")]
            Saved = 4,
        }

        public enum SubModuleType
        {
            [Display(Name = "Recommended")]
            Recommended = 1,

            [Display(Name = "Mandatory")]
            Mandatory = 2,

            [Display(Name = "Optional")]
            Optional = 3,
        }

        public enum UsersListSortBy
        {
            [Display(Name = "Username")]
            UserName = 1,

            [Display(Name = "Creation Date")]
            CreationDate = 2,

            [Display(Name = "Approval Status")]
            Approval_Status = 3,

            [Display(Name = "Status")]
            Status = 4
        }

        public enum Status
        {
            [Display(Name = "Active")]
            Active = 1,

            [Display(Name = "In-Active")]
            In_Active = 2
        }

        public enum RoleListSortBy
        {
            [Display(Name = "Role Name")]
            Name = 1,

            [Display(Name = "Display Name")]
            DisplayName = 2,

            [Display(Name = "Description")]
            Description = 3,

            [Display(Name = "Approval Status")]
            Approval_Status = 4,

            [Display(Name = "Status")]
            Status = 5
        }

        public enum PlantListSortBy
        {
            [Display(Name = "Plant Id")]
            PlantId = 1,

            [Display(Name = "Status")]
            Status = 2,

            [Display(Name = "License")]
            License = 3,

            [Display(Name = "Country")]
            CountryId = 4,
        }

        public enum GateListSortBy
        {
            [Display(Name = "Plant Name")]
            PlantName = 1,

            [Display(Name = "Sub Plant Id")]
            PlantId = 2,

            [Display(Name = "Gate Code")]
            GateCode = 3,

            [Display(Name = "Status")]
            Status = 4,
        }

        public enum LocationListSortBy
        {
            [Display(Name = "Sub Plant Id")]
            PlantId = 1,

            [Display(Name = "Location Code")]
            LocationCode = 2,

            [Display(Name = "Status")]
            Status = 3,

            [Display(Name = "Storage Location Type")]
            StorageLocationType = 4,

            [Display(Name = "Area Code")]
            Area = 5,
        }

        public enum PlantType
        {
            [Display(Name = "Master Plant")]
            MasterPlant = 1,

            [Display(Name = "Sub Plant")]
            SubPlant = 2
        }

        public enum CubicleListSortBy
        {
            [Display(Name = "Sub Plant  Id")]
            PlantId = 1,

            [Display(Name = "Cubicle Code")]
            CubicleCode = 2,

            [Display(Name = "Status")]
            Status = 3,

            [Display(Name = "Area Code")]
            Area = 4,

            [Display(Name = "Approval Status")]
            ApprovalStatus = 5,
        }

        public enum EquipmentListSortBy
        {
            [Display(Name = "Sub Plant Id")]
            PlantId = 1,

            [Display(Name = "Equipment Code")]
            EquipmentCode = 2,

            [Display(Name = "Equipment Type")]
            EquipmentType = 3,

            [Display(Name = "Status")]
            Status = 4,
        }

        public enum HandlingUnitListSortBy
        {
            [Display(Name = "Sub Plant Id")]
            PlantId = 1,

            [Display(Name = "Handling Unit Code")]
            HandlingUnitCode = 2,

            [Display(Name = "Handling Unit Type")]
            HandlingUnitType = 3,

            [Display(Name = "Status")]
            Status = 4,
        }

        public enum ModuleListSortBy
        {
            [Display(Name = "Module Name")]
            Name = 1,

            [Display(Name = "Display Name")]
            DisplayName = 2,

            [Display(Name = "Description")]
            Description = 3,

            [Display(Name = "Status")]
            Status = 4
        }

        public enum SubModuleListSortBy
        {
            [Display(Name = "Sub-Module Name")]
            Name = 1,

            [Display(Name = "Display Name")]
            DisplayName = 2,

            [Display(Name = "Status")]
            Status = 3,

            [Display(Name = "Sub-Module Type")]
            SubModuleType = 4,

            [Display(Name = "Module Name")]
            ModuleName = 5,

            [Display(Name = "Is Approval Required")]
            IsApprovalRequired = 6,
        }

        public enum StandardWeightBoxListSortBy
        {
            [Display(Name = "Sub Plant Id")]
            SubPlantId = 1,

            [Display(Name = "Standard Weight Box Id")]
            StandardWeightBoxId = 2,

            [Display(Name = "Department Code")]
            DepartmentId = 3,

            [Display(Name = "Area Code")]
            Area = 4,

            [Display(Name = "Status")]
            Status = 5,

            [Display(Name = "Approval Status")]
            ApprovalStatus = 6,
        }

        public enum StandardWeightListSortBy
        {
            [Display(Name = "Sub Plant Id")]
            SubPlantId = 1,

            [Display(Name = "Standard Weight Id")]
            StandardWeightBoxId = 2,

            [Display(Name = "Department Code")]
            DepartmentId = 3,

            [Display(Name = "Area Code")]
            Area = 4,

            [Display(Name = "Status")]
            Status = 5,

            [Display(Name = "Capacity")]
            Capacity = 6,

            [Display(Name = "Approval Status")]
            ApprovalStatus = 7,
        }

        public enum DepartmentListSortBy
        {
            [Display(Name = "Sub Plant Id")]
            SubPlantId = 1,

            [Display(Name = "Department Code")]
            DepartmentId = 2,

            [Display(Name = "Department Name")]
            Area = 3,

            [Display(Name = "Status")]
            Status = 4,

            [Display(Name = "Approval Status")]
            ApprovalStatus = 5,
        }

        public enum UnitOfMeasurementListSortBy
        {
            [Display(Name = "UOM Code")]
            UOMCode = 1,

            [Display(Name = "UOM Name")]
            UOMName = 2,

            [Display(Name = "UOM Type")]
            UOMType = 3,

            [Display(Name = "Status")]
            Status = 4,

            [Display(Name = "Approval Status")]
            ApprovalStatus = 5
        }

        public enum AreaListSortBy
        {
            [Display(Name = "Sub Plant Id")]
            SubPlantId = 1,

            [Display(Name = "Department Code")]
            DepartmentId = 2,

            [Display(Name = "Area Code")]
            AreaCode = 3,

            [Display(Name = "Area Name")]
            Area = 4,

            [Display(Name = "Status")]
            Status = 5,

            [Display(Name = "Approval Status")]
            ApprovalStatus = 6,
        }

        public enum InspectionChecklistListSortBy
        {
            [Display(Name = "Sub Plant Id")]
            SubPlantId = 1,

            [Display(Name = "Checklist Code")]
            DepartmentId = 2,

            [Display(Name = "Sub Module Name")]
            AreaCode = 3,

            [Display(Name = "Status")]
            Status = 4,

            [Display(Name = "Approval Status")]
            ApprovalStatus = 5,
        }

        public enum ApprovalRequireEnum
        {
            Yes = 1,
            No = 2
        }

        public enum WeighingMachineListSortBy
        {
            [Display(Name = "Weighing Machine Code")]
            WeighingMachineCode = 1,

            [Display(Name = "Sub Plant Id")]
            SubPlantId = 2,

            [Display(Name = "Unit Of Measurement")]
            UnitOfMeasurement = 3,

            [Display(Name = "Make")]
            Make = 4,

            [Display(Name = "Model")]
            Modal = 5,

            [Display(Name = "Status")]
            Status = 6,
        }

        public enum DeviceListSortBy
        {
            [Display(Name = "Sub Plant Id")]
            SubPlantId = 1,

            [Display(Name = "Device Id")]
            DeviceId = 2,

            [Display(Name = "Device Type")]
            DeviceType = 3,

            [Display(Name = "Make")]
            Make = 4,

            [Display(Name = "Model")]
            Model = 5,

            [Display(Name = "Status")]
            Status = 6
        }

        public enum TemperatureUnit
        {
            [Display(Name = "Celsius")]
            Celsius = 1,

            [Display(Name = "Fahrenheit")]
            Fahrenheit = 2,

            [Display(Name = "Kelvin")]
            Kelvin = 3
        }

        public enum WeighingMachineBalancedType
        {
            [Display(Name = "Analytical")]
            Analytical = 1,

            [Display(Name = "Normal")]
            Normal = 2
        }

        public enum WeighingMachineFrequencyType
        {
            [Display(Name = "Daily Verification")]
            Daily = 1,

            [Display(Name = "Weekly Calibration")]
            Weekly = 2,

            [Display(Name = "Monthly Calibration")]
            Monthly = 3
        }

        public enum ChecklistTypeSortBy
        {
            [Display(Name = "Sub Plant Id")]
            SubPlantId = 1,

            [Display(Name = "Checklist Type Code")]
            ChecklistTypeCode = 2,

            [Display(Name = "Checklist Name")]
            ChecklistName = 3,

            [Display(Name = "Sub-Module")]
            SubModuleName = 4,

            [Display(Name = "Status")]
            Status = 5,

            [Display(Name = "Approval Status")]
            ApprovalStatus = 6,
        }

        public enum VehicleInspectionListSortBy
        {
            [Display(Name = "Gate Pass No")]
            GatePassNo = 1,

            [Display(Name = "Purchase Order No")]
            PurchaseOrderNo = 2,

            [Display(Name = "Invoice No")]
            InvoiceNo = 3,

            [Display(Name = "LR No")]
            LRNo = 4,

            [Display(Name = "Status")]
            TransactionStatus = 5
        }

        public enum MaterialInspectionListSortBy
        {
            [Display(Name = "Gate Pass No")]
            GatePassNo = 1,

            [Display(Name = "Purchase Order No")]
            PurchaseOrderNo = 2,

            [Display(Name = "Invoice No")]
            InvoiceNo = 3,

            [Display(Name = "LR No")]
            LRNo = 4,

            [Display(Name = "Status")]
            UserEnteredTransaction = 5
        }

        public enum GateEntryListSortBy
        {
            [Display(Name = "Gate Pass No")]
            GatePassNumber = 1,

            [Display(Name = "Purchase Order No")]
            PurchaseOrderId = 2,

            [Display(Name = "Status")]
            Status = 3
        }

        public enum GrnPostingListSortBy
        {
            [Display(Name = "GRN No")]
            GRNNo = 1,

            [Display(Name = "Purchase Order No")]
            PurchaseOrderNo = 2
        }

        public enum WeightCaptureListSortBy
        {
            [Display(Name = "Purchase Order No")]
            PurchaseOrderNo = 1,

            [Display(Name = "Invoice No")]
            InvoiceNo = 2,

            [Display(Name = "Material Code")]
            MaterialItemCode = 3,

            [Display(Name = "Mfg Batch No")]
            ManufacturedBatchNo = 4
        }

        public enum CalenderListSortBy
        {
            [Display(Name = "Sub Plant Id")]
            SubPlantId = 1,

            [Display(Name = "Holiday Date")]
            HolidayDate = 2,

            [Display(Name = "Holiday Name")]
            HolidayName = 3,

            [Display(Name = "Holiday Type")]
            HolidayType = 4,

            [Display(Name = "Status")]
            Status = 5,

            [Display(Name = "Approval Status")]
            ApprovalStatus = 6,
        }

        public enum PalletizationListSortBy
        {
            [Display(Name = "Pallet Barcode")]
            PalletId = 1,

            [Display(Name = "Material Code")]
            MaterialCode = 2,

            [Display(Name = "SAP Batch No")]
            SAPBatchNo = 3,

            [Display(Name = "Count")]
            Count = 4,
        }

        public enum MaterialTransferType
        {
            [Display(Name = "PalletToBin")]
            PutAwayPalletToBin = 1,

            [Display(Name = "MaterialToBin")]
            PutAwayMaterialToBin = 2,

            [Display(Name = "PalletToBin")]
            BinToBinTranferPalletToBin = 3,

            [Display(Name = "MaterialToBin")]
            BinToBinTranferMaterialToBin = 4
        }

        public enum PutAwayListSortBy
        {
            [Display(Name = "Pallet Barcode")]
            PalletBarcode = 1,

            [Display(Name = "Location Barcode")]
            LocationBarcode = 2,

            [Display(Name = "Count")]
            Count = 3
        }

        public enum CalibrationStatus
        {
            [Display(Name = "Calibrated")]
            Calibrated = 1,

            [Display(Name = "Not Calibrated")]
            Not_Calibrated = 2,

            [Display(Name = "In Progress")]
            In_Progress = 3,
        }

        public enum CalibrationTestStatus
        {
            [Display(Name = "Passed")]
            Passed = 1,

            [Display(Name = "Failed")]
            Failed = 2,
        }

        public enum WeighingCalibrationStepType
        {
            WeighingCalibrationDetails = 1,
            EccentricityTest = 2,
            LinearityTest = 3,
            RepeatabilityTest = 4,
            UncertainityTest = 5
        }

        public enum WeighingCalibrationCapturedWeightKeyType
        {
            CapturedWeightValueCalibration = 1,
            CapturedWeightEccentricityCValue = 2,
            CapturedWeightEccentricityLFValue = 3,
            CapturedWeightEccentricityRFValue = 4,
            CapturedWeightEccentricityLBValue = 5,
            CapturedWeightEccentricityRBValue = 6,
            CapturedWeightLinearityWeight1Value = 7,
            CapturedWeightLinearityWeight2Value = 8,
            CapturedWeightLinearityWeight3Value = 9,
            CapturedWeightLinearityWeight4Value = 10,
            CapturedWeightLinearityWeight5Value = 11,
            CapturedWeightRepeatabilityWeight1Value = 12,
            CapturedWeightRepeatabilityWeight2Value = 13,
            CapturedWeightRepeatabilityWeight3Value = 14,
            CapturedWeightRepeatabilityWeight4Value = 15,
            CapturedWeightRepeatabilityWeight5Value = 16,
            CapturedWeightRepeatabilityWeight6Value = 17,
            CapturedWeightRepeatabilityWeight7Value = 18,
            CapturedWeightRepeatabilityWeight8Value = 19,
            CapturedWeightRepeatabilityWeight9Value = 20,
            CapturedWeightRepeatabilityWeight10Value = 21,
        }

        public enum WeighingCalibrationListSortBy
        {
            [Display(Name = "Calibration Date")]
            CalibrationTestDate = 1,

            [Display(Name = "Weighing Machine")]
            WeighingMachineCode = 2,

            [Display(Name = "Calibration Frequency")]
            UserEnteredCalibrationFrequency = 3,

            [Display(Name = "Calibration Status")]
            UserEnteredCalibrationStatus = 4
        }

        public enum CubicleAssignmentListSortBy
        {
            [Display(Name = "Group Code")]
            GroupCode = 1,

            [Display(Name = "Process Order No/Reservation No")]
            ProcessOrderNo = 2,

            [Display(Name = "Group Status")]
            Status = 3,
        }

        public enum SamplingCubicleAssignmentListSortBy
        {
            [Display(Name = "Group Code")]
            GroupCode = 1,

            [Display(Name = "Inspection Lot No")]
            ProcessOrderNo = 2,

            [Display(Name = "Group Status")]
            Status = 3,
        }

        public enum CubicleAssignementDetailStatus
        {
            [Display(Name = "Cancelled")]
            Cancelled = 1,

            [Display(Name = "Dispensed")]
            Dispensed = 2,

            [Display(Name = "InProgress")]
            InProgress = 3,
        }

        public enum CubicleAssignmentGroupStatus
        {
            [Display(Name = "Open")]
            Open = 1,

            [Display(Name = "Close")]
            Close = 2
        }

        public enum PickingHeaderStatus
        {
            [Display(Name = "InProgress")]
            InProgress = 1,

            [Display(Name = "Completed")]
            Completed = 2
        }

        public enum PreStageHeaderStatus
        {
            [Display(Name = "InProgress")]
            InProgress = 1,

            [Display(Name = "Completed")]
            Completed = 2,

            [Display(Name = "BatchInProgress")]
            BatchInProgress = 3,

            [Display(Name = "BatchCompleted")]
            BatchCompleted = 4
        }

        public enum PreStageBatchPickingStatus
        {
            [Display(Name = "InProgress")]
            InProgress = 1,

            [Display(Name = "Completed")]
            Completed = 2
        }

        public enum StageHeaderStatus
        {
            [Display(Name = "InProgress")]
            InProgress = 1,

            [Display(Name = "Completed")]
            Completed = 2,

            [Display(Name = "BatchInProgress")]
            BatchInProgress = 3,

            [Display(Name = "BatchCompleted")]
            BatchCompleted = 4
        }

        public enum CubicleCleaningHeaderStatus
        {
            [Display(Name = "Started")]
            Started = 1,

            [Display(Name = "Cleaned")]
            Cleaned = 2,

            [Display(Name = "Verified")]
            Verified = 3,

            [Display(Name = "Uncleaned")]
            Uncleaned = 4,

            [Display(Name = "Approved")]
            Approved = 5,
        }

        public enum EquipmentCleaningHeaderStatus
        {
            [Display(Name = "Started")]
            Started = 1,

            [Display(Name = "Cleaned")]
            Cleaned = 2,

            [Display(Name = "Verified")]
            Verified = 3,

            [Display(Name = "Uncleaned")]
            Uncleaned = 4,
        }

        public enum LineClearanceHeaderStatus
        {
            [Display(Name = "Started")]
            Started = 1,

            [Display(Name = "Verified")]
            Verified = 2,

            [Display(Name = "Approved")]
            Approved = 3,

            [Display(Name = "Rejected")]
            Rejected = 4,

            [Display(Name = "Cancelled")]
            Cancelled = 5,
        }

        public enum AreaUsageLogStatus
        {
            [Display(Name = "Started")]
            Started = 1,

            [Display(Name = "Verified")]
            Verified = 2,

            [Display(Name = "Approved")]
            Approved = 3,

            [Display(Name = "Rejected")]
            Rejected = 4,

            [Display(Name = "Cancelled")]
            Cancelled = 5,
        }


        public enum EquipmentUsageLogStatus
        {
            [Display(Name = "Started")]
            Started = 1,

            [Display(Name = "Verified")]
            Verified = 2,

            [Display(Name = "Approved")]
            Approved = 3,

            [Display(Name = "Rejected")]
            Rejected = 4,

            [Display(Name = "Cancelled")]
            Cancelled = 5,
        }

        public enum EquipementType
        {
            [Display(Name = "Fixed")]
            Fixed = 1,

            [Display(Name = "Portable")]
            Portable = 2
        }

        public enum MaterialBatchdispensingHeaderType
        {
            Picking = 1,
            PreStaging = 2,
            Staging = 3
        }

        public enum PrinterType
        {
            PRN = 1
        }

        public enum WeighingScaleType
        {
            Normal = 1
        }

        public enum ERPConnectorType
        {
            SAPAjanta = 1
        }

        public enum ScannerType
        {
            Barcode = 1
        }

        public enum DispensingHeaderStatus
        {
            [Display(Name = "InProgress")]
            InProgress = 1,

            [Display(Name = "Completed")]
            Completed = 2
        }

        public enum SamplingHeaderStatus
        {
            [Display(Name = "InProgress")]
            InProgress = 1,

            [Display(Name = "Completed")]
            Completed = 2
        }

        public enum UOMType
        {
            Pack = 1,
            Weight = 2,
        }

        public enum StageOutStatus
        {
            [Display(Name = "InProgress")]
            InProgress = 1,

            [Display(Name = "Completed")]
            Completed = 2
        }

        public enum ReturnToVendorStatus
        {
            [Display(Name = "NotPosted")]
            NotPosted = 1,

            [Display(Name = "Posted")]
            Posted = 2
        }

        public enum ReportType
        {
            [Display(Name = "VehicleInspection")]
            VehicleInspection = 1,

            [Display(Name = "MaterialInspection")]
            MaterialInspection = 2,

            [Display(Name = "Allocation")]
            Allocation = 3,

            [Display(Name = "CubicleAssignment")]
            CubicleAssignment = 4,

            [Display(Name = "Picking")]
            Picking = 5,
            [Display(Name = "LineClearance")]
            LineClearance = 6,

            [Display(Name = "Dispensing")]
            Dispensing = 7,
            [Display(Name = "CubicleCleaning")]
            CubicleCleaning = 8,
            [Display(Name = "EquipmentCleaning")]
            EquipmentCleaning = 9,
            [Display(Name = "WeighingCalibration")]
            WeighingCalibration = 10,
            [Display(Name = "Disptach")]
            Disptach = 11,

        }
        public enum StockStatus
        {
            [Display(Name = "Available")]
            Available = 1,

            [Display(Name = "Not-Available")]
            NotAvailable = 2
        }

        public enum PasswordListSortBy
        {
            [Display(Name = "Employee Code")]
            UserName = 1,

            [Display(Name = "First Name")]
            FirstName = 2,

            [Display(Name = "Last Name")]
            LastName = 3,


        }

        public enum ResetPasswordStatus
        {
            [Display(Name = "Reset PW Pending")] /* request to reset from user*/
            ResetPending = 1,

            [Display(Name = "Reset PW Submitted")] /* reset done by admin*/
            Submitted = 2,

            [Display(Name = "Reset PW Completed")] /* reset done by user  themseleves after requesting to admin*/
            SelfResetAfterRequest = 3,

            [Display(Name = "SelfReset")] /* reset done by user without requesting to admin*/
            SelfReset = 4,
        }

        public enum RoleCategories
        {
            [Display(Name = "SuperAdmin")]
            SuperAdmin = 1,

            [Display(Name = "Admin")]
            Admin = 2,

            [Display(Name = "SuperAdminAndAdmin")]
            SuperAdminAndAdmin = 3,
        }
        public enum CubicleStatus
        {
            [Display(Name = "Not In Use")]
            NotInUse = 1,

            [Display(Name = "Uncleaned")]
            Uncleaned = 2,

            [Display(Name = "Cleaned")]
            Cleaned = 3,

            [Display(Name = "In Process")]
            InProcess = 4,
        }
        public enum EquipmentStatus
        {
            [Display(Name = "Not In Use")]
            NotInUse = 1,

            [Display(Name = "Uncleaned")]
            Uncleaned = 2,

            [Display(Name = "Cleaned")]
            Cleaned = 3,

            [Display(Name = "In Process")]
            InProcess = 4,
        }
        public enum MaterialStatusLabel
        {
            [Display(Name = "Under Test")]
            UnderTest = 1,

            [Display(Name = "Retest.")]
            Retest = 2,

            [Display(Name = "Sampled")]
            Sampled = 3,

            [Display(Name = "Released")]
            Released = 4,
            [Display(Name = "Rejected")]
            Rejected = 5,
            [Display(Name = "Expired")]
            Expired = 6,
        }
    }
}