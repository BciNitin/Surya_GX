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
        public const string ShiftMaster = "GETShiftMaster";
        public const string CustomerMaster = "CustomerMaster";
        public const string GetPlantCode = "GetPlantCode";
        public const string GetBinCode = "GetBinCode";
        public const string GetBinById = "GetBinById";
        public const string sp_masters_bin = "sp_masters_bin";
        public const string SP_Master = "sp_masters";
        public const string SP_SelectList = "sp_get_selectList";
        public const string SP_GenerateSerialNumber = "sp_Generate_BarCode";
        public const string SP_PackingOrderConfirmation = "sp_PackingOrderConfirmation";
        public const string Type = "@sType";
        public const string sType_GenerateBarCode = "GenerateBarCode";
        public const string GetLineCode = "GetLineCode";
        public const string GetPackingOrder = "GetPackingOrderNo";
        public const string GetSerialNumberDetails = "GetSerialNumberDetails";
        public const string GetPackingOrderConfirmation = "GetPackingOrderConfirmation";

    }
}
