using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ELog.Application.RM.RMPutaway
{
    public interface IRMPutawayAppService : IApplicationService
    {
        public Task<IActionResult> ValidateLocation(string Location);
        public Task<IActionResult> PutawayAndValidateBarcode(string Location, string barCode);
    }
}