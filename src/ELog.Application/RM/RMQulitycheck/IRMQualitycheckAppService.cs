using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Threading.Tasks;

namespace ELog.Application
{
    public interface IRMQualityCheckAppService : IApplicationService
    {
        public Task<IActionResult> GetGRNNo();
        public Task<IActionResult> BindInventoryDetail(string sGRNNo);
        public Task<IActionResult> SCANBARCODEBindInventoryDetail(string sGRNNo,string ItemNO);
        public Task<IActionResult> SaveQualityCheckFullBatch(int status,
             string sGRNNo, string sItemNo, string sRemarks, bool isFullBatch);
    }
}
