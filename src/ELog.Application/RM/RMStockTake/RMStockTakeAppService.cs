using Abp.Application.Services;
using ELog.Core.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MobiVueEVO.BL;
using MobiVueEVO.BL.Common;
using MobiVueEVO.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static ELog.Application.RMStockTakeAppService;

namespace ELog.Application
{
    [PMMSAuthorize]
    public class RMStockTakeAppService : ApplicationService, IRMStockTakeAppService
    {
        private string Result;
        private string Message;
        private readonly RMStockTakeBL blobj;
        public RMStockTakeAppService(IConfiguration configuration)
        {
            DBConfig.Configuration = configuration;
            blobj = new RMStockTakeBL();
        }
        [PMMSAuthorize]
        public async Task<DataTable> GetItemNo()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.GetItemNo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dt;
        }

        [PMMSAuthorize]
        public async Task<DataTable> GetZone()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.GetZone();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dt;
        }

        [PMMSAuthorize]
        public async Task<IActionResult> GetLocation(string Location)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.GetLocation(Location);
                if (dt.Rows.Count > 0)
                {
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

                }
            }


            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return new OkObjectResult(Message);
        }

        public async Task<IActionResult> SaveFullStock(string sMaterial,
            string sZone, string sLocation, List<StockTakeModel> stockTakeModel)
        {
            DataTable dt = new DataTable();
            try
            {
                //if (string.IsNullOrWhiteSpace(sMaterial))
                //{
                //    return new OkObjectResult("ERROR~Please Select Material");
                //}

                //if (string.IsNullOrWhiteSpace(sZone))
                //{
                //    return new OkObjectResult("ERROR~Please select Zone");
                //}
                //if (string.IsNullOrWhiteSpace(sLocation))
                //{
                //    return new OkObjectResult("ERROR~Please Enter Location");
                //}

                DataTable dtTable = ConvertToDataTable(stockTakeModel);


                dt = await blobj.SaveFullStock(dtTable);
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

        [PMMSAuthorize]
        public async Task<DataTable> GetPhysicalAdviceNo()
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.GetPhysicalAdviceNo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dt;
        }

        [PMMSAuthorize]
        public async Task<DataTable> GetPhysicalDetails(string AdviceNo)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.GetPhysicalDetails(AdviceNo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dt;
        }
        [PMMSAuthorize]
        public async Task<DataTable> GetScanItem(string ITEMNO)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.GetScanItem(ITEMNO);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dt;
        }

        [PMMSAuthorize]
        public async Task<DataTable> SaveFullPhysicalStock(string AdviceNo, string MaterialCode,
          string ScanItem, string Systemqty, string PhysicalStock, string Remarks)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.SaveFullPhysicalStock(AdviceNo, MaterialCode, ScanItem, Systemqty, 
                    PhysicalStock, Remarks);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dt;
        }
        [PMMSAuthorize]
        public string GetPhysicalStock(string sStockArea, string sBarcode)
        {
            var message = "";
            try
            {
                var StockTakeBL = new RMStockTakeBL();

                DataTable dt = StockTakeBL.GetPhysicalStock(sStockArea, sBarcode);

                if (dt.Rows.Count > 0)
                {
                    decimal dQty = Convert.ToDecimal(dt.Rows[0]["APPROVED_QTY"].ToString());
                    if (Convert.ToDecimal(dQty) == 0)
                    {
                        message = ("Qty not found in inventory area for scanned barcode");

                        return message;
                    }
                    else
                    {
                        //if approved quality is not zero then submit button display and quanity will be appear on quantity text box
                        if (dQty.ToString() != null)
                        {
                            message = Convert.ToString(dQty);
                            return message;
                        }
                        //if approved quality is null then display erro message
                        else if (dQty.ToString() == null)
                        {
                            message = ("Qty not found in inventory area for scanned barcode");

                            return message;
                        }
                    }
                }
                else
                {
                    return message = ("No result found against scanned barcode");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return message;
        }

        [PMMSAuthorize]
        public async Task<DataTable> GetStockReconciliationDetails(string AdviceNo)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = await blobj.GetStockReconciliationDetails(AdviceNo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dt;
        }

        [PMMSAuthorize]
        public async Task<DataTable> UpdateFullStockReconciliation(List<StockReconciliationModel> StockReconciliationModel)
        {
            DataTable dt = new DataTable();
            try
            {
                DataTable dtTable = ConvertToDataTable(StockReconciliationModel);
                dt = await blobj.UpdateFullStockReconciliation(dtTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dt;
        }


        public class StockTakeModel
        {
            public string MaterialCode { get; set; }
            public string Zone { get; set; }
            public string Location { get; set; }
        }

        public class StockReconciliationModel
        {
            public string AdviceNo { get; set; }
            public string MaterialCode { get; set; }
            public string Physicalstock { get; set; }
        }

        private DataTable ConvertToDataTable(List<StockTakeModel> stockTakeModelList)
        {
            DataTable dataTable = new DataTable();

            // Add columns to the DataTable
            dataTable.Columns.Add("MaterialCode", typeof(string));
            dataTable.Columns.Add("Zone", typeof(string));
            dataTable.Columns.Add("Location", typeof(string));

            // Add rows to the DataTable
            foreach (var item in stockTakeModelList)
            {
                DataRow row = dataTable.NewRow();
                row["MaterialCode"] = item.MaterialCode;
                row["Zone"] = item.Zone;
                row["Location"] = item.Location;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
        private DataTable ConvertToDataTable(List<StockReconciliationModel> StockReconciliationModelLIst)
        {
            DataTable dataTable = new DataTable();

            // Add columns to the DataTable
            dataTable.Columns.Add("AdviceNo", typeof(string));
            dataTable.Columns.Add("MaterialCode", typeof(string));
            dataTable.Columns.Add("Physicalstock", typeof(string));

            // Add rows to the DataTable
            foreach (var item in StockReconciliationModelLIst)
            {
                DataRow row = dataTable.NewRow();
                row["AdviceNo"] = item.AdviceNo;
                row["MaterialCode"] = item.MaterialCode;
                row["Physicalstock"] = item.Physicalstock;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}
