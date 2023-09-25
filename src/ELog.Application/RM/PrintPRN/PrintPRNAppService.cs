using Abp.Application.Services;
using ELog.Application.RM.RMPutaway;
using ELog.Core.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MobiVueEVO.BL;
using MobiVueEVO.DAL;
using System;
using MobiVueEVO.BO;
using System.Data;
using System.Threading.Tasks;

namespace MobiVueEVO.Application
{
   
    public class PrintPRNAppService : ApplicationService
    {
        private readonly PrintPRNBL blobj;
        private string Message;
        private string Result;
        public PrintPRNAppService(IConfiguration configuration)
        {
            DBConfig.Configuration = configuration;
            blobj = new PrintPRNBL();
        }


        public PrnData LabelPrnToPrint(string plantCode)
        {
            PrnData prn = null;
            try
            {
                var labelToPrintBL = new PrintPRNBL();
                var labelToPrint = labelToPrintBL.FetchLabelToPrint(plantCode);
                prn = new PrnData()
                {
                    Id = labelToPrint.ID,
                    Labels = labelToPrint.Labels,
                    Port = labelToPrint.PrinterPort,
                    PrinterName = labelToPrint.PrinterName,
                    PrinterType = (int)labelToPrint.PrinterType,
                    Qty = labelToPrint.Qty
                };
                return prn;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return prn;
        }

        ///// ---
        //public ActionResult LabelPrnToPrint(string plantCode)
        //{
        //    PrnData prn = null;
        //    try
        //    {
        //        var labelToPrintBL = new PrintPRNBL();
        //        var labelToPrint = labelToPrintBL.FetchLabelToPrint(plantCode);

        //        if (labelToPrint != null)
        //        {
        //            prn = new PrnData()
        //            {
        //                Id = labelToPrint.ID,
        //                Labels = labelToPrint.Labels,
        //                Port = labelToPrint.PrinterPort,
        //                PrinterName = labelToPrint.PrinterName,
        //                PrinterType = (int)labelToPrint.PrinterType,
        //                Qty = labelToPrint.Qty
        //            };
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        // Handle the exception (optional)
        //        return null;
        //    }

        //    return new JsonResult(prn);
        //}
        /// </summary>
        
        /// <param name="id"></param>

        public void PRNRemove(long id)
        {

             try
            {
                var labelToPrintBL = new PrintPRNBL();
                labelToPrintBL.DeleteLabelToPrint(id, (long)AbpSession.UserId);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

       
    }
}