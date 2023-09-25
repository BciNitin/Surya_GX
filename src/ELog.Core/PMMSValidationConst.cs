namespace ELog.Core
{
    public static class PMMSValidationConst
    {
        public const int ValidationCode = 422;
        public const string PlantDelete = "Plant cannot be deleted. One or more entities are associated with this plant.";
        public const string LocationDelete = "Location cannot be deleted. One or more entities are associated with this location.";
        public const string DepartmentDelete = "Department cannot be deleted. One or more entities are associated with this department.";
        public const string AreaDelete = "Area cannot be deleted. One or more entities are associated with this area.";
        public const string PlantAlreadyExist = "Plant id already exist.";
        public const string PlantCannotDeactivated = "Plant cannot be de-activated. One or more entities are associated with this plant.";
        public const string DepartmentCannotDeactivated = "Department cannot be de-activated. One or more entities are associated with this department.";
        public const string AreaCannotDeactivated = "Area cannot be de-activated. One or more entities are associated with this area.";
        public const string LocationCannotDeactivated = "Location cannot be de-activated. One or more entities are associated with this location.";
        public const string GateCodeAlreadyExist = "Gate code already exist.";
        public const string StandardWeightBoxIdAlreadyExist = "Standard weight box id already exist.";
        public const string InspectionCheckListAlreadyExist = "Inspection checkList code already exist.";
        public const string StandardWeightIdAlreadyExist = "Standard weight id already exist.";
        public const string CubicleCodeAlreadyExist = "Cubicle code already exist.";
        public const string EquipmentCodeAlreadyExist = "Equipment code already exist.";
        public const string LocationCodeAlreadyExist = "Location code already exist.";
        public const string HUCodeAlreadyExist = "Handling unit code already exist.";
        public const string DeviceIdAlreadyExist = "Device id already exist";
        public const string CubicleCannotDeactivated = "Cubicle cannot be de-activated. One or more entities are associated with this cubicle.";
        public const string CubicleDelete = "Cubicle cannot be deleted. One or more entities are associated with this cubicle.";
        public const string UOMCannotDeactivated = "UOM cannot be de-activated. One or more entities are associated with this UOM.";
        public const string UOMDelete = "UOM cannot be deleted. One or more entities are associated with this UOM.";
        public const string GatePassNoAlreadyExist = "Gate pass is already done for this {0}.";
        public const string GatePassNoCanNotNull = "Gate pass number can not be null.";
        public const string UOMAlreadyExist = "UOM already exist.";
        public const string PlantTypeCannotChanged = "Plant type for this plant cannot be changed.  One or more entities are associated with this plant.";
        public const string SAPPlantNotFound = "Plant Id not found.";
        public const string SAPPOCreated = "Purchase order created successfully.";
        public const string SAPPROCreated = "Process order created successfully.";
        public const string MaterialNotFound = "Material not found ,Please update data in MaterialMaster table";

        public const string WeighingMachineAlreadyExist = "Weighing machine code already exist for selected plant.";

        public const string VehicleInspectionAlreadyExist = "Vehicle inspection is already done for this {0}.";

        public const string MaterialVerificationAlreadyExist = "Material verification already done for this material code {0}.";

        public const string PostToSAPProductCodeAlreadyExist = "This process order no already posted.";

        public const string DispContainerCodeAlreadyExist = "This container code already exists.";

        public const string ValueTagRequired = "Value tag is reuired.";
        public const string AcceptanceValueRequired = "Acceptanace value is reuired.";
        public const string InvalidPipeSepratedString = "Please enter valid value tag. Use | as saperator while entering values. e.g (Value1 | Value2 | Value3).";
        public const string UniqueValuesPipeSepratedString = "Please enter unique values";
        public const string InvalidAccpetanceValueFromValueTag = "Please enter valid acceptance value from value tag.";

        public const string CannotCompleteMaterialInspection = "Material inspection cannot be completed.One or more materials are pending to be Saved.";
        public const string MaterialInspectionCompleted = "Material inspection is completed.No changes allowed.";
        public const string MaterialInspectionAlreadyExist = "Material inspection is already done for this {0}.";
        public const string GRNNotFoundByGRNNo = "Entered GRN no not Found.";
        public const string LabelsAlreadyPrinted = "Labels are already printed for this material.";
        public const string PageRangeNotValid = "Entered page range is not valid.";
        public const string InspectionCheckListDelete = "Inspection checklist cannot be deleted. One or more entities are associated with this inspection checklist.";
        public const string CheckListTypeDelete = "Checklist type cannot be deleted. One or more entities are associated with this checklist type.";
        public const string WeighingMachineDelete = "Weighing machine cannot be deleted. One or more entities are associated with this weighing machine.";
        public const string WeighingMachineRejected = "Weighing machine cannot be rejected. One or more entities are associated with this weighing machine.";
        public const string CheckListTypeInactive = "Checklist type cannot be de-activated. One or more entities are associated with this checklist type.";
        public const string PartitionNotValid = "Only 6 combination Of material code and SAP batch no are allowed";
        public const string DuplicateMaterialBarcode = "Material Barcode is already present";
        public const string PlantRejected = "Plant cannot be rejected. One or more entities are associated with this plant.";
        public const string LocationRejected = "Location cannot be rejected. One or more entities are associated with this location.";
        public const string CubicleRejected = "Cubicle cannot be rejected. One or more entities are associated with this cubicle.";
        public const string DepartmentRejected = "Department cannot be rejected. One or more entities are associated with this department.";
        public const string UOMRejected = "UOM cannot be rejected. One or more entities are associated with this UOM.";
        public const string AreaRejected = "Area cannot be rejected. One or more entities are associated with this area.";
        public const string InspectionCheckListRejected = "Inspection checklist cannot be rejected. One or more entities are associated with this inspection checklist.";
        public const string CheckListTypeRejected = "Checklist type cannot be rejected. One or more entities are associated with this checklist type.";
        public const string StdWeightBoxInactive = "Standard weight Box cannot be de-activated. One or more entities are associated with this Standard Weight Box.";
        public const string StdWeightBoxDelete = "Standard weight Box cannot be deleted. One or more entities are associated with this Standard Weight Box.";
        public const string StdWeightBoxReject = "Standard weight Box cannot be rejected. One or more entities are associated with this Standard Weight Box.";
        public const string StdWeightInactive = "Standard weight cannot be de-activated. One or more entities are associated with this Standard Weight.";
        public const string StdWeightDelete = "Standard weight cannot be deleted. One or more entities are associated with this Standard Weight.";
        public const string StdWeightReject = "Standard weight cannot be rejected. One or more entities are associated with this Standard Weight.";


        public const string PalletMappedMaterialNotAllowed = "Material mapped with pallet is not allowed for scanning";
        public const string MaterialIsNotMappedWithBin = "Material is not mapped with bin for bin To bin Transfer";
        public const string PalletIsNotMappedWithBin = "Pallet is not mapped with bin for bin To bin Transfer";
        public const string PalletIsAlreadyMappedWithBin = "Pallet is already mapped to another Bin";
        public const string CleaningInProgresss = "Cubicle cleaning cannot be started. Cubicle cleaning of another type is already in progress.";
        public const string MaterialIsAlreadyMappedWithBin = "Material is already mapped with bin";
        public const string EquipmentCleaningInProgresss = "Equipment cleaning cannot be started. Equipment cleaning of another type is already in progress.";

        public const string SAPPOMaterialReceived = "Material for purchase order received successfully.";
        public const string SAPPOMaterialUpdated = "Material for purchase order received updated successfully.";
        public const string SAPQualityControlDetailSaved = "Quality control detail saved successfully.";
        public const string SAPQualityControlDetailUpdated = "Quality control detail updated successfully.";
        public const string SAPGRNPostingDetailSaved = "GRN posting detail saved successfully.";
        public const string LineClearanceInProgresss = "Line clearance cannot be started. Line clearance of another group is already in progress.";
        public const string CubicleClosedGroupValidation = "All assigned groups under this cubicle has closed status.";
        public const string CubicleNoGroupAssignedValidation = "No active group found for this cubicle.";
        public const string CubicleNotFoundValidation = "Cubicle does not found.";
        public const string BinIsNotFromSuggestedBinValidation = "Bin is not from suggested bins.";
        public const string InActiveAndNotApprovedBinValidation = "Bin does not found.";
        public const string CubicleUncleaned = "Line clearance cannot be started on unclean cubicle";
        public const string LineClearanceEuipementNotClean = "Scan cubicle has some unclean equipment, Please clean the equipment to start line clearance.";
        public const string CubicleAssignGroupLineClearanceNotDone = "Line clearance is not completed for assigned group under this cubicle.";

        public const string NoMaterialAvailableUnderGroup = "There are no material available under this group.";
        public const string NoSAPAvailableUnderGroup = "There are no SAP batch number available under this material.";
        public const string ContainerNotFound = "Material container not found or consumed completely.";
        public const string ContainerNotFoundForBin = "Material container not found under selected bin.";
        public const string ContainerAlreadyScanned = "Material container barcode is already scanned.";
        public const string ContainerUnderpickingNotAllowed = "Material container underpicking is not allowed.";
        public const string CubicleAssignGroupPickingNotDone = "Picking is not completed for material under this cubicle.";
        public const string GateEntryRejected = "Gate entry cannot be rejected. One or more entities are associated with this gate Entry.";
        public const string PreStageCompletionNotAllowed = "Pre stage cannot be completed as not all picked material is placed in the pre stage area.";
        public const string NoSapBatchNoAvailableUnderMaterial = "There are no sap batch no available under this material.";
        public const string CubicleAssignGroupPreStageNotDone = "Pre Stage is not completed for assigned group under this cubicle.";
        public const string StagingCompletionNotAllowed = "Staging cannot be completed as not all required materials are placed in dispensing area.";

        public const string EquipmentNotFoundValidation = "Equipment does not found.";
        public const string NoProcessorderAvailableUnderEquipment = "There are no po available under this RLAF Barcode.";
        public const string NoMaterialAvailableUnderPO = "There are no material available under this PO.";
        public const string NoStagingCompletedPo = "Staging is not completed for purchase order under this equipment.";
        public const string BalanceIsNotFromSuggestedBalanceValidation = "Please scan suggested balance only";
        public const string BalanceIdNotFound = "Balance does not found.";
        public const string NotStagedContainer = "Staging is not completed for scan material container Barcode.";
        public const string WMNotCalibrated = "No weighing machine is calibrated for selected UOM.";

        public const string AllSAPBatchDispensingNotCompleted = "Dispensing for all SAP Batches is not completed.";

        public const string NoStagingCompletedInspectionLot = "Staging is not completed for inspection lot number under this equipment.";
        public const string NoInspectionLotNoAvailableUnderEquipment = "There are no inspection lot available under this RLAF Barcode.";
        public const string NoMaterialAvailableUnderInspectionLotNo = "There are no material available under this inspection lot.";
        public const string NoInspectionLotAvailableUnderEquipment = "There are no inspection lot no available under this RLAF Barcode.";

        public const string NetWeightGreaterThanBalanceQuantity = "Net weight is greater than balance quantity.";
        public const string NoOfPackGreaterThanBalanceQuantity = "Number of pack is greater than balance quantity.";
        public const string NetWeightGreaterThanContainerBalanceQuantity = "Net weight is greater than container balance quantity.";
        public const string NoOfPackGreaterThanContainerBalanceQuantity = "Number of Pack is greater than container balance quantity.";

        public const string EquipmentNotFound = "Equipment not found.";
        public const string EquipmentNotCleaned = "Equipment is not cleaned.";
        public const string OneOrMoreEquipmentAlreadyAssigned = "One or more equipment is already assigned under this cubicle.";
        public const string EquipmentAlreadyAssigned = "Equipment is already assigned under this cubicle.";

        public const string EquipmentCannotDeassigned = "One or more equipment cannot be deassigned as dispensing is in progress.";
        public const string SampleEquipmentCannotDeassigned = "One or more equipment cannot be deassigned as sampling is in progress.";
        public const string InvalidValueMsg = "Please enter valid data.";
        public const string MaterialDocumentNotFoundValidation = "Material Document does not found.";
        public const string QuantityNotmatched = "Scan Quantity should match with total quantity.";
        public const string NoMaterialRejected = "No material is rejected for selected material document no.";
        public const string scanQtyGreaterThanBalanceQuantity = "Scan Material code quantity is greater than rejected quantity.";
        public const string NotSamplingContainer = "Sampling is not completed for scan material container Barcode.";
        public const string NoGroupFoundUnderCubicle = "There are no group available under selected cubicle.";
        public const string NoInspectionLotFoundUnderCubicle = "There are no inspection lot available under selected cubicle.";
        public const string NoLooseContainerFound = "There are no loose container available for this material.";
        public const string NoLooseContainerFoundByBarcode = "There are no loose container available for this material container barcode.";
        public const string StageOutCannotCompleted = "One or more loose container is pending.Stage Out cannot be completed.";
        public const string NoLooseContainerForCompletion = "No loose container found for Stage Out completion.";
        public const string PalletNotThere = "Pallet code not exist.";

        public const string MovementTypeNotValid = "Movement type is not valid.";
        public const string ContainerBarcodeNotValid = "Scanned material container barcode is not valid.";
        public const string ContainerNotFoundMovementType = "Material container not found for selected movement type.";
        public const string NoContainerFoundForSAPPosting = "No material container exist for SAP Posting.";
        public const string UserCreationLimitExceeded = "Only {0} active users are allowed.";
        public const string CurrentUserIsNotSuperAdmin = "Only super admin user can add or update user creation max value.";
        public const string NotValidUser = "Only store and quality user can access the reports.";

        public const string NoInspectionLotNoAvailable = "Their are no inspection lot available for destruction.";
        public const string LogoFileExtensionValidationMsg = "File extension is invalid - only png image formats is accepted";
        public const string LogoSuccessMsg = "Image uploaded successfully";
        public const string ContainerReturnedToVendor = "Material has already been returned.";
        public const string GRNNotCreatedForQCMaterial = "GRN of material is not created.";
        public const string GRNPostingFailed = "Error occured while posting GRN.";
        public const string MaterilNotExist = "Material does not exist in material master.";
        public const string MaterialLabelContainerNotFound = "Material container not found.";
        public const string MaterialBarcoodeNotExist = "Material container not exists for current pack";


        public const string ProcessOrderAlreadyAssigned = "Process order is already assigned with same cubicle and equipment";
        public const string ProcessOrderAssignedWithFixedEquipment = "Selected process order assigned with same fixed equipment cannot be assigned to different cubicle ";
        public const string CubicleNotCleaned = "Cubicle is not Cleaned";
        public const string EquipmentOrCubicleNotAssignedToProcessOrder = "Equipment or Cubicle is not assigned with the selected process order.";
        public const string LineClearenceAlreadyDone = "Line clearance is already done for the selected process order.";
        public const string ActivityAlreadyExist = "Activity code already exist.";
        public const string PrnNameAlreadyExist = "PRN name already exist.";
        public const string LavelAlreadyExist = "Level name already exist.";
        public const string AreaUsageLogAlreadyDone = "Area Usage Log is already done for the selected Activity.";
    }
}