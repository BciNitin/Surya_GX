using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ELog.Application
{
    public interface IRMSupplierReturnAppService : IApplicationService
    {
        public Task<IActionResult> GetDocumentNo();
        public Task<IActionResult> GetItemCode(string sDocumentNo);
        public Task<IActionResult> GetItemLineNo(string sDocumentNo, string sItemCode);
        public Task<IActionResult> GetOrderDetails(string sDocumentNo, string sItemCode,string sItemLineNo);
        public Task<IActionResult> SaveSupplierReturnBarcode(string sOrderNo, string sPartCode,
            string sLineNo, string sLocationCode, string sScannedBarcode, string UserID,
            string sSiteCode, string sLineCode);
    }
}
