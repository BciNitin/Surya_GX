using Abp.Application.Services;
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
    public class RMSFGReturnToRMWarehouseAppService : ApplicationService,IRMSFGReturnToRMWarehouseAppService
    {
        private string Result;
        private string Message;
        private readonly RMSFGReturnToRMWarehouseBL blobj;
        public RMSFGReturnToRMWarehouseAppService(IConfiguration configuration)
        {
            DBConfig.Configuration = configuration;
            blobj = new RMSFGReturnToRMWarehouseBL();
        }

        [PMMSAuthorize]
        public async Task<IActionResult> GetMaterialDocument()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.GetMaterialDocument();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new OkObjectResult(ex.Message);
            }

            return new OkObjectResult(dt);
        }

        public async Task<IActionResult> GetItemNo(string slipNo)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.GetItemNo(slipNo.Trim());
                if (dt.Rows.Count > 0)
                {
                    Result = dt.Rows[0][0].ToString();
                    Message = Result.Split('~').ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }

            return new OkObjectResult(dt);
        }
        public async Task<IActionResult> GetItemLineNo(string slipNo, string itemNo)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.GetItemLineNo(slipNo.Trim(), itemNo.Trim());
                if (dt.Rows.Count > 0)
                {
                    Result = dt.Rows[0][0].ToString();
                    Message = Result.Split('~').ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }

            return new OkObjectResult(dt);
        }

        public async Task<IActionResult> SFGReturnToRMWarehouse(string slipNo, string itemNo,
            string location, string barcode)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.SFGReturnToRMWarehouse(slipNo.Trim(), itemNo.Trim(), location.Trim(), barcode.Trim());
                if (dt.Rows.Count > 0)
                {
                    Result = dt.Rows[0][0].ToString();
                    Message = Result.Split('~').ToString();

                    if (Result.StartsWith("SUCCESS~"))
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkObjectResult(Result);
        }

        //public async Task<IActionResult> GetRMPrintBarcodeFromWIPIssue(string _MRNNo, string _Part_Barcode,
        //    string sScanBy, decimal dReturnQty, string sSiteCode, string _PartCode, string sWorkOrderNo,
        //    string sScanType, string sUserID, string sLineCode, string sRemarks)
        //{
        //    dynamic Message = null;
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        dt = await blobj.RMPrintBarcode(_MRNNo, _Part_Barcode, sScanBy,
        //    dReturnQty, sSiteCode, _PartCode, sWorkOrderNo, sScanType, sUserID, sLineCode
        //        , sRemarks
        //            );
        //        Message = dt;
        //        string[] msg = Message.Split('~');
        //        string Msgs = msg[0];
        //        if (Message.Length > 0)
        //        {
        //            if (msg[0].StartsWith("N~") || msg[0].StartsWith($"{GlobalSettings.Error}"))
        //            {
        //                return Message;
        //            }
        //            else
        //            {
        //                return Message;
        //            }
        //        }
        //        else
        //        {
        //            return Message = "No result found, Please try again";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return new BadRequestObjectResult(ex.Message);
        //    }
        //}
    }
}
