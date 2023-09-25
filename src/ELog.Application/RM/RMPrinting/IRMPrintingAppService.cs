using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;

namespace MobiVueEVO.Application
{
    public interface IRMPrintingAppService : IApplicationService
    {
        public Task<IActionResult> GetGRN();
        public Task<IActionResult> GetGRNLineNo(string GRNNo);
        public Task<IActionResult> GetGRNDetails(string sGRNNO, string GRNLineNo);
        public Task<IActionResult> GRNPrinting(string sGRNNo, string sDocumentType,
            string sRecevingTransaction, string sGRPODate, string sTransactionType,
            string sGRNLineNo, decimal sGRNQty, decimal sRemqty, string sSupplierCode, string sSupplierName,
            string sBatchNo, string sPoNo, string sItemNo, string sPOLineNo,
            string sPoScheduleNo, string sNumberofprint, int spacksize);
        //public IActionResult GRNPrintingDemo(GRNPrintingModel model);
        public Task<IActionResult> GetREMQTY(string sGRNNO, string sGRNLineNO);
    }
}