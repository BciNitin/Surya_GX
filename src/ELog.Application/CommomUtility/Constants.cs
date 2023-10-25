using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELog.Application.CommomUtility
{
    public static class Constants
    {
        public const string Schema = "surya_roshni_01.";
        public const string PlantMaster = "GETPLANT";
        public const string MaterialMaster = "GETMATERIAL";
        public const string LineMaster = "GETLINE";
        public const string StorageLocationMaster = "GETSTORAGELOCATTION";
        public const string BinMaster = "GETBIN";
        public const string ShiftMaster = "GetShiftMaster";
        public const string CustomerMaster = "CustomerMaster";
        public const string GetPlantCode = "GetPlantCode";
        public const string GetBinCode = "GetBinCode";
        public const string GetBinById = "GetBinById";
        public const string GetPackingMasters = "PackingMaster";
        public const string sp_masters_bin = "sp_masters_bin";
        public const string SP_Master = "sp_masters";
        public const string SP_SelectList = "sp_get_selectList";
        public const string SP_GenerateSerialNumber = "sp_Generate_BarCode";
        public const string SP_PackingOrderConfirmation = "sp_PackingOrderConfirmation";
        public const string sp_QualitySampling = "sp_QualitySampling";
        public const string Type = "sType";
        public const string sp_ManualPacking = "sp_ManualPacking";
        //public const string GetManualPackingDetails = "GetManualPackingDetails";
        public const string sType_GenerateBarCode = "GenerateBarCode";
        public const string GetLineCode = "GetLineCode";
        public const string GetStorageLocCode = "GetStorageLocCode";
        public const string GetPackingOrder = "GetPackingOrderNo";
        public const string GetSerialNumberDetails = "GetSerialNumberDetails";
        public const string GetPackingOrderConfirmation = "GetPackingOrderConfirmation";
        public const string PackingOrderConfirmation = "ConfirmPackingOrder";
        public const string GetQualitySamplingQty = "GetQualitySamplingQty";
        public const string SaveQualitySampling = "SaveQualitySampling";
        public const string SP_StorageLocation = "sp_StorageLocation";
        public const string GetStorageLocationDetails = "GetStorageLocDtls";
        public const string GetbarcodeDetails = "GetBarcodeScannedDtls";
        public const string StorageLocationTransfer = "StorageLocationTransfer";
        public const string sp_Manual_Packing = "sp_Manual_Packing";
        public const string GetManualPackingDetails = "GetPackingOrderDetails";
        public const string validateBarcode = "ProcessData";
        public const string sp_QualityTested_ItemPlacement = "sp_QualityTested_ItemPlacement";
        public const string GetQualityItemTestedDtls = "GetQualityItemTestedDtls";
        public const string ValidateShiperBarcode = "ValidateShiperBarcode";
        public const string GetChallanNo = "GetChallanNo";
        public const string sp_Transfer_To_BranchFrom_Plant = "sp_Transfer_To_BranchFrom_Plant";
        public const string GetValidateScanCartonBarcode = "GetValidateScanCartonBarcode";
        public const string GetChallanDetails = "GetChallanDetails";
        public const string sp_TransferToDealer = "sp_TransferToDealerCustFromBranchLocation";
        public const string GetSOchallanNo = "GetSOchallanNo";
        public const string GetSOChallanDetails = "GetSOChallanDetails";
        public const string GetValidateSOScanCartonBarcode = "GetValidateSOScanCartonBarcode";
        public const string sp_Revalidation_Process_Branch = "sp_Revalidation_Process_BranchPlant";
        public const string GetExpiredItemCode = "GetExpiredItemCode";
        public const string GetExpiredItemDetails = "GetExpiredItemCodeDetails"; 
        public const string GetValidateItem = "GetValidateItem";
        public const string GetValidateGRNConfirm = "GetValidateGRNConfirmation";
        public const string sp_GRN_Confirm = "sp_GRN_Confirmation";
    }
}
