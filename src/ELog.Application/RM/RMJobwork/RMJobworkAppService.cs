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
    public class RMJobWorkAppService : ApplicationService, IRMJobWorkAppService
    {
        private readonly RMJobworkBL blobj;
        private string Message;
        private string Result;
        public RMJobWorkAppService(IConfiguration configuration)
        {
            DBConfig.Configuration = configuration;
            blobj = new RMJobworkBL();
        }

        [PMMSAuthorize]
        public async Task<IActionResult> GetIssueSlipNo()
        {
            DataTable dt = new DataTable();
            try
            {

                //if (string.IsNullOrWhiteSpace(orgCode))
                //{
                //    return new OkObjectResult("ERROR~Please enter organization code");
                //}


                dt = await blobj.GetIssueSlipNo();

                //if (!ValidatorUtility.IsDtValid(dt))
                //{
                //    return new OkObjectResult($"ERROR~No Material Document found for Organization code : {orgCode}");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new OkObjectResult($"ERROR~{ex.Message}");
            }

            return new OkObjectResult(dt);
        }

        public async Task<IActionResult> GetItemCode(string issueSlipNo)
        {
            DataTable dt = new DataTable();
            try
            {
                if (string.IsNullOrWhiteSpace(issueSlipNo))
                {

                    return new OkObjectResult("ERROR~Please select material doc no");

                }
                //if (string.IsNullOrWhiteSpace(orgCode))
                //{
                //    return new BadRequestObjectResult("ERROR~Please enter Organization code");
                //}

                dt = await blobj.GetItemCode(issueSlipNo.Trim());

                //if (!ValidatorUtility.IsDtValid(dt))
                //{
                //    return new OkObjectResult($"ERROR~No Item Code found againt Slip No : {issueSlipNo} and Organization code : {orgCode}");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new OkObjectResult($"ERROR~{ex.Message}");
            }

            return new OkObjectResult(dt);
        }

        public async Task<IActionResult> GetItemLineNo(string issueSlipNo, string itemNumber
            )
        {
            DataTable dt = new DataTable();
            try
            {

                

                if (string.IsNullOrWhiteSpace(itemNumber))
                {
                    return new OkObjectResult("ERROR~Please select Item Code");
                }


                dt = await blobj.GetItemLineNo(issueSlipNo.Trim(), itemNumber.Trim());

                //if (!ValidatorUtility.IsDtValid(dt))
                //{
                //    return new OkObjectResult($"ERROR~No Item Line No. found againt Slip No : {issueSlipNo} and Item Code : {itemNumber}");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new OkObjectResult($"ERROR~{ex.Message}");
            }
            return new OkObjectResult(dt);
        }

        public async Task<IActionResult> GetGridDetail(string issueSlipNo, string itemNo, string itemLineNo
           )
        {
            DataTable dt = new DataTable();
            try
            {

                if (string.IsNullOrWhiteSpace(itemNo))
                {
                    return new OkObjectResult("ERROR~Please select Item Code");
                }


                dt = await blobj.GetGridDetail(issueSlipNo.Trim(), itemNo.Trim(), itemLineNo.Trim());

                //if (!ValidatorUtility.IsDtValid(dt))
                //{
                //    return new OkObjectResult($"ERROR~No Grid Record found");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new OkObjectResult($"ERROR~{ex.Message}");
            }

            return new OkObjectResult(dt);
        }

        public async Task<IActionResult> SaveJobWork(string issueSlipNo, string itemNo, string itemLineNo,
            string barCode, string location)
        {
            try
            {
                string result = string.Empty;
                DataTable dtResult = await blobj.SaveJobWork(issueSlipNo, itemNo, itemLineNo, barCode, location);

                if (dtResult.Rows.Count > 0)
                {
                    Result = dtResult.Rows[0][0].ToString();
                    Message = Result.Split('~')[1].ToString();
                    if (Result.Contains("SUCCESS"))
                    {
                        Message = $"SUCCESS~{Message}";
                    }
                    else
                    {
                        Message = $"ERROR~{Message}";
                    }

                    //string[] msg = result.Split('~');
                    //string Msgs = msg[0];
                    //if (result.Length > 0)
                    //{
                    //    if (result.StartsWith("N~"))
                    //    {
                    //        // GetOrderDetails();
                    //        //GetRMJobWork(ISSUE_NO.Trim(), PART_CODE.Trim(), ITEM_LINE_NO.ToString());
                    //    }
                    //    else
                    //    {
                    //        //GetRMJobWork(ISSUE_NO.Trim(), PART_CODE.Trim(), ITEM_LINE_NO.ToString());
                    //        // GetOrderDetails();
                    //    }
                    //}
                    //else
                    //{
                    //}
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new OkObjectResult($"ERROR~{ex.Message}");
            }

            return new OkObjectResult(Message);
        }
    }
}
