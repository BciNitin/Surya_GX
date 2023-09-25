using Abp.Application.Services;
using Abp.Notifications;
using Common;
using ELog.Core.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MobiVUE;
using MobiVueEVO.BL;
using MobiVueEVO.DAL;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ELog.Application
{
    [PMMSAuthorize]
    public class RMIssueToShopFloorAppService : ApplicationService, IRMIssueToShopFloorAppService
    {
        private string Message;
        private string Result;
        private readonly RMIssueToShopFloorBL blobj;
        public RMIssueToShopFloorAppService(IConfiguration configuration)
        {
            DBConfig.Configuration = configuration;
            blobj = new RMIssueToShopFloorBL();
        }

        [PMMSAuthorize]
        public async Task<IActionResult> GetRMWORKORDERNO()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = blobj.GetWORKORDERNO();
                if (dt.Rows.Count == 0 || dt is null)
                {
                    return new OkObjectResult("ERROR~No record found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new OkObjectResult(dt);
        }
        public async Task<IActionResult> FillFGITEMCODE(string sWORKORDERNO)
        {
            DataSet ds = new DataSet();
            try
            {
                if (string.IsNullOrWhiteSpace(sWORKORDERNO))
                {
                    return new OkObjectResult("ERROR~Please scan work order number.");
                }

                ds = blobj.FETCHFGITEMCODE(sWORKORDERNO);
                if (ds.Tables.Count == 0 || ds is null)
                {
                    return new OkObjectResult("ERROR~No record found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new OkObjectResult(ds);

        }
        public async Task<IActionResult> ValidateLocation(string _WORKORDERNO, string _ScanLocation)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = blobj.GETVILIDATELOCATION(_WORKORDERNO, _ScanLocation);
                Result = dt.Rows[0][0].ToString();
                //Message = Result.Split('~')[1].ToString();

                if (Result.Contains("SUCCESS"))
                {
                    Message = "SUCCESS~Valid Location";
                }
                else
                {
                    Message = "ERROR~Invalid Location";
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;
            }

            return new OkObjectResult(Message);
        }

        public async Task<IActionResult> ScanIssueBarcode(string _WORKORDERNO, string _FGItemCode, string _ScanLocation, string sPartBarcode)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = blobj.sScanIssueBarcode(_WORKORDERNO, _FGItemCode, _ScanLocation, sPartBarcode);

                Result = dt.Rows[0][0].ToString();
                Message = Result.Split('~')[1];
                if (Result.StartsWith("SUCCESS~"))
                {
                    Message = $"SUCCESS~{Message}";
                }
                else
                {
                    Message = $"ERROR~{Message}";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new OkObjectResult(Message);
        }
    }
}
