using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ELog.Application
{
    public interface IRMSFGReturnToRMWarehouseAppService : IApplicationService
    {
        public Task<IActionResult> GetMaterialDocument();
        //public Task<IActionResult> GetRMPrintBarcodeFromWIPIssue(string _MRNNo, string _Part_Barcode, string sScanBy,
        //    decimal dReturnQty,
        //    string sSiteCode, string _PartCode, string sWorkOrderNo, string sScanType, string sUserID, string sLineCode
        //    , string sRemarks);
        //public Task<IActionResult> GetRMBarcodeFromWIPIssue(string Type, string WorkOrder, string BarCode, string SiteCode);
        //public Task<IActionResult> GetRMPrintBarcodeFromWIPIssue(string _MRNNo, string _Part_Barcode, string sScanBy,
        //    decimal dReturnQty,
        //    string sSiteCode, string _PartCode, string sWorkOrderNo, string sScanType, string sUserID, string sLineCode
        //    , string sRemarks);
    }
}
