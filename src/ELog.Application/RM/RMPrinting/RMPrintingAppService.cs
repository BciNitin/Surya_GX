using Abp.Application.Services;
using ELog.Core.Authorization;
using Ionic.Zip;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using MobiVUE.Utility;
using MobiVueEVO.BL;
using MobiVueEVO.BL.Common;
using MobiVueEVO.BO;
using MobiVueEVO.DAL;
using Nancy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace MobiVueEVO.Application
{
    [PMMSAuthorize]
    public class RMPrintingAppService : ApplicationService, IRMPrintingAppService
    {
        private RMPrintingBL blobj;
        private string Message;
        private string Result;
        string message;
        public RMPrintingAppService(IConfiguration configuration)
        {
            DBConfig.Configuration = configuration;
            blobj = new RMPrintingBL();
        }

        [PMMSAuthorize]
        public async Task<IActionResult> GetGRN()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.GetGRN();

                //if (dt.Rows.Count == 0 || dt is null)
                //{
                //    return new OkObjectResult("No Record Found");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new OkObjectResult(dt);
        }
        public async Task<IActionResult> GetGRNLineNo(string sGRNNO)
        {
            DataTable dt = new DataTable();
            try
            {
                if (string.IsNullOrWhiteSpace(sGRNNO))
                {
                    return new BadRequestObjectResult("ERROR~Please Enter GRN No");
                }
                sGRNNO = sGRNNO?.Trim();

                dt = await blobj.BindGRNLineNo(sGRNNO);

                if (dt.Rows.Count == 0 || dt is null)
                {
                    return new OkObjectResult($"ERROR~No record found for scanned GRN No: {sGRNNO}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkObjectResult(dt);
        }
        public async Task<IActionResult> GetGRNDetails(string sGRNNO, string GRNLineNo)
        {
            DataTable dt = new DataTable();
            try
            {
                if (string.IsNullOrWhiteSpace(sGRNNO))
                {
                    return new BadRequestObjectResult("ERROR~Please scan GRN No");
                }
                sGRNNO = sGRNNO?.Trim();

                dt = await blobj.BindGRNDetail(sGRNNO, GRNLineNo);

                if (dt.Rows.Count == 0 || dt is null)
                {
                    return new OkObjectResult($"ERROR~No record found for scanned GRN No: {sGRNNO}"); ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkObjectResult(dt);
        }

        [PMMSAuthorize]
        public async Task<IActionResult> GRNPrinting(string sGRNNo, string sDocumentType,
            string sRecevingTransaction, string sGRPODate, string sTransactionType,
            string sGRNLineNo, decimal sGRNQty, decimal sRemqty, string sSupplierCode, string sSupplierName,
            string sBatchNo, string sPoNo, string sItemNo, string sPOLineNo,
            string sPoScheduleNo, string sNumberofprint, int spacksize)
        {
            string sResult = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (string.IsNullOrWhiteSpace(sGRNNo))
                {
                    return new OkObjectResult("ERROR~Please scan GRN No");
                }
                if (string.IsNullOrWhiteSpace(sDocumentType))
                {
                    return new OkObjectResult("ERROR~Please select document type");
                }
                if (string.IsNullOrWhiteSpace(sRecevingTransaction))
                {
                    return new OkObjectResult("ERROR~Please scan GRN No");
                }
                if (string.IsNullOrWhiteSpace(sNumberofprint))
                {
                    return new OkObjectResult("ERROR~Please Enter Numeric No.");
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(sNumberofprint, "[^0-9]"))
                {
                    return new OkObjectResult("ERROR~Please Enter Valid Numeric No.");
                }
                if (sNumberofprint == null || sNumberofprint == "0")
                {
                    return new OkObjectResult("ERROR~Please Enter Valid Numeric No.");
                }
                if (Convert.ToInt32(sNumberofprint) > sRemqty)
                {
                    return new OkObjectResult("ERROR~NO of Print cannot be greater than REM qty");
                }
                if (Convert.ToDecimal(spacksize) > Convert.ToDecimal(sRemqty))
                {
                    return new OkObjectResult("ERROR~Pack size can't be greater then remaining quantity");

                }
                if (Convert.ToDecimal(Convert.ToDecimal(sNumberofprint) * Convert.ToDecimal(spacksize))
                  > Convert.ToDecimal(sRemqty))
                {

                    return new OkObjectResult("ERROR~Print quantity can't be greater than remaining quantity");
                }

                List<string> prnList = new List<string>();
                LabelToPrint prnSave = null;
                for (int i = 0; i < Convert.ToInt32(sNumberofprint); i++)
                {
                    dt = await blobj.GRNPrinting(sGRNNo, sDocumentType, sRecevingTransaction,
                        sGRPODate, sTransactionType, sGRNLineNo, sGRNQty, sRemqty, sSupplierCode, sSupplierName,
                        sBatchNo, sPoNo, sItemNo, sPOLineNo, sPoScheduleNo, spacksize);

                    if (dt.Rows.Count > 0)
                    {
                        message = dt.Rows[0][0].ToString().Trim();

                        if (message.StartsWith("SUCCESS"))
                        {

                            var prn = blobj.PrintGRNLabel(sPOLineNo, sGRNNo).Result;
                            prnList.Add(prn);
                            // return list from upper method and take it in a varibal eof same type
                            // create an object of labeltoprint and pass the parameters present in it
                            /// save this object data to db
                        }
                        else
                        {
                            return new OkObjectResult("ERROR~Printing Failed");
                        }
                    }
                }

                var jsonPrnList = JsonConvert.SerializeObject(prnList);


                prnSave = new LabelToPrint() { Labels = jsonPrnList, PrinterType = PrinterType.LAN, PrinterName = "192.168.100.31", PrinterPort = 9100, Qty = Convert.ToInt32(sNumberofprint), Site = new KeyValue<long, string>((long)AbpSession.UserId, "") };
                prnSave = new LabelsToPrint().Save(prnSave);


                //bool fileSaved = await LabelPrinting.ZipAndDownloadPRNFilesAsync();


                if (message.StartsWith("SUCCESS"))
                {
                    sResult = "SUCCESS~Label Prinited Successfully";
                }
                else
                {
                    sResult = "ERROR~Label Printing failed";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new OkObjectResult($"ERROR~{ex.Message}");
            }

            return new OkObjectResult(sResult);
        }


        public async Task<IActionResult> GetREMQTY(string sGRNNO, string sGRNLineNO)
        {
            DataTable dt = new DataTable();
            try
            {
                if (string.IsNullOrWhiteSpace(sGRNNO))
                {
                    return new BadRequestObjectResult("ERROR~Please scan GRN No");
                }
                sGRNNO = sGRNNO?.Trim();

                dt = await blobj.GetREMQTY(sGRNNO, sGRNLineNO);

                if (dt.Rows.Count == 0 || dt is null)
                {
                    return new OkObjectResult($"ERROR~No record found for scanned GRN No: {sGRNNO}"); ;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkObjectResult(dt);
        }
        public async Task<IActionResult> GetPrintDetail(string sGRNNO, string sGRNLineNO)
        {
            DataTable dt = new DataTable();
            try
            {
                if (string.IsNullOrWhiteSpace(sGRNNO))
                {
                    return new BadRequestObjectResult("ERROR~Please scan GRN No");
                }
                sGRNNO = sGRNNO?.Trim();

                dt = await blobj.GetPrintDetail(sGRNNO, sGRNLineNO);

                if (dt.Rows.Count == 0 || dt is null)
                {
                    return new OkObjectResult($"ERROR~No record found for scanned GRN No: {sGRNNO}"); ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
            return new OkObjectResult(dt);
        }


    }
}