using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ELog.Application
{
    public interface IRMJobWorkAppService : IApplicationService
    {
        public Task<IActionResult> GetIssueSlipNo();
        public Task<IActionResult> GetItemCode(string issueSlipNo);
        public Task<IActionResult> GetItemLineNo(string issueSlipNo, string itemNumber);
        public Task<IActionResult> GetGridDetail(string issueSlipNo, string itemNo, string itemLineNo
           );
        public Task<IActionResult> SaveJobWork(string issueSlipNo, string itemNo, string itemLineNo,
            string barCode, string location);
    }
}
