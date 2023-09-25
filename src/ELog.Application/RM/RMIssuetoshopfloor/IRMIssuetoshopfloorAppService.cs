using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;

namespace ELog.Application
{
    public interface IRMIssueToShopFloorAppService : IApplicationService
    {
        public Task<IActionResult> GetRMWORKORDERNO();
        public Task<IActionResult> FillFGITEMCODE(string sWORKORDERNO);
        public Task<IActionResult> ValidateLocation(string _WORKORDERNO, string _ScanLocation);
        public Task<IActionResult> ScanIssueBarcode(string _WORKORDERNO, string _FGItemCode, string _ScanLocation, string sPartBarcode);
    }
}
