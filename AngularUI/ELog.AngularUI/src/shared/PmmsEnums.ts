export enum TransactionStatus {
    OnHold = 1,
    Accepted = 2,
    Rejected = 3,
    Saved = 4
}
export enum CubicleCleaningStatus{
    Started=7,
    Cleaned=8,
    Verified=9,
    Uncleaned=10
}
export enum CheckPointTypeMaster {
    Condition = 1,
    Options = 2,
    Text = 3,
}

export enum PutAwayAndBinToBinTypeId {
    PutAwayPalletToBin = 1,
    PutAwayMaterialToBin = 2,
    BinToBinTranferPalletToBin = 3,
    BinToBinTranferMaterialToBin = 4
}
export const PutAwayAndBinToBinTypes = [
    {
        'id': PutAwayAndBinToBinTypeId.PutAwayPalletToBin,
        'displayName': "Pallet To Bin"
    },
    {
        'id': PutAwayAndBinToBinTypeId.PutAwayMaterialToBin,
        'displayName': "Material To Bin"
    },
    {
        'id': PutAwayAndBinToBinTypeId.BinToBinTranferPalletToBin,
        'displayName': "Pallet To Bin"
    },
    {
        'id': PutAwayAndBinToBinTypeId.BinToBinTranferMaterialToBin,
        'displayName': "Material To Bin"
    }
];
export enum EquipmentCleaningTypeId {
    Fixed = 1,
    Portable = 2
}
export const EquipmentCleaningTypes = [
    {
        'id': EquipmentCleaningTypeId.Fixed,
        'displayName': "Fixed"
    },
    {
        'id': EquipmentCleaningTypeId.Portable ,
        'displayName': "Portable"
    }
];
export enum WeighingCalibrationSaveType {
    WeighingCalibrationDetails = 1,
    EccentricityTest = 2,
    LinearityTest = 3,
    RepeatabilityTest = 4,
    UncertainityTest = 5
}
export enum CalibrationStatus {
    Calibrated = 1,
    Not_Calibrated = 2,
    In_Progress
}
export enum CalibrationTestStatus {
    Passed = 1,
    Failed = 2
}
export enum WeighingMachineFrequencyType{
Daily = 1,
Weekly = 2,
Monthly = 3
}
export enum LineClearanceSubModule  
    {
        SamplingLineClearance_SubModule= "LineClearance(Sampling)",
        DispensingLineClearance_SubModule="LineClearance",
}
export enum EquipmentCleaningSubModule {
    SamplingEquipmentCleaning_SubModule = "EquipmentCleaning(Sampling)",
    DispensingEquipmentCleaning_SubModule = "EquipmentCleaning",
}
export enum CubicleCleaningSubModule {
    SamplingCubicleCleaning_SubModule = "CubicleCleaning(Sampling)",
    DispensingCubicleCleaning_SubModule = "CubicleCleaning",
}
export enum UomTypeEnum {
    Weight = "Weight",
    Pack = "Pack",
}
export enum ReportSubModule {
    VehicleInspectionReport_SubModule = "VehicleInspectionReport",
    MaterialInspectionReport_SubModule = "MaterialInspectionReport",
    AllocationReport_SubModule = "AllocationReport",
    CubicleAssignmentReport_SubModule = "CubicleAssignmentReport",
    PickingReport_SubModule = "PickingReport",
    LineClearanceReport_SubModule = "LineClearanceReport",
    DispensingReport_SubModule = "DispensingReport",
    CubicleCleaningReport_SubModule = "CubicleCleaningReport",
    EquipmentCleaningReport_SubModule = "EquipmentCleaningReport",
    DispensingTrackingReport_SubModule = "DispensingTrackingReport",
    DispatchReport_SubModule = "DispatchReport",
}
export enum ModeMaster{
    Production = 1,
    Quality = 2,
    Store = 3
    }