using Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MobiVueEVO.BO;

namespace ELog.Application.RM.RMPutaway
{
    public interface IPrintPRNAppService : IApplicationService
    {
        public PrnData LabelPrnToPrint(string plantCode);
       // public ActionResult LabelPrnToPrint(string plantCode);
        public void PRNRemove(long id);
    }
}