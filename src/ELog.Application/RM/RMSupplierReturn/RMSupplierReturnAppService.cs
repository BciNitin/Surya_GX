using Abp.Application.Services;
using Common;
using ELog.Core.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MobiVueEVO.BL;
using MobiVueEVO.DAL;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ELog.Application
{
    [PMMSAuthorize]
    public class RMSupplierReturnAppService : ApplicationService, IRMSupplierReturnAppService
    {
        private readonly RMSupplierReturnBL blobj;
        public RMSupplierReturnAppService(IConfiguration configuration)
        {
            DBConfig.Configuration = configuration;
            blobj = new RMSupplierReturnBL();
        }

        [PMMSAuthorize]
        public async Task<IActionResult> GetDocumentNo()
        {
            DataTable dtmsg = new DataTable();
            try
            {
                DataTable dt = new DataTable();
                dt = await blobj.GetDocumentNo();
                if (dt.Rows.Count > 0)
                {
                    dtmsg = dt;
                }
                else
                {
                    dtmsg = dt;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex;
            }
            return new OkObjectResult(dtmsg);
        }

        public async Task<IActionResult> GetItemCode(string sDocumentNo)
        {
            DataTable dtmsg = new DataTable();
            try
            {
                DataTable dt = new DataTable();
                dt = await blobj.GetItemCode(sDocumentNo);
                if (dt.Rows.Count > 0)
                {
                    dtmsg = dt;
                }
                else
                {
                    dtmsg = dt;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex;
            }
            return new OkObjectResult(dtmsg);
        }

        public async Task<IActionResult> GetItemLineNo(string sDocumentNo, string sItemCode)
        {
            DataTable dtmsg = new DataTable();
            try
            {
                DataTable dt = new DataTable();
                dt = await blobj.GetItemLineNo(sDocumentNo, sItemCode);
                if (dt.Rows.Count > 0)
                {
                    dtmsg = dt;
                }
                else
                {
                    dtmsg = dt;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex;
            }
            return new OkObjectResult(dtmsg);
        }

        public async Task<IActionResult> GetOrderDetails(string sDocumentNo, string sItemCode,
            string sItemLineNo)
        {
            DataTable dtmsg = new DataTable();
            try
            {
                DataTable dt = new DataTable();
                dt = await blobj.GetOrderDetails(sDocumentNo, sItemCode, sItemLineNo);
                if (dt.Rows.Count > 0)
                {
                    dtmsg = dt;
                }
                else
                {
                    dtmsg = dt;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex;
            }
            return new OkObjectResult(dtmsg);
        }

        public async Task<IActionResult> SaveSupplierReturnBarcode(string sOrderNo,
            string sPartCode, string sLineNo, string sLocationCode, string sScannedBarcode,
            string UserID, string sSiteCode, string sLineCode)
        {
            dynamic message = "";
            try
            {
                if (sOrderNo == "0" || sOrderNo == "-1")
                {
                    message = "Order number is not selected";
                    return message;
                }
                if (sPartCode == "0" || sPartCode == "-1")
                {
                    message = "Item code is not selected";
                    return message;
                }
                if (string.IsNullOrEmpty(sLocationCode.Trim()))
                {
                    message = "Item code is not selected";
                    return message;
                }
                if (string.IsNullOrEmpty(sScannedBarcode.Trim()))
                {
                    message = "Please scan barcode";
                    return message;
                }
                string _Result = string.Empty;
                DataTable dtResult = await blobj.SaveSupplierReturnData(sOrderNo.ToString(),
                    sPartCode.ToString(), sLineNo.ToString(), sLocationCode.Trim(), 
                    sScannedBarcode.Trim(), "1", "4", "1");
                if (dtResult.Rows.Count > 0)
                {
                    _Result = dtResult.Rows[0][0].ToString();
                    string[] msg = _Result.Split('~');
                    string Msgs = msg[0];
                    if (_Result.Length > 0)
                    {
                        if (_Result.StartsWith("N~"))
                        {
                            message = "ERROR~" + msg[1];
                        }
                        else
                        {
                            message = "SUCCESS~" + msg[1];
                        }
                    }
                    else
                    {
                        message = "ERROR~No result found against scanned barcode";
                    }
                }
                else
                {
                    message = "No result found against scanned barcode";
                }
            }
            catch (Exception ex)
            {
                CommonHelper.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, System.Reflection.Assembly.GetExecutingAssembly().GetName() + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkObjectResult(message); ;
        }
    }
}
