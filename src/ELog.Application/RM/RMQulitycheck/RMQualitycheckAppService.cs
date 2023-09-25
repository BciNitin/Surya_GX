using Abp.Application.Services;
using ELog.Core.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MobiVueEVO.BL;
using MobiVueEVO.BL.Common;
using MobiVueEVO.DAL;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ELog.Application
{
    [PMMSAuthorize]
    public class RMQualityCheckAppService : ApplicationService, IRMQualityCheckAppService
    {
        private readonly RMQualityCheckBL blobj;
        private string Message;
        private string Result;
        public RMQualityCheckAppService(IConfiguration configuration)
        {
            DBConfig.Configuration = configuration;
            blobj = new RMQualityCheckBL();
        }

        [PMMSAuthorize]
        public async Task<IActionResult> GetGRNNo()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.GetGRNNo();
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

        public async Task<IActionResult> BindInventoryDetail(string sGRNNo)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.BindInventoryDetail(sGRNNo);
                if (dt.Rows.Count == 0 || dt is null)
                {
                    return new OkObjectResult($"ERROR~No record found for scanned barcode : {sGRNNo}");
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new OkObjectResult(dt);
        }
        public async Task<IActionResult> SCANBARCODEBindInventoryDetail(string sGRNNo, string ItemNO)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.SCANBARCODEBindInventoryDetail(sGRNNo, ItemNO);
                if (dt.Rows.Count == 0 || dt is null)
                {
                    return new OkObjectResult($"ERROR~No record found for scanned barcode : {sGRNNo}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new OkObjectResult(dt);
        }
        public async Task<IActionResult> SaveQualityCheckFullBatch(int status,
             string sGRNNo, string sItemNo, string sRemarks, bool isFullBatch)
        {
            DataTable dt = new DataTable();
            try
            {
                
                sItemNo = sItemNo.Trim();
                if (string.IsNullOrWhiteSpace(sItemNo))
                {
                    return new OkObjectResult("ERROR~Please scan atleast one record");
                }

                if (string.IsNullOrWhiteSpace(sGRNNo))
                {
                    return new OkObjectResult("ERROR~Please select GRN No");
                }
                string[] itemNoArray = sItemNo.Split(',');

                DataTable dtItemNos = LabelPrinting.GenerateDataTableFromArray(itemNoArray);
                DataTable dtItemDetail = await blobj.GetItemDetail(dtItemNos, sGRNNo);

                dt = await blobj.SaveQualityCheckFullBatch("SAVE", status, sGRNNo, dtItemNos, sRemarks, isFullBatch);
                Result = dt.Rows[0][0].ToString();
                Message = Result.Split('~')[1].ToString();
                if (Result.Contains("SUCCESS"))
                {
                    Message = $"SUCCESS~{Message}";
                }
                else
                {
                    Message = $"ERROR~{Message}";
                }
                if (dt.Rows.Count == 0 || dt is null)
                {
                    return new OkObjectResult("ERROR~No record found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new OkObjectResult(Message);
        }
    }
}
